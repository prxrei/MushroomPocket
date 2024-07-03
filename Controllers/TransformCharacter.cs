using System;
using System.Linq;
using System.Collections.Generic;
using MushroomPocket.Models;

namespace MushroomPocket.Controllers
{
    public static class TransformCharacter
    {
        public static void TransformCharacterMethod(MushroomDBContext context, List<MushroomMaster> mushroomMasters)
        {
            // Prints the list of normal characters in inventory along with their potential transformation
            var characters = context.Inventories.Where(c => c.ItemType == "Character").ToList();
            foreach (var character in characters)
            {
                var tMaster = mushroomMasters.FirstOrDefault(mm => mm.Name == character.CharacterName);
                if (tMaster != null)
                {
                    Console.WriteLine($"{character.CharacterName} --> {tMaster.TransformTo}");
                }
            }

            // Asks player to choose the character to transform
            Console.Write("Enter the name of the character to transform: ");
            var name = Console.ReadLine();
            var characterToTransform = context.Inventories.FirstOrDefault(c => c.CharacterName == name);
            if (characterToTransform == null)
            {
                Console.WriteLine("Character not found.");
                return;
            }

            // Queries the MushroomMaster for valid transformation
            var master = mushroomMasters.FirstOrDefault(mm => mm.Name == name);
            if (master == null)
            {
                Console.WriteLine("No transformation available for this character.");
                return;
            }

            // Check the pocket for the duplicates to fulfill the NoToTransform criteria, and if it satisfies the criteria then the transformation shall proceed
            var pocketDuplicates = context.Pockets.Where(c => c.CharacterName == name).ToList();
            if (pocketDuplicates.Count >= master.NoToTransform - 1)
            {
                // Get the new character's skills from the Characters table
                var newCharacterDetails = context.Characters.FirstOrDefault(c => c.Name == master.TransformTo);
                if (newCharacterDetails == null)
                {
                    Console.WriteLine("Transformation details not found for the new character.");
                    return;
                }

                var transformedCharacter = new Inventory
                {
                    ItemType = "Character",
                    CharacterName = master.TransformTo,
                    HP = 100,
                    Exp = 0,
                    Level = 1,
                    Skill = newCharacterDetails.Skill, // Assign new character's skills
                    Rarity = characterToTransform.Rarity,
                    Attack = 50,
                    Defense = 50,
                    CritRate = 1,
                    CritDamage = 100,
                    AscensionStage = 1
                };

                context.Inventories.Remove(characterToTransform);
                context.Inventories.Add(transformedCharacter);

                context.Pockets.RemoveRange(pocketDuplicates.Take(master.NoToTransform - 1));
                context.SaveChanges();
                Console.WriteLine($"{characterToTransform.CharacterName} has been transformed to {master.TransformTo}.");
            }
            else
            {
                Console.WriteLine("Not enough duplicates to transform this character.");
            }
        }
    }
}
