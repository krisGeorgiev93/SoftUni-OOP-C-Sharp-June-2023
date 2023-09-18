using System;
using System.Collections.Generic;
using System.Text;
using WarCroft.Constants;
using WarCroft.Entities.Characters.Contracts;
using WarCroft.Entities.Inventory;

namespace WarCroft.Entities.Characters
{
    public class Warrior : Character, IAttacker
    {
        public Warrior(string name)
            : base(name, 100, 50, 40, new Satchel())
        {
        }

        public void Attack(Character character)
        {
            if (!character.IsAlive || !this.IsAlive) //For a character to attack another character, both of them need to be alive.
            {
                throw new InvalidOperationException(ExceptionMessages.AffectedCharacterDead);
            }

            if (Name == character.Name) //If the character they are trying to attack is the same character
            {
                throw new InvalidOperationException(ExceptionMessages.CharacterAttacksSelf);
            }

            character.TakeDamage(AbilityPoints);
        }
    }
}