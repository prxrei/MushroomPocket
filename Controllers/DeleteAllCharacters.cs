using System;
using System.Linq;

namespace MushroomPocket.Controllers
{
    public static class DeleteAllCharacters
    {
        public static void DeleteAll(MushroomDBContext context)
        {
            // Retrieve all characters and special characters from the inventory and pocket
            var characters = context.Inventories.Where(i => i.ItemType == "Character" || i.ItemType == "SpecialCharacter").ToList();
            var pocCharacters = context.Pockets.ToList();

            // Remove all characters from the inventory and pocket
            if (characters.Any())
            {
                context.Inventories.RemoveRange(characters);
                context.Pockets.RemoveRange(pocCharacters);
                context.SaveChanges();
                Console.WriteLine("All characters and special characters have been deleted from the inventory.");
            }
            else
            {
                Console.WriteLine("No characters or special characters found in the inventory.");
            }
        }
    }
}