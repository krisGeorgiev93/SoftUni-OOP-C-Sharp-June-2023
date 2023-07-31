using Heroes.Models.Contracts;
using Heroes.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heroes.Models
{
    public abstract class Hero : IHero
    {
        private string name;
        private int health;
        private int armour;
        private IWeapon weapon;

        public Hero(string name, int health, int armour)
        {
            this.Name = name;
            this.Health = health;
            this.Armour = armour;
        }

        public string Name
        {
            get { return name; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(string.Format(ExceptionMessages.HeroNameNull));
                }
                this.name = value;
            }
        }
        public int Health
        {
            get { return health;}
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException(string.Format(ExceptionMessages.HeroHealthBelowZero));
                }
                this.health = value;
            }
        }

        public int Armour
        {
            get { return armour; }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException(string.Format(ExceptionMessages.HeroArmourBelowZero));
                }
                this.armour = value;
            }
        }

        public IWeapon Weapon
        {
            get { return this.weapon; }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentException(string.Format(ExceptionMessages.WeaponNull));
                }
                this.weapon = value;
            }
        }

        public bool IsAlive => this.Health > 0;


        public void AddWeapon(IWeapon weapon) // This method adds a weapon to the given hero.
                                              // A hero can have only one weapon.
        {
            if (this.Weapon == null)
            {
                this.Weapon = weapon;
            }
        }

        public void TakeDamage(int points)
        {
            int transferPoints = points - this.Armour;

            if (Armour - points <= 0)
            {
                Armour = 0;

                if (this.Health - transferPoints <= 0)
                {
                    this.Health = 0;
                }
                else
                {
                    this.Health -= transferPoints;
                }
            }
            else
            {
                this.Armour -= points;
            }
        }
    }
}
