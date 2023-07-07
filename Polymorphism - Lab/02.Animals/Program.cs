namespace Animals
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            List<Animal> list = new List<Animal>
            {
            new Cat("Rosi", "Whiskas"),
            new Dog("Kiko", "Meat")
            };
            foreach (var animal in list)
            {
                Console.WriteLine(animal.ExplainSelf());
            }

        }
    }
}