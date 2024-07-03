using System;
using System.Linq;

namespace MushroomPocket.Controllers
{
    public static class CheatMode
    {
        //This method is a method that WILL NEVER BE IN A GAME, this is for testing purposes to ensure the simple boss fight can be cleared
        public static void MaxAllCharacters(MushroomDBContext context)
        {
            var characters = context.Inventories.Where(i => i.ItemType == "Character" || i.ItemType == "SpecialCharacter").ToList();
            
            foreach (var character in characters)
            {
                character.Level = 100;
                character.HP = 10000;
                character.Attack = 1040;
                character.Defense = 1040;
                character.CritRate = 100;
                character.CritDamage = 298;
                character.Exp = character.Level * 1000;
                character.AscensionStage = 10; // Max ascension stage for level 100
            }

            context.SaveChanges();
            Console.WriteLine("All characters have been maxed out.");
        }
    }
}