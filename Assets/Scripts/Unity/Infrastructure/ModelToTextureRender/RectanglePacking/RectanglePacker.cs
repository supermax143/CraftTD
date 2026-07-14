using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Exploration.Scripts.Controllers.ModelToTextureRender.RectanglePacking
{
    /**
     * Class used to pack rectangles within container rectangle with close to optimal solution.
     */
    public class RectanglePacker
    {
        private readonly List<SortableSize> _rectangles = new();

        private readonly List<IntegerRectangle> _freeAreas = new();
        private readonly List<IntegerRectangle> _newFreeAreas = new();

        private readonly Dictionary<int, SortableSize> _idToSize = new();
        private readonly Dictionary<int, IntegerRectangle> _idToRectangle = new();

        private IntegerRectangle _outsideRectangle;

        private readonly List<SortableSize> _sortableSizeStack = new();
        private readonly List<IntegerRectangle> _rectangleStack = new();
          
        private readonly List<List<IntegerRectangle>> _pages = new();
        public int PagesCount => _pages.Count;

        public List<IntegerRectangle> GetPageRects(int pageIndex)
        {
            return _pages[pageIndex];
        }

        public int GetRectanglesCount(int pageIndex)
        {
            if (_pages.Count == 0)
            {
                return 0;
            }

            return _pages[pageIndex].Count;
        }

        public int PageWidth { get; private set; } = 0;
        public int PageHeight { get; private set; } = 0;
        public int Padding { get; private set; } = 0;

        private int _packedWidth = 0;
        private int _packedHeight = 0;


        public RectanglePacker(int width, int height, int padding = 0)
        {
            _outsideRectangle = new IntegerRectangle();
            _outsideRectangle.Update(width + 1, height + 1, 0, 0);
            Reset(width, height, padding);
        }

        public void Reset(int width, int height, int padding = 0, bool freePages = true)
        {
            if (freePages)
            {
                foreach (var page in _pages)
                {
                    while (page.Count > 0)
                    {
                        FreeRectangle(page.Pop());
                    }
                }

                _idToRectangle.Clear();
            }

            while (_freeAreas.Count > 0)
            {
                FreeRectangle(_freeAreas.Pop());
            }

            PageWidth = width;
            PageHeight = height;

            _packedWidth = 0;
            _packedHeight = 0;

            _freeAreas.Add(AllocateRectangle(0, 0, PageWidth, PageHeight));

            Padding = padding;
        }

        public IntegerRectangle GetRectangle(int page, int index, IntegerRectangle rectangle)
        {
            IntegerRectangle inserted = _pages[page][index];

            rectangle.X = inserted.X;
            rectangle.Y = inserted.Y;
            rectangle.Width = inserted.Width;
            rectangle.Height = inserted.Height;

            return rectangle;
        }

        public int GetRectangleId(int page, int index)
        {
            if (_pages.Count == 0)
            {
                return -1;
            }

            IntegerRectangle inserted = _pages[page][index];
            return inserted.ID;
        }

        public bool TryGetRectangle(int id, out IntegerRectangle rect)
        {
            return _idToRectangle.TryGetValue(id, out rect);
        }

        public int AddRectangle(int width, int height)
        {
            SortableSize sortableSize = AllocateSize(width, height);
            _rectangles.Add(sortableSize);
            return sortableSize.Id;
        }

        public void PackRectangles(Action<List<SortableSize>> sortFunction = null)
        {
            if (_rectangles == null)
            {
                return;
            }

            if (sortFunction != null)
            {
                sortFunction(_rectangles);
            }

            _pages.Clear();
            if (_rectangles.IsEmpty())
            {
                return;
            }
            PackRectangleInternal();
        }
        
        private void PackRectangleInternal(List<SortableSize> rectangles = null)
        {
            if (rectangles == null)
            {
                rectangles = _rectangles;
            }
            
            List<SortableSize> notAddedRectangles = new();
            List<IntegerRectangle> addedRectangles = new();  

            while (rectangles.Count > 0)
            {
                SortableSize sortableSize = rectangles.Pop();
                int width = sortableSize.Width;
                int height = sortableSize.Height;

                int index = GetFreeAreaIndex(width, height);
                if (index >= 0)
                {
                    IntegerRectangle freeArea = _freeAreas[index];
                    IntegerRectangle target = AllocateRectangle(freeArea.X, freeArea.Y, width, height);
                    target.ID = sortableSize.Id;
                    target.PageIndex = _pages.Count;
                    _idToRectangle[target.ID] = target;
                    // Generate the new free areas, these are parts of the old ones intersected or touched by the target
                    GenerateNewFreeAreas(target, _freeAreas, _newFreeAreas);

                    while (_newFreeAreas.Count > 0)
                    {
                        _freeAreas.Add(_newFreeAreas.Pop());
                    }

                    addedRectangles.Add(target);

                    if (target.Right > _packedWidth)
                    {
                        _packedWidth = target.Right;
                    }

                    if (target.Bottom > _packedHeight)
                    {
                        _packedHeight = target.Bottom;
                    }

                    FreeSize(sortableSize);
                }
                else
                {
                    notAddedRectangles.Add(sortableSize);
                }
            }

            if (addedRectangles.Count == 0)
            {
                Debug.Log($"{GetType().Name} empty addedRectangles");
                return;
            }

            _pages.Add(addedRectangles);

            if (notAddedRectangles.Count > 0)
            {
                Reset(PageWidth, PageHeight, Padding, false);
                PackRectangleInternal(notAddedRectangles);
                return;
            }
        }


        private void FilterSelfSubAreas(List<IntegerRectangle> areas)
        {
            for (int i = areas.Count - 1; i >= 0; i--)
            {
                IntegerRectangle filtered = areas[i];
                for (int j = areas.Count - 1; j >= 0; j--)
                {
                    if (i != j)
                    {
                        IntegerRectangle area = areas[j];
                        if (filtered.X >= area.X && filtered.Y >= area.Y && filtered.Right <= area.Right &&
                            filtered.Bottom <= area.Bottom)
                        {
                            FreeRectangle(filtered);
                            IntegerRectangle topOfStack = areas.Pop();
                            if (i < areas.Count)
                            {
                                // Move the one on the top to the freed position
                                areas[i] = topOfStack;
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void GenerateNewFreeAreas(IntegerRectangle target, List<IntegerRectangle> areas,
            List<IntegerRectangle> results)
        {
            // Increase dimensions by one to get the areas on right / bottom this rectangle touches
            // Also add the padding here
            int x = target.X;
            int y = target.Y;
            int right = target.Right + Padding;
            int bottom = target.Bottom + Padding;

            IntegerRectangle targetWithPadding = null;
            if (Padding == 0)
            {
                targetWithPadding = target;
            }

            for (int i = areas.Count - 1; i >= 0; i--)
            {
                IntegerRectangle area = areas[i];
                if (!(x >= area.Right || right <= area.X || y >= area.Bottom || bottom <= area.Y))
                {
                    if (targetWithPadding == null)
                    {
                        targetWithPadding = AllocateRectangle(target.X, target.Y, target.Width + Padding,
                            target.Height + Padding);
                    }

                    GenerateDividedAreas(targetWithPadding, area, results);
                    IntegerRectangle topOfStack = areas.Pop();
                    if (i < areas.Count)
                    {
                        // Move the one on the top to the freed position
                        areas[i] = topOfStack;
                    }
                }
            }

            if (targetWithPadding != null && targetWithPadding != target)
            {
                FreeRectangle(targetWithPadding);
            }

            FilterSelfSubAreas(results);
        }

        private void GenerateDividedAreas(IntegerRectangle divider, IntegerRectangle area,
            List<IntegerRectangle> results)
        {
            int count = 0;

            int rightDelta = area.Right - divider.Right;
            if (rightDelta > 0)
            {
                results.Add(AllocateRectangle(divider.Right, area.Y, rightDelta, area.Height));
                count++;
            }

            int leftDelta = divider.X - area.X;
            if (leftDelta > 0)
            {
                results.Add(AllocateRectangle(area.X, area.Y, leftDelta, area.Height));
                count++;
            }

            int bottomDelta = area.Bottom - divider.Bottom;
            if (bottomDelta > 0)
            {
                results.Add(AllocateRectangle(area.X, divider.Bottom, area.Width, bottomDelta));
                count++;
            }

            int topDelta = divider.Y - area.Y;
            if (topDelta > 0)
            {
                results.Add(AllocateRectangle(area.X, area.Y, area.Width, topDelta));
                count++;
            }

            if (count == 0 && (divider.Width < area.Width || divider.Height < area.Height))
            {
                // Only touching the area, store the area itself
                results.Add(area);
            }
            else
            {
                FreeRectangle(area);
            }
        }

        private int GetFreeAreaIndex(int width, int height)
        {
            IntegerRectangle best = _outsideRectangle;
            int index = -1;

            int paddedWidth = width + Padding;
            int paddedHeight = height + Padding;

            int count = _freeAreas.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                IntegerRectangle free = _freeAreas[i];
                if (free.X < _packedWidth || free.Y < _packedHeight)
                {
                    // Within the packed area, padding required
                    if (free.X < best.X && paddedWidth <= free.Width && paddedHeight <= free.Height)
                    {
                        index = i;
                        if ((paddedWidth == free.Width && free.Width <= free.Height && free.Right < this.PageWidth) ||
                            (paddedHeight == free.Height && free.Height <= free.Width))
                            break;

                        best = free;
                    }
                }
                else
                {
                    // Outside the current packed area, no padding required
                    if (free.X < best.X && width <= free.Width && height <= free.Height)
                    {
                        index = i;
                        if ((width == free.Width && free.Width <= free.Height && free.Right < this.PageWidth) ||
                            (height == free.Height && free.Height <= free.Width))
                            break;

                        best = free;
                    }
                }
            }

            return index;
        }

        private IntegerRectangle AllocateRectangle(int x, int y, int width, int height)
        {
            IntegerRectangle rectangle;
            if (_rectangleStack.Count > 0)
            {
                rectangle = _rectangleStack.Pop();
            }
            else
            {
                rectangle = new IntegerRectangle();
            }

            rectangle.Update(x, y, width, height);

            return rectangle;
        }

        private void FreeRectangle(IntegerRectangle rectangle)
        {
            _rectangleStack.Add(rectangle);
        }

        private SortableSize AllocateSize(int width, int height)
        {
            SortableSize size;

            if (_sortableSizeStack.Count > 0)
            {
                size = _sortableSizeStack.Pop();
            }
            else
            {
                size = new SortableSize(_idToSize.Count);
                _idToSize[size.Id] = size;
            }

            size.Width = width;
            size.Height = height;

            return size;
        }

        private void FreeSize(SortableSize size)
        {
            _sortableSizeStack.Add(size);
        }
    }

    static class ListExtension
    {
        public static T Pop<T>(this List<T> list)
        {
            int index = list.Count - 1;

            T r = list[index];
            list.RemoveAt(index);
            return r;
        }
    }
}