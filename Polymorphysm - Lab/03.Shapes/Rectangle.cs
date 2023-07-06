namespace Shapes
{
    public partial class StartUp
    {
        public class Rectangle : Shape
        {
            private double width;
            private double height;

            public Rectangle(double width, double height)
            {
                Width = width;
                Height = height;
            }

            public double Width
            {
                get { return this.width; }
                private set { this.width = value; }
            }

            public double Height
            {
                get { return this.height; }
                private set { this.height = value; }
            }

            public override double CalculateArea()
            {
                return (this.Width * this.Height);
            }

            public override double CalculatePerimeter()
            {
                return 2 * (this.Width + this.Height);
            }
        }
    }
}