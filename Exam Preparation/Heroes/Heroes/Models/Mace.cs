using System;
using System.Collections.Generic;
using System.Text;

namespace Heroes.Models
{
    public class Mace : Weapon
    {
        public Mace(string name, int durability) : base(name, durability)
        {
        }
        //The DoDamage() method returns the damage of each weapon:
    //• Mace - 25 damage
    //• Claymore - 20 damage
        public override int DoDamage()
        {
            if (this.Durability - 1 == 0)
            {
                return 0;
            }
            this.Durability--;
            return 25;
        }
    }
}
