using System;
using System.Linq;
using MushroomPocket.Models;

namespace MushroomPocket.Controllers
{
    public static class AddFatesExp
    {
        //C# methods such as TryParse, FirstOrDefault are references from IT2163 Application Security and IT2166 Enterprise Development Project modules taken in 2023S2.
        public static void AddFatesAndExpBooks(MushroomDBContext context)
        {
            Console.Write("Enter number of fates to add: ");
            if (int.TryParse(Console.ReadLine(), out int fateCount) && fateCount > 0)
            {
                var fate = context.Inventories.FirstOrDefault(i => i.ItemType == "Fate");
                if (fate != null)
                {
                    fate.FateQuantity += fateCount;
                }
                else
                {
                    context.Inventories.Add(new Inventory
                    {
                        ItemType = "Fate",
                        FateQuantity = fateCount
                    });
                }
                context.SaveChanges();
                Console.WriteLine($"Added {fateCount} fates.");
            }
            else
            {
                Console.WriteLine("Invalid number of fates.");
            }

            Console.Write("Enter number of exp books to add: ");
            if (int.TryParse(Console.ReadLine(), out int expBookCount) && expBookCount > 0)
            {
                var expBook = context.Inventories.FirstOrDefault(i => i.ItemType == "ExpBook");
                if (expBook != null)
                {
                    expBook.ExpBookQuantity += expBookCount;
                }
                else
                {
                    context.Inventories.Add(new Inventory
                    {
                        ItemType = "ExpBook",
                        ExpBookQuantity = expBookCount,
                        ExpAmount = 1000 // Fixed amount of exp per book
                    });
                }
                context.SaveChanges();
                Console.WriteLine($"Added {expBookCount} exp books.");
            }
            else
            {
                Console.WriteLine("Invalid number of exp books.");
            }
        }
    }
}