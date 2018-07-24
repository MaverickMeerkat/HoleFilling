namespace HoleFilling.DataObjects
{
    /// <summary>
    /// A grayscale normalized representation of a pixel, containing value of [0,1], or -1 if holed
    /// </summary>
    public class Pixel
    {
        /// <summary>
        /// Row
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Column
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Normalized grayscale value of [0,1], or -1 if holed
        /// </summary>
        public float Value { get; set; }

        public Pixel(int x, int y, float val)
        {
            X = x;
            Y = y;
            Value = val;
        }
    }
}
