#nullable enable

using System.ComponentModel.DataAnnotations;

namespace MushroomPocket.Models
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }
        public string? ItemType { get; set; }
        // Character's attributes
        //This assignment's advanced features are inspired by Genshin Impact.
        //This Inventory.cs has no code references, only game design references.
        public string? CharacterName { get; set; }
        public int? Exp { get; set; }
        public int? Level { get; set; }
        public int? HP { get; set; }
        public string? Skill { get; set; }
        public int? Rarity { get; set; }
        public int? Attack { get; set; }
        public int? Defense { get; set; }
        public int? CritRate { get; set; }
        public int? CritDamage { get; set; }
        public int? AscensionStage { get; set; }

        // Non-Character's properties
        public int? ExpAmount { get; set; }
        public int? ExpBookQuantity { get; set; }
        public int? FateQuantity { get; set; }
    }
}
