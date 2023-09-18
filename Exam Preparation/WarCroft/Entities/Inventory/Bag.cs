using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarCroft.Constants;
using WarCroft.Entities.Items;

namespace WarCroft.Entities.Inventory
{
    public abstract class Bag : IBag
    {
        private List<Item> items;
        private const int maxCapacity = 100;
        private int capacity;

        protected Bag(int capacity)
        {
            Capacity = capacity;
            items = new List<Item>();
        }

        public int Capacity
        {
            get
            {
                return capacity;
            }
            set
            {
                if (value < 0 || value > maxCapacity)
                {
                    capacity = maxCapacity;
                }
                capacity = value;
            }
        }

        public int Load => (int)items.Sum(x => x.Weight);

        public IReadOnlyCollection<Item> Items => items;

        public void AddItem(Item item)
        {
            if (item.Weight + this.Load >= Capacity)
            {
                throw new InvalidOperationException(ExceptionMessages.ExceedMaximumBagCapacity);
            }

            items.Add(item);
        }

        public Item GetItem(string name)
        {
            if (!this.items.Any())
            {
                throw new InvalidOperationException(ExceptionMessages.EmptyBag);
            }

            if (!this.items.Any(x => x.GetType().Name == name))
            {
                throw new ArgumentException(string.Format(ExceptionMessages.ItemNotFoundInBag, name));
            }

            Item item = this.items.FirstOrDefault(x => x.GetType().Name == name);
            items.Remove(item);
            return item;
        }
    }
}