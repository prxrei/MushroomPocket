using System.ComponentModel.DataAnnotations;

namespace MushroomPocket.Models
{
    public class Artifact
    {
        //This assignment's advanced features are inspired by Genshin Impact.
        //This Artifact.cs has no code references, only game design references.
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Skill { get; set; }
        public int Rarity { get; set; }
        public int ArtifactAttack { get; set; }
        public int ArtifactDefense { get; set; }
        public int ArtifactCritRate { get; set; }
        public int ArtifactCritDamage { get; set; }
    }
}