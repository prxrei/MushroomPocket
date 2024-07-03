using System;
using System.Linq;
using MushroomPocket.Models;

namespace MushroomPocket.Controllers
{
    public static class UpgradeCharacter
    {
        //This method has references to ChatGPT for error correcting and exception handling due to logic errors while debugging
        public static void UpgradeCharacterMethod(MushroomDBContext context)
        {
            //Check for Exp Book
            var expBooks = context.Inventories.FirstOrDefault(i => i.ItemType == "ExpBook");
            if (expBooks == null || expBooks.ExpBookQuantity <= 0)
            {
                Console.WriteLine("No exp books available.");
                return;
            }
            
            //Print the list of characters in inventory
            var characters = context.Inventories.Where(i => i.ItemType == "Character" || i.ItemType == "SpecialCharacter").ToList();
            foreach (var character in characters)
            {
                Console.WriteLine($"Name: {character.CharacterName}, Level: {character.Level}, Exp: {character.Exp}");
            }
            //Input name of character to be upgraded, check if the character exists.
            Console.Write("Enter the name of the character to upgrade: ");
            var name = Console.ReadLine();
            var characterToUpgrade = context.Inventories.FirstOrDefault(c => c.CharacterName == name);
            if (characterToUpgrade == null)
            {
                Console.WriteLine("Character not found.");
                return;
            }
            //Set current level to the level of the character chosen to be upgraded and check if it is at max level already.
            int currentLevel = characterToUpgrade.Level ?? 0;
            int maxLevel = 100;

            if (currentLevel == maxLevel)
            {
                Console.WriteLine("Character is already at the maximum level.");
                return;
            }

            //Set the levelCap for the character to be 10 times the AscensionStage for game logic to lock leveling behind ascensions. This is not developed here but in the inspiration game, there are materials for ascension.
            int levelCap = (characterToUpgrade.AscensionStage ?? 0) * 10;
            if (levelCap > maxLevel) levelCap = maxLevel;

            //If the current level of the character is at the levelCap, print a message stating that the character cannot be upgraded any further before ascension.
            if (currentLevel == levelCap)
            {
                Console.WriteLine($"Character {characterToUpgrade.CharacterName} needs to ascend to level up further.");
                return;
            }

            //Calculate Exp needed to reach level cap
            int expForLevelCap = 0;
            //Calculate the maximum number of books required to hit levelCap.
            for (int level = currentLevel; level < levelCap && level < maxLevel; level++)
            {
                expForLevelCap += level * 1000;
            }
            int remainingExp = expForLevelCap - (characterToUpgrade.Exp ?? 0);
            int maxBooksToUse = (int)Math.Ceiling(remainingExp / 1000.0);
            //Line 59-65 is referenced from ChatGPT for a method to calculate the number of books required to hit the level cap, reducing wastage of exp books.

            while (true)
            {
                Console.Write($"Enter the number of exp books to use (You have {expBooks.ExpBookQuantity} exp books): ");
                if (!int.TryParse(Console.ReadLine(), out int numBooks) || numBooks <= 0)
                {
                    Console.WriteLine("Invalid number of exp books.");
                    continue;
                }

                if (numBooks > expBooks.ExpBookQuantity)
                {
                    Console.WriteLine($"You do not have enough exp books. You only have {expBooks.ExpBookQuantity} exp books.");
                }
                else if (numBooks > maxBooksToUse)
                {
                    numBooks = maxBooksToUse;
                    Console.WriteLine($"Only {maxBooksToUse} exp books will be used to reach the next ascension cap.");
                }
                //Upgrade the character and update the character's stats
                int totalExp = numBooks * 1000;
                characterToUpgrade.Exp += totalExp;
                UpdateCharacterStats(characterToUpgrade, levelCap);

                //Update the number of ExpBooks after upgrade
                expBooks.ExpBookQuantity -= numBooks;
                if (expBooks.ExpBookQuantity <= 0)
                {
                    context.Inventories.Remove(expBooks);
                }

                context.SaveChanges();
                Console.WriteLine("Character upgraded successfully.");
                break;
            }
        }

        //Method to update character's stats after upgrading(level up)
        private static void UpdateCharacterStats(Inventory character, int levelCap)
        {
            while (character.Exp >= character.Level * 1000 && character.Level < levelCap && character.Level < 100)
            {
                character.Exp -= character.Level * 1000;
                character.Level++;
                character.HP = character.Level * 100;
                character.Attack += 10;
                character.Defense += 10;
                character.CritRate += 1;
                character.CritDamage += 2;
            }
        }
    }
}