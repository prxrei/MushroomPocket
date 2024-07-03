#nullable enable

using System.ComponentModel.DataAnnotations;

namespace MushroomPocket.Models
{
    public class Pocket
    {
        [Key]
        public int Id { get; set; }
        // Character's attributes
        //This assignment's advanced features are inspired by Genshin Impact.
        //This Pocket.cs has no code references, only game design references.
        public string? CharacterName { get; set; }
        public string? Skill { get; set; }
        public int? Rarity { get; set; }
    }
}
