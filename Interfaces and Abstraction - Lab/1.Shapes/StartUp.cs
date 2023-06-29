using System.Security.Cryptography.X509Certificates;

namespace Shapes
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            Circle circle = new(5);
            Rectangle rectangle = new(10, 20);

            circle.Draw();
            rectangle.Draw();
        }
    }
}