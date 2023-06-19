using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03.ShoppingSpree.Models
{
    public class Person
    {
        private string name;
        private decimal money;
        private List<Product> products;

        public Person(string name,decimal money)
        {
            this.Name = name;
            this.Money = money;
            this.products = new List<Product>();
        }


        public string Name
        {
            get => this.name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name cannot be empty");
                }
                this.name = value;
            }
        }

        public decimal Money
        {
            get => this.money;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Money cannot be negative");
                }
                this.money = value;
            }
        }


        public string Add(Product product)
        {
            if (Money < product.Cost)
            {
                return $"{Name} can't afford {product}";
            }

            products.Add(product);

            Money -= product.Cost;

            return $"{Name} bought {product}";
        }

        public override string ToString()
        {
            string productsString = products.Any()
                 ? string.Join(", ", products)
                 : "Nothing bought";

            return $"{Name} - {productsString}";
        }
    }
}
