using System;
using System.Linq;
using MushroomPocket.Models;

namespace MushroomPocket.Controllers
{
    public static class AddCharacter
    {
        public static void AddCharacterManually(MushroomDBContext context)
        {
            // Displays all normal characters in console
            Console.WriteLine("Available characters:");
            var availableCharacters = context.Characters.ToList();
            foreach (var character in availableCharacters)
            {
                Console.WriteLine($"Name: {character.Name}, Skill: {character.Skill}, Rarity: {character.Rarity}");
            }

            // Displays all special characters in console
            Console.WriteLine("Available special characters:");
            var availableSpecialCharacters = context.SpecialCharacters.ToList();
            foreach (var specialCharacter in availableSpecialCharacters)
            {
                Console.WriteLine($"Name: {specialCharacter.Name}, Skill: {specialCharacter.Skill}, Rarity: {specialCharacter.Rarity}");
            }

            Console.Write("Enter character name: ");
            var name = Console.ReadLine();

            // Try to find the character in normal characters
            var characterToAdd = context.Characters.FirstOrDefault(c => c.Name == name);
            if (characterToAdd == null)
            {
                // Try to find the character in special characters
                var specialCharacterToAdd = context.SpecialCharacters.FirstOrDefault(sc => sc.Name == name);
                if (specialCharacterToAdd == null)
                {
                    Console.WriteLine("Character not found.");
                    return;
                }
                // If found, invoke a method to add special character
                AddSpecialCharacterToInventory(context, specialCharacterToAdd);
            }
            else
            {
                // If found, invoke a method to add normal character
                AddCharacterToInventory(context, characterToAdd);
            }

            context.SaveChanges();
        }

        private static void AddCharacterToInventory(MushroomDBContext context, Character characterToAdd)
        {
            var existingCharacter = context.Inventories.FirstOrDefault(i => i.CharacterName == characterToAdd.Name);
            if (existingCharacter != null)
            {
                var newPocketCharacter = new Pocket
                {
                    CharacterName = characterToAdd.Name,
                    Skill = characterToAdd.Skill,
                    Rarity = characterToAdd.Rarity
                };
                context.Pockets.Add(newPocketCharacter);
                Console.WriteLine("Character is in your collection, duplicate added to pocket.");
            }
            else
            {
                var (hp, exp) = GetCharacterStats();
                var newCharacter = new Inventory
                {
                    ItemType = "Character",
                    CharacterName = characterToAdd.Name,
                    HP = hp,
                    Exp = exp,
                    Level = 1,
                    Skill = characterToAdd.Skill,
                    Rarity = characterToAdd.Rarity,
                    Attack = 50,
                    Defense = 50,
                    CritRate = 1,
                    CritDamage = 100,
                    AscensionStage = 1
                };
                context.Inventories.Add(newCharacter);
                Console.WriteLine($"{newCharacter.CharacterName} has been added to Inventory");
            }
        }

        private static void AddSpecialCharacterToInventory(MushroomDBContext context, SpecialCharacter specialCharacterToAdd)
        {
            var existingCharacter = context.Inventories.FirstOrDefault(i => i.CharacterName == specialCharacterToAdd.Name);
            if (existingCharacter != null)
            {
                var newPocketCharacter = new Pocket
                {
                    CharacterName = specialCharacterToAdd.Name,
                    Skill = specialCharacterToAdd.Skill,
                    Rarity = specialCharacterToAdd.Rarity
                };
                context.Pockets.Add(newPocketCharacter);
                Console.WriteLine("Special character is in your collection, duplicate added to pocket.");
            }
            else
            {
                var (hp, exp) = GetCharacterStats();
                var newCharacter = new Inventory
                {
                    ItemType = "SpecialCharacter",
                    CharacterName = specialCharacterToAdd.Name,
                    HP = hp,
                    Exp = exp,
                    Level = 1,
                    Skill = specialCharacterToAdd.Skill,
                    Rarity = specialCharacterToAdd.Rarity,
                    Attack = 50,
                    Defense = 50,
                    CritRate = 1,
                    CritDamage = 100,
                    AscensionStage = 1
                };
                context.Inventories.Add(newCharacter);
                Console.WriteLine("Special character added to inventory.");
            }
        }

        private static (int hp, int exp) GetCharacterStats()
        {
            int hp = GetValidInput("Enter HP for the character: ", 100);
            int exp = GetValidInput("Enter EXP for the character: ", 0);
            return (hp, exp);
        }

        private static int GetValidInput(string prompt, int defaultValue)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            if (int.TryParse(input, out int value) && value > 0)
            {
                return value;
            }
            else
            {
                Console.WriteLine($"Invalid input. Setting to default value: {defaultValue}");
                return defaultValue;
            }
        }
    }
}
