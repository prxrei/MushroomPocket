using System;
using System.Linq;

namespace MushroomPocket.Controllers
{
    public static class CheckInventory
    {
        public static void CheckInventoryMethod(MushroomDBContext context)
        {
            // Add the characters and special characters in inventory to a list and console print.
            var characters = context.Inventories.Where(i => i.ItemType == "Character" || i.ItemType == "SpecialCharacter").ToList();
            foreach (var character in characters)
            {
                Console.WriteLine($"Name: {character.CharacterName}, HP: {character.HP}, Exp: {character.Exp}, Level: {character.Level}, Skill: {character.Skill}, Rarity: {character.Rarity}, Attack: {character.Attack}, Defense: {character.Defense}, CritRate: {character.CritRate}, CritDamage: {character.CritDamage}, Ascension Stage: {character.AscensionStage}");
            }

            // Add the non-characters in inventory to a list and console print them under a quantity.
            var expBooks = context.Inventories.FirstOrDefault(i => i.ItemType == "ExpBook");
            var expBooksCount = expBooks?.ExpBookQuantity ?? 0;
            var fate = context.Inventories.FirstOrDefault(i => i.ItemType == "Fate");
            var fatesCount = fate?.FateQuantity ?? 0;

            Console.WriteLine($"Number of exp books: {expBooksCount}");
            Console.WriteLine($"Number of fates: {fatesCount}");
        }
    }
}