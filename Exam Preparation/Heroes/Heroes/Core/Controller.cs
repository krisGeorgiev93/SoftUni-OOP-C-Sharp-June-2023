using Heroes.Core.Contracts;
using Heroes.Models;
using Heroes.Models.Contracts;
using Heroes.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heroes.Core
{
    public class Controller : IController
    {
        private HeroRepository heroes;
        private WeaponRepository weapons;

        public Controller()
        {
            this.heroes = new HeroRepository();
            this.weapons = new WeaponRepository();
        }

        public string AddWeaponToHero(string weaponName, string heroName)
        {
            if (!this.heroes.Models.Any(h => h.Name == heroName))
            {
                throw new InvalidOperationException($"Hero {heroName} does not exist.");
            }
            if (!this.weapons.Models.Any(w => w.Name == weaponName))
            {
                throw new InvalidOperationException($"Weapon {weaponName} does not exist.");
            }
            if (this.heroes.FindByName(heroName).Weapon != null)
            {
                throw new InvalidOperationException($"Hero {heroName} is well-armed.");
            }

            IWeapon weapon = weapons.FindByName(weaponName);
            this.heroes.FindByName(heroName).AddWeapon(weapon);

            return $"Hero {heroName} can participate in battle using a {weapon.GetType().Name.ToLower()}.";
        }

        public string CreateHero(string type, string name, int health, int armour)
        {
            if (this.heroes.Models.Any(h => h.Name == name))
            {
                throw new InvalidOperationException($"The hero {name} already exists.");
            }
            if (type != "Knight" && type != "Barbarian")
            {
                throw new InvalidOperationException("Invalid hero type.");
            }

            IHero hero;
            string message = "";
            if (type == "Knight")
            {
                hero = new Knight(name, health, armour);
                message = $"Successfully added Sir {name} to the collection.";
            }
            else
            {
                hero = new Barbarian(name, health, armour);
                message = $"Successfully added Barbarian {name} to the collection.";
            }

            heroes.Add(hero);
            return message;
        }

        public string CreateWeapon(string type, string name, int durability)
        {
            if (this.weapons.Models.Any(w => w.Name == name))
            {
                throw new InvalidOperationException($"The weapon {name} already exists.");
            }
            if (type != "Claymore" && type != "Mace" && type != "Weapon")
            {
                throw new InvalidOperationException("Invalid weapon type.");
            }

            IWeapon weapon;
            string message = "";
            if (type == "Claymore")
            {
                weapon = new Claymore(name, durability);
                message = $"A claymore {name} is added to the collection.";
            }
            else
            {
                weapon = new Mace(name, durability);
                message = $"A mace {name} is added to the collection.";
            }

            weapons.Add(weapon);
            return message;
        }

        public string HeroReport()
        {
            //Returns information about each hero separated with a new line.
            //Order them by hero type alphabetically,
            //then by health descending,
            //then by hero name alphabetically:

            //--Health: { hero health }
            //--Armour: { hero armour }
            //--Weapon: { weapon name }/ Unarmed


            StringBuilder sb = new StringBuilder();

            foreach (var hero in heroes.Models.OrderBy(h => h.GetType().Name)
                .ThenByDescending(h => h.Health)
                .ThenBy(h => h.Name))
            {
                sb.AppendLine($"{hero.GetType().Name}: {hero.Name}");
                sb.AppendLine($"--Health: {hero.Health}");
                sb.AppendLine($"--Armour: {hero.Armour}");
                if (hero.Weapon == null)
                {
                    sb.AppendLine("--Weapon: Unarmed");
                }
                else
                {
                    sb.AppendLine($"--Weapon: {hero.Weapon.Name}");
                }
            }

            return sb.ToString();
        }

        public string StartBattle()
        {
            var map = new Map();
            return map.Fight((ICollection<IHero>)heroes.Models);
        }
    }
}