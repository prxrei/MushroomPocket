using System;
using System.Linq;

namespace MushroomPocket.Controllers
{
    public static class ViewCharacters
    {
        // List all characters in inventory
        public static void ListInventoryCharacters(MushroomDBContext context)
        {
            var characters = context.Inventories
                .Where(i => i.ItemType == "Character" || i.ItemType == "SpecialCharacter")
                .OrderByDescending(i => i.HP)
                .ToList();

            foreach (var character in characters)
            {
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine($"Name: {character.CharacterName}");
                Console.WriteLine($"HP: {character.HP}");
                Console.WriteLine($"Exp: {character.Exp}");
                Console.WriteLine($"Level: {character.Level}");
                Console.WriteLine($"Skill: {character.Skill}");
                Console.WriteLine($"Rarity: {character.Rarity}");
                Console.WriteLine($"Attack: {character.Attack}");
                Console.WriteLine($"Defense: {character.Defense}");
                Console.WriteLine($"CritRate: {character.CritRate}");
                Console.WriteLine($"CritDamage: {character.CritDamage}");
                Console.WriteLine($"Ascension Stage: {character.AscensionStage}");
                Console.WriteLine("--------------------------------------------------------------------");
            }
        }
    }
}
