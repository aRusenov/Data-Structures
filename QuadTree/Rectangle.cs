namespace QuadTree
{
    public class Rectangle
    {
        public Rectangle(int left, int top, int width, int height)
        {
            this.Left = left;
            this.Right = left + width;
            this.Top = top;
            this.Bottom = top + height;
        }

        public int Top { get; set; }

        public int Left { get; set; }

        public int Bottom { get; set; }

        public int Right { get; set; }

        public int Width
        {
            get { return this.Right - this.Left; }
        }

        public int Height
        {
            get { return this.Bottom - this.Top; }
        }

        public int MidX
        {
            get { return this.Left + this.Width / 2; }
        }

        public int MidY
        {
            get { return this.Top + this.Height / 2; }
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}) .. ({2}, {3})",
                this.Left, this.Top, this.Right, this.Bottom);
        }

        public bool Intersects(Rectangle other)
        {
            return this.Left <= other.Right &&
                    other.Left <= this.Right &&
                    this.Top <= other.Bottom &&
                    other.Top <= this.Bottom;
        }

        public bool IsInside(Rectangle other)
        {
            return this.Right <= other.Right &&
                   this.Left >= other.Left &&
                   this.Top >= other.Top &&
                   this.Bottom <= other.Bottom;
        }
    }
}
