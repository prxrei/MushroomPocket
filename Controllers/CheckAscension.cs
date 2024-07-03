using System;
using System.Linq;

namespace MushroomPocket.Controllers
{
    public static class CheckAscension
    {
        public static void CheckAscensions(MushroomDBContext context)
        {
            // Check if there is any eligible character for ascension by checking the level and ascension stage, and console print the list of eligible characters.
            var characters = context.Inventories.Where(c => (c.ItemType == "Character" || c.ItemType == "SpecialCharacter") && c.Level % 10 == 0 && c.Level / 10 == c.AscensionStage).ToList();
            foreach (var character in characters)
            {
                //Informs that character is at max level if character level = 100
                if (character.Level == 100)
                {
                    Console.WriteLine($"{character.CharacterName} is at Max Level");
                }
                else
                {
                    Console.WriteLine($"{character.CharacterName} can ascend at level {character.Level}");
                }
            }
        }
    }
}