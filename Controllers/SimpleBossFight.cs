using System;
using System.Linq;
using System.Collections.Generic;
using MushroomPocket.Models;

namespace MushroomPocket.Controllers
{
    public static class SimpleBossFight
    {
        //This method has references to ChatGPT for error correcting and exception handling due to logic errors while debugging
        public static void StartFight(MushroomDBContext context)
        {
            var characters = context.Inventories.Where(i => i.ItemType == "Character" || i.ItemType == "SpecialCharacter").ToList();
            if (characters.Count == 0)
            {
                Console.WriteLine("No characters available for battle.");
                return;
            }

            //Prints a list of available characters in the inventory
            Console.WriteLine("Select up to 4 characters for the battle (enter character names separated by commas):");
            foreach (var character in characters)
            {
                Console.WriteLine($"Name: {character.CharacterName}, HP: {character.HP}, Attack: {character.Attack}, CritRate: {character.CritRate}, CritDamage: {character.CritDamage}");
            }

            //Reads the names of the input characters and add them to a list if they are correct.
            var selectedNames = Console.ReadLine().Split(',').Select(name => name.Trim()).ToList();
            var selectedCharacters = characters.Where(c => selectedNames.Contains(c.CharacterName)).Take(4).ToList();

            //Exception break if there are no available characters
            if (selectedCharacters.Count == 0)
            {
                Console.WriteLine("No valid characters selected.");
                return;
            }

            Console.WriteLine("BATTLE STARTING...");

            //Creates a new Boss object
            var boss = new AbyssBoss
            {
                Name = "Abyssal Overlord",
                AbyssBossHP = 10000,
                AbyssBossAttack = 500,
                AbyssBossDefense = 200,
                AbyssBossCritRate = 20,
                AbyssBossCritDamage = 150
            };

            //Starts a battle with the boss using the chosen characters
            BattleLogic(selectedCharacters, boss);
        }

        private static void BattleLogic(List<Inventory> characters, AbyssBoss boss)
        {
            //Creates a new random class instance to calculate probabilities for the battle
            Random random = new Random();

            //Auto Battling till either Boss or Party is wiped.
            while (boss.AbyssBossHP > 0 && characters.Any(c => c.HP > 0))
            {
                foreach (var character in characters.Where(c => c.HP > 0))
                {
                    Console.WriteLine($"{character.CharacterName} Attacks!");
                    int damage = character.Attack ?? 0;

                    // Check for critical hit, generates a random number and if it is smaller than the crit rate, will create a critical hit
                    if (random.Next(0, 100) < (character.CritRate ?? 0))
                    {
                        damage = (int)(damage * ((character.CritDamage ?? 0) * 0.01));
                        Console.WriteLine("Critical Hit!");
                    }

                    //Character deals damage to the boss based on Attack, Crit Rate and Crit Damage
                    boss.AbyssBossHP -= damage;
                    if (boss.AbyssBossHP < 0)
                    {
                        boss.AbyssBossHP = 0;
                    }
                    Console.WriteLine($"{character.CharacterName} deals {damage} damage to the Boss. Boss HP: {boss.AbyssBossHP}");

                    //Boss defeated prompt if boss is taken down.
                    if (boss.AbyssBossHP == 0)
                    {
                        Console.WriteLine("Boss Defeated!");
                        return;
                    }
                }

                Console.WriteLine("Boss Attacks!");

                //Boss attacks random character
                var target = characters.Where(c => c.HP > 0).OrderBy(c => Guid.NewGuid()).FirstOrDefault();
                if (target != null)
                {
                    int damage = boss.AbyssBossAttack;

                    // Check for critical hit, generates a random number and if it is smaller than the crit rate, will create a critical hit
                    if (random.Next(0, 100) < boss.AbyssBossCritRate)
                    {
                        damage = (int)(damage * boss.AbyssBossCritDamage * 0.01);
                        Console.WriteLine("Boss Critical Hit!");
                    }

                    //Boss deals damage to the character based on Attack, Crit Rate and Crit Damage
                    target.HP -= damage;
                    if (target.HP < 0)
                    {
                        target.HP = 0;
                    }
                    Console.WriteLine($"Boss deals {damage} damage to {target.CharacterName}. {target.CharacterName} HP: {target.HP}");

                    if (target.HP == 0)
                    {
                        Console.WriteLine($"{target.CharacterName} is Down!");
                    }
                }
            }

            //Game over prompt if party is wiped out
            if (characters.All(c => c.HP == 0))
            {
                Console.WriteLine("All characters Down. Game over!");
            }
        }
    }
}