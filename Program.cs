using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MushroomPocket.Models;
using MushroomPocket.Controllers;

namespace MushroomPocket
{
    class Program
    {
        static void Main(string[] args)
        {
            // MushroomMaster criteria list for checking character transformation availability.
            List<MushroomMaster> mushroomMasters = new List<MushroomMaster>(){
                new MushroomMaster("Daisy", 2, "Peach"),
                new MushroomMaster("Wario", 3, "Mario"),
                new MushroomMaster("Waluigi", 1, "Luigi")
            };

            // Configures and create options for DbContext for EntityFramework.
            var options = new DbContextOptionsBuilder<MushroomDBContext>()
                .UseSqlite("Data Source=Database/MushroomPocket.db")
                .Options;
            // Lines 21-26 are referenced from ChatGPT as a method to utilize DbContext and EntityFramework.
            using var context = new MushroomDBContext(options);

            // Initialize fates in the inventory if not already present, giving players free Fates to play Gacha.
            var fate = context.Inventories.FirstOrDefault(i => i.ItemType == "Fate");
            if (fate == null)
            {
                context.Inventories.Add(new Inventory
                {
                    ItemType = "Fate",
                    FateQuantity = 100
                });
                context.SaveChanges();
            }

            // Declare and initialize a new instance of MushroomGacha class and allow it to perform database operations.
            MushroomGacha gachaSystem = new MushroomGacha(context);

            bool continueProgram = true;

            while (continueProgram)
            {
                DisplayMenu();
                Console.Write("Please only enter [1,2,3,4,5,6,7,8,9,10,11,12,13] or Q to quit: ");
                var choice = Console.ReadLine().ToLower();

                switch (choice)
                {
                    case "1":
                        AddCharacter.AddCharacterManually(context);
                        break;
                    case "2":
                        PerformGachaPull(gachaSystem);
                        break;
                    case "3":
                        ViewCharacters.ListInventoryCharacters(context);
                        break;
                    case "4":
                        ViewPocket.ListPocketCharacters(context);
                        break;
                    case "5":
                        CheckAscension.CheckAscensions(context);
                        break;
                    case "6":
                        AscendCharacter.AscendCharacterMethod(context);
                        break;
                    case "7":
                        TransformCharacter.TransformCharacterMethod(context, mushroomMasters);
                        break;
                    case "8":
                        UpgradeCharacter.UpgradeCharacterMethod(context);
                        break;
                    case "9":
                        CheckInventory.CheckInventoryMethod(context);
                        break;
                    case "10":
                        AddFatesExp.AddFatesAndExpBooks(context);
                        break;
                    case "11":
                        SimpleBossFight.StartFight(context);
                        break;
                    case "12":
                        CheatMode.MaxAllCharacters(context);
                        break;
                    case "13":
                        DeleteAllCharacters.DeleteAll(context);
                        break;
                    case "q":
                        continueProgram = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                if (continueProgram)
                {
                    Console.WriteLine("Do you want to continue? (Y/y to continue, any other key to exit): ");
                    var continueChoice = Console.ReadLine();
                    if (continueChoice.ToLower() != "y")
                    {
                        continueProgram = false;
                    }
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("WELCOME TO MUSHROOM POCKET IMPACT GAMING");
            Console.WriteLine("1. Manually Add Character");
            Console.WriteLine("2. Gacha Wishing System - Roll a lottery-like system to get a character");
            Console.WriteLine("3. View Characters (in inventory)");
            Console.WriteLine("4. View Pocket Characters - Duplicates/Copies ONLY");
            Console.WriteLine("5. Check if characters can ascend - Check if any Character Level is in multiples of 10");
            Console.WriteLine("6. Ascend Character - Ascension Required to overcome ->10 level cap.");
            Console.WriteLine("7. Transform Character - MushroomMaster class method");
            Console.WriteLine("8. Upgrade Character - Use Exp Books to level up characters.");
            Console.WriteLine("9. Check Inventory - Check Inventory for Exp Books and remaining Fates (used for Gacha Wishing System)");
            Console.WriteLine("10. Add Fates and Exp Books - Add materials for nice Gacha and Upgrading EXperience!");
            Console.WriteLine("11. Start Boss Fight - Minigame for fighting bosses, this is currently just Autobattling!");
            Console.WriteLine("12. Max All Characters - Cheat Mode that maxes out your characters and make them OP!");
            Console.WriteLine("13. Delete All Characters - Clear the Inventory and Pocket tables!");
            Console.WriteLine("Q. Exit");
        }

        // A method specially made to handle two types of Gacha pulling.
        static void PerformGachaPull(MushroomGacha gachaSystem)
        {
            Console.WriteLine("Choose Gacha Pull type:");
            Console.WriteLine("1. Single Pull");
            Console.WriteLine("2. Multi Pull (10 pulls)");
            var gachaChoice = Console.ReadLine();

            switch (gachaChoice)
            {
                case "1":
                    gachaSystem.GachaPull();
                    break;
                case "2":
                    gachaSystem.MultiPull();
                    break;
                default:
                    Console.WriteLine("Invalid option. Performing Single Pull.");
                    gachaSystem.GachaPull();
                    break;
            }
        }
    }
}