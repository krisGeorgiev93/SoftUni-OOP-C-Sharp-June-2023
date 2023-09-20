using AquaShop.Core.Contracts;
using AquaShop.Models.Aquariums;
using AquaShop.Models.Aquariums.Contracts;
using AquaShop.Models.Decorations;
using AquaShop.Models.Decorations.Contracts;
using AquaShop.Models.Fish;
using AquaShop.Models.Fish.Contracts;
using AquaShop.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AquaShop.Core
{
    public class Controller : IController
    {
        private List<IDecoration> decorations;
        private List<IAquarium> aquariums;

        public Controller()
        {
            decorations = new List<IDecoration>();
            aquariums = new List<IAquarium>();
        }
        public string AddAquarium(string aquariumType, string aquariumName)
        {
            IAquarium aquarium;
            if (aquariumType == nameof(FreshwaterAquarium))
            {
                aquarium = new FreshwaterAquarium(aquariumName);
            }
            else if (aquariumType == nameof(SaltwaterAquarium))
            {
                aquarium = new SaltwaterAquarium(aquariumName);
            }
            else
            {
                throw new InvalidOperationException("Invalid aquarium type.");
            }
            aquariums.Add(aquarium);
            return $"Successfully added {aquariumType}.";
        }

        public string AddDecoration(string decorationType)
        {
            IDecoration decoration;

            if (decorationType == nameof(Ornament))
            {
                decoration = new Ornament();
            }
            else if (decorationType == nameof(Plant))
            {
                decoration = new Plant();
            }
            else
            {
                throw new InvalidOperationException("Invalid decoration type.");
            }
            decorations.Add(decoration);
            return $"Successfully added {decorationType}.";
        }

        public string AddFish(string aquariumName, string fishType, string fishName, string fishSpecies, decimal price)
        {
            IFish fish;
            if (fishType != nameof(FreshwaterFish) && fishType != nameof(SaltwaterFish))
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.InvalidFishType));
            }

            if (fishType == nameof(FreshwaterFish))
            {
                fish = new FreshwaterFish(fishName, fishSpecies, price);
            }
            else 
            {
                fish = new SaltwaterFish(fishName, fishSpecies, price);
            }
           
            IAquarium aquarium = aquariums.First(x => x.Name == aquariumName);
            if (aquarium.GetType().Name == nameof(FreshwaterAquarium) && fish.GetType().Name == nameof(FreshwaterFish))
            {
                aquarium.AddFish(fish);
            }
            else if (aquarium.GetType().Name == nameof(SaltwaterAquarium) && fish.GetType().Name == nameof(SaltwaterFish))
            {
                aquarium.AddFish(fish);
            }
            else
            {
                return "Water not suitable.";
            }

            return $"Successfully added {fishType} to {aquariumName}.";
        }

        public string CalculateValue(string aquariumName)
        {
            IAquarium aquarium = aquariums.First(x => x.Name == aquariumName);
            decimal priceSum = aquarium.Fish.Sum(x => x.Price);
            decimal decorationSum = aquarium.Decorations.Sum(x => x.Price);
            decimal sum = priceSum + decorationSum;
            return $"The value of Aquarium {aquariumName} is {sum:f2}.";
        }

        public string FeedFish(string aquariumName)
        {
            IAquarium aquarium = aquariums.First(x => x.Name == aquariumName);
            foreach (var fish in aquarium.Fish)
            {
                fish.Eat();
            }
            return $"Fish fed: {aquarium.Fish.Count()}";
        }

        public string InsertDecoration(string aquariumName, string decorationType)
        {
            IAquarium aquarium;
            IDecoration decoration;
            decoration = decorations.FirstOrDefault(x => x.GetType().Name == decorationType);
            if (decorationType != nameof(Ornament) && decorationType != nameof(Plant) || decoration == null)
            {
                throw new InvalidOperationException($"There isn't a decoration of type {decorationType}.");
            }

            aquarium = aquariums.FirstOrDefault(x => x.Name == aquariumName);
            aquarium.AddDecoration(decoration);
            decorations.Remove(decoration);
            return $"Successfully added {decorationType} to {aquariumName}.";
        }

        public string Report()
        {
           StringBuilder sb = new StringBuilder();
            foreach (var aquarium in aquariums)
            {
                sb.AppendLine(aquarium.GetInfo());
            }
            return sb.ToString().TrimEnd();
        }
    }
}
