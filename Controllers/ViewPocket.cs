using System;
using System.Linq;

namespace MushroomPocket.Controllers
{
    public static class ViewPocket
    {
        //List all duplicate characters in pocket
        public static void ListPocketCharacters(MushroomDBContext context)
        {
            var characters = context.Pockets.ToList();
            foreach (var character in characters)
            {
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine($"Name: {character.CharacterName}");
                Console.WriteLine($"Skill: {character.Skill}");
                Console.WriteLine($"Rarity: {character.Rarity}");
                Console.WriteLine("--------------------------------------------------------------------");
            }
        }
    }
}