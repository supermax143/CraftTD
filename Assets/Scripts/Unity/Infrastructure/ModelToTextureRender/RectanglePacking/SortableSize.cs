namespace Exploration.Scripts.Controllers.ModelToTextureRender.RectanglePacking
{
    /**
     * Class used for sorting the inserted rectangles based on the dimensions
     */
    public class SortableSize
    {
        internal int Width { get; set; }
        internal int Height { get; set; }
        internal int Id { get; private set; }

        public SortableSize(int id)
        {
            Id = id;
        }
    }
}