using UnityEngine;

namespace Exploration.Scripts.Controllers.ModelToTextureRender.RectanglePacking
{
    /**
     * Class used to store rectangles values inside rectangle packer
     * ID parameter needed to connect rectangle with the originally inserted rectangle
     */
    public class IntegerRectangle
    {
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        
        public int PageIndex { get; set; }

        public int Right { get; private set; }
        public int Bottom { get; private set; }

        public void Update(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

            Right = x + width;
            Bottom = y + height;
        }
    }
}