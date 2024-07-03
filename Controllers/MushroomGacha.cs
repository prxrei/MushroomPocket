using System;
using System.Collections.Generic;
using System.Linq;
using MushroomPocket.Models;

namespace MushroomPocket.Controllers
{
    //This method has multiple references to ChatGPT for error correcting and exception handling due to logic errors while debugging
    public class MushroomGacha
    {
        private MushroomDBContext _context;
        private Random _random = new Random();
        //Line 12 is referenced from ChatGPT to generate an instance of Random class to be used for probability calculations

        public MushroomGacha(MushroomDBContext context)
        {
            _context = context;
        }

        public void GachaPull()
        {
            var fate = _context.Inventories.FirstOrDefault(i => i.ItemType == "Fate");
            if (fate == null || fate.FateQuantity <= 0)
            {
                Console.WriteLine("You do not have enough fates.");
                return;
            }

            fate.FateQuantity--;

            //Generates a random double to be used to check against a probability of obtaining a special character, normal character or exp books
            double pullProbability = _random.NextDouble();
            //Line 31 is referenced from ChatGPT to generate a random double
            if (pullProbability <= 0.05)
            {
                //Used to randomly select a Special Character from the SpecialCharacters collection in the DbContext
                var specialCharacter = _context.SpecialCharacters.AsEnumerable().OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                //Line 37 is referenced from ChatGPT for a method to randomly choose a special character from the available special characters in the database
                if (specialCharacter != null)
                {
                    AddSpecialCharacterToInventoryOrPocket(specialCharacter);
                    Console.WriteLine($"You obtained a special character: {specialCharacter.Name}");
                }
            }
            else if (pullProbability <= 0.25)
            {
                //Used to randomly select a normal Character from the SpecialCharacters collection in the DbContext
                var character = _context.Characters.AsEnumerable().OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                //Line 48 is referenced from ChatGPT for a method to randomly choose a normal character from the available special characters in the database
                if (character != null)
                {
                    AddCharacterToInventoryOrPocket(character);
                    Console.WriteLine($"You obtained a character: {character.Name}");
                }
            }
            else
            {
                AddExpBook(1);
                Console.WriteLine("You obtained an exp book.");
            }

            _context.SaveChanges();
        }

        public void MultiPull()
        {
            var fate = _context.Inventories.FirstOrDefault(i => i.ItemType == "Fate");
            if (fate == null || fate.FateQuantity < 10)
            {
                Console.WriteLine("You do not have enough fates for a multi-pull.");
                return;
            }

            fate.FateQuantity -= 10;

            //Creates a Hashset to store characters that have been obtained in the multi-pull and prevent duplicates in the same multi-pull from being added to inventory together.
            var obtainedCharacters = new HashSet<string>();
            //Line 72 is referenced from ChatGPT for a method to prevent duplicates from being added to inventory during the multi-pull feature.

            for (int i = 0; i < 10; i++)
            {
                double pullProbability = _random.NextDouble();
                if (pullProbability <= 0.05)
                {
                    //Used to randomly select a Special Character from the SpecialCharacters collection in the DbContext
                    var specialCharacter = _context.SpecialCharacters.AsEnumerable().OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                    //Line 86 is referenced from ChatGPT for a method to randomly choose a special character from the available special characters in the database
                    if (specialCharacter != null)
                    {
                        if (!obtainedCharacters.Contains(specialCharacter.Name))
                        {
                            AddSpecialCharacterToInventoryOrPocket(specialCharacter);
                            obtainedCharacters.Add(specialCharacter.Name);
                            Console.WriteLine($"You obtained a special character: {specialCharacter.Name}");
                        }
                        else
                        {
                            AddSpecialCharacterToPocket(specialCharacter);
                            Console.WriteLine("Special character is in your collection, duplicate added to pocket.");
                        }
                    }
                }
                else if (pullProbability <= 0.25)
                {
                    //Used to randomly select a normal Character from the SpecialCharacters collection in the DbContext
                    var character = _context.Characters.AsEnumerable().OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                    //Line 106 is referenced from ChatGPT for a method to randomly choose a normal character from the available special characters in the database
                    if (character != null)
                    {
                        if (!obtainedCharacters.Contains(character.Name))
                        {
                            AddCharacterToInventoryOrPocket(character);
                            obtainedCharacters.Add(character.Name);
                            Console.WriteLine($"You obtained a character: {character.Name}");
                        }
                        else
                        {
                            AddCharacterToPocket(character);
                            Console.WriteLine("Character is in your collection, duplicate added to pocket. Inventory remains unchanged.");
                        }
                    }
                }
                else
                {
                    AddExpBook(1);
                    Console.WriteLine("You obtained an exp book.");
                }
            }

            _context.SaveChanges();
        }

        //Checking if the normal character is already in inventory, and if it is, add to pocket instead. Otherwise add to inventory.
        private void AddCharacterToInventoryOrPocket(Character character)
        {
            var existingCharacter = _context.Inventories.FirstOrDefault(i => i.CharacterName == character.Name && i.ItemType == "Character");
            if (existingCharacter != null)
            {
                AddCharacterToPocket(character);
                Console.WriteLine("Character is in your collection, duplicate added to pocket.");
            }
            else
            {
                AddCharacterToInventory(character);
            }
        }

        //Checking if the special character is already in inventory, and if it is, add to pocket instead. Otherwise add to inventory.
        private void AddSpecialCharacterToInventoryOrPocket(SpecialCharacter specialCharacter)
        {
            var existingCharacter = _context.Inventories.FirstOrDefault(i => i.CharacterName == specialCharacter.Name && i.ItemType == "SpecialCharacter");
            if (existingCharacter != null)
            {
                AddSpecialCharacterToPocket(specialCharacter);
                Console.WriteLine("Special character is in your collection, duplicate added to pocket.");
            }
            else
            {
                AddSpecialCharacterToInventory(specialCharacter);
            }
        }

        //Extra Method created to add Normal Character to inventory, implementation is different due to issues with multi-pull
        private void AddCharacterToInventory(Character character)
        {
            var newCharacter = new Inventory
            {
                ItemType = "Character",
                CharacterName = character.Name,
                HP = 100,
                Exp = 0,
                Level = 1,
                Skill = character.Skill,
                Rarity = character.Rarity,
                Attack = 50,
                Defense = 50,
                CritRate = 1,
                CritDamage = 100,
                AscensionStage = 1
            };
            _context.Inventories.Add(newCharacter);
        }

        //Extra Method created to add Special Character to inventory, implementation is different due to issues with multi-pull
        private void AddSpecialCharacterToInventory(SpecialCharacter specialCharacter)
        {
            var newCharacter = new Inventory
            {
                ItemType = "SpecialCharacter",
                CharacterName = specialCharacter.Name,
                HP = 100,
                Exp = 0,
                Level = 1,
                Skill = specialCharacter.Skill,
                Rarity = specialCharacter.Rarity,
                Attack = 50,
                Defense = 50,
                CritRate = 1,
                CritDamage = 100,
                AscensionStage = 1
            };
            _context.Inventories.Add(newCharacter);
        }

        //Extra Method created to add Normal Character to pocket, implementation is different due to issues with multi-pull
        private void AddCharacterToPocket(Character character)
        {
            var newPocketCharacter = new Pocket
            {
                CharacterName = character.Name,
                Skill = character.Skill,
                Rarity = character.Rarity
            };

            _context.Pockets.Add(newPocketCharacter);
        }

        //Extra Method created to add Special Character to inventory, implementation is different due to issues with multi-pull
        private void AddSpecialCharacterToPocket(SpecialCharacter specialCharacter)
        {
            var newPocketCharacter = new Pocket
            {
                CharacterName = specialCharacter.Name,
                Skill = specialCharacter.Skill,
                Rarity = specialCharacter.Rarity
            };

            _context.Pockets.Add(newPocketCharacter);
        }

        //Method to Add Exp Book to inventory, just increases the quantity if the ExpBook object already exists.
        private void AddExpBook(int amount)
        {
            var expBook = _context.Inventories.FirstOrDefault(i => i.ItemType == "ExpBook");
            if (expBook != null)
            {
                expBook.ExpBookQuantity += amount;
            }
            else
            {
                _context.Inventories.Add(new Inventory
                {
                    ItemType = "ExpBook",
                    ExpBookQuantity = amount,
                    ExpAmount = 1000 // Fixed amount of exp per book
                });
            }
        }
    }
}