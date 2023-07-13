namespace _1.SquareRoot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {               
                int inputNumber = int.Parse(Console.ReadLine());
                if (inputNumber < 0)
                {
                    throw new InvalidOperationException("Invalid number.");
                }
                Console.WriteLine(Math.Sqrt(inputNumber));
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Goodbye.");
            }
        }
    }
}