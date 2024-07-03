using System;
using System.Linq;

namespace MushroomPocket.Controllers
{
    public static class AscendCharacter
    {
        public static void AscendCharacterMethod(MushroomDBContext context)
        {
            //Check if there is any eligible character for ascension by checking the level and ascension stage, and console print the list of eligible characters.
            var characters = context.Inventories.Where(c => (c.ItemType == "Character" || c.ItemType == "SpecialCharacter") && c.Level % 10 == 0 && c.Level / 10 == c.AscensionStage && !(c.Level == 100)).ToList();
            foreach (var character in characters)
            {
                Console.WriteLine($"Name: {character.CharacterName}, Level: {character.Level}");
            }

            Console.Write("Enter the name of the character to ascend: ");
            var name = Console.ReadLine();
            //Check if the name input of the character means the ascension criteria, if so proceed with ascension.
            var characterToAscend = context.Inventories.FirstOrDefault(c => c.CharacterName == name && c.Level % 10 == 0 && c.Level / 10 == c.AscensionStage);

            if (characterToAscend != null)
            {
                //Increase the Ascension Stage of the Character by 1
                characterToAscend.AscensionStage++;
                context.SaveChanges();
                Console.WriteLine("Character ascended successfully.");
            }
            else
            {
                Console.WriteLine("Character not found or cannot be ascended.");
            }
        }
    }
}