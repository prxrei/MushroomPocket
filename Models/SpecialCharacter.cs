using System.ComponentModel.DataAnnotations;

namespace MushroomPocket.Models
{
    public class SpecialCharacter
    {
        //This assignment's advanced features are inspired by Genshin Impact.
        //This SpecialCharacter.cs has no code references, only game design references.
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Skill { get; set; }
        public int Rarity { get; set; }
    }
}
