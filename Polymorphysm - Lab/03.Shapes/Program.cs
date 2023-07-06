namespace Shapes
{
    public partial class StartUp
    {
        static void Main(string[] args)
        {
            Shape rectangle = new Rectangle(10, 20);
            Shape circle = new Circle(30);

            Console.WriteLine(rectangle.CalculateArea());

            List<Shape> shapes = new List<Shape>();

            shapes.Add(rectangle);

            shapes.Add(circle);

            foreach (var item in shapes)
            {
                Console.WriteLine(item.CalculateArea());
            }

        }
    }
}