using Bakery.Core.Contracts;
using Bakery.Models.BakedFoods;
using Bakery.Models.BakedFoods.Contracts;
using Bakery.Models.BakedFoods.Models;
using Bakery.Models.Drinks;
using Bakery.Models.Drinks.Contracts;
using Bakery.Models.Drinks.Models;
using Bakery.Models.Tables;
using Bakery.Models.Tables.Contracts;
using Bakery.Models.Tables.Models;
using Bakery.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bakery.Core
{
    public class Controller : IController
    {
        private List<IBakedFood> bakedFoods;
        private List<IDrink> drinks;
        private List<ITable> tables;
        private decimal totalIncome;
        public Controller()
        {
            bakedFoods = new List<IBakedFood>();
            drinks = new List<IDrink>();
            tables = new List<ITable>();
        }

        public string AddDrink(string type, string name, int portion, string brand)
        {
            IDrink drink;           
            if (type == nameof(Tea))
            {
                drink = new Tea(name, portion, brand);
            }
            else
            {
                drink = new Water(name, portion, brand);
            }
            drinks.Add(drink);
            return string.Format(OutputMessages.DrinkAdded, drink.Name, drink.Brand);
        }

        public string AddFood(string type, string name, decimal price)
        {
            IBakedFood food;
            if (type == nameof(Bread))
            {
                food = new Bread(name, price);
            }
            else
            {
                food = new Cake(name, price);
            }
            bakedFoods.Add(food);
            return string.Format(OutputMessages.FoodAdded, food.Name, food.GetType().Name);
        }

        public string AddTable(string type, int tableNumber, int capacity)
        {
            ITable table;
            if (type == nameof(InsideTable))
            {
                table = new InsideTable(tableNumber, capacity);
            }
            else
            {
                table = new OutsideTable(tableNumber, capacity);
            }
            tables.Add(table);
            return string.Format(OutputMessages.TableAdded, table.TableNumber);
        }

        public string GetFreeTablesInfo()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var table in tables.Where(x => x.IsReserved == false).ToList())
            {
                sb.AppendLine(table.GetFreeTableInfo());
            }
            return sb.ToString().TrimEnd();
        }

        public string GetTotalIncome()
        {            
            return $"Total income: {totalIncome:f2}lv";
        }

        public string LeaveTable(int tableNumber)
        {
            ITable table = tables.FirstOrDefault(x => x.TableNumber == tableNumber);
            decimal bill = table.GetBill();
            totalIncome += bill;
            table.Clear();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Table: {tableNumber}");
            stringBuilder.AppendLine($"Bill: {bill:f2}");
            return stringBuilder.ToString().TrimEnd();
        }

        public string OrderDrink(int tableNumber, string drinkName, string drinkBrand)
        {
            if (!tables.Any(x => x.TableNumber == tableNumber))
            {
                return string.Format(OutputMessages.WrongTableNumber, tableNumber);
            }
            if (!drinks.Any(x => x.Name == drinkName && x.Brand == drinkBrand))
            {
                return string.Format(OutputMessages.NonExistentDrink, drinkName, drinkBrand);
            }
            ITable table = tables.FirstOrDefault(x => x.TableNumber == tableNumber);
            IDrink drink = drinks.FirstOrDefault(x=> x.Name == drinkName && x.Brand == drinkBrand);
            table.OrderDrink(drink);
            return $"Table {tableNumber} ordered {drinkName} {drinkBrand}";
        }

        public string OrderFood(int tableNumber, string foodName)
        {
            if (!tables.Any(x => x.TableNumber == tableNumber))
            {
                return string.Format(OutputMessages.WrongTableNumber,tableNumber);
            }
            if (!bakedFoods.Any(x=> x.Name == foodName))
            {
                return string.Format(OutputMessages.NonExistentFood, foodName);
            }
            ITable table = tables.FirstOrDefault(x => x.TableNumber == tableNumber);
            IBakedFood food = bakedFoods.FirstOrDefault(x => x.Name == foodName);
            table.OrderFood(food);
            return $"Table {tableNumber} ordered {foodName}";
        }

        public string ReserveTable(int numberOfPeople)
        {
            if (!tables.Any(x=> x.IsReserved == false && x.Capacity >= numberOfPeople))
            {
                return string.Format(OutputMessages.ReservationNotPossible, numberOfPeople);
            }
            ITable table = tables.FirstOrDefault(x=> x.IsReserved == false && x.Capacity >= numberOfPeople);
            table.Reserve(numberOfPeople);
            return string.Format(OutputMessages.TableReserved,table.TableNumber, numberOfPeople);
        }
    }
}
