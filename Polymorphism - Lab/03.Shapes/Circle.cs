namespace Shapes
{
    public partial class StartUp
    {
        public class Circle : Shape
        {
            private double radius;

            public Circle(double radius)
            {
                this.Radius = radius;
            }

            public double Radius
            {
                get { return this.radius; }
                private set { this.radius = value; } // TODO add validation
            }

            public override double CalculateArea()
            {
                return Math.PI * Math.Pow(this.Radius, 2);
            }

            public override double CalculatePerimeter()
            {
                return 2 * Math.PI * this.Radius;
            }
        }
    }
}