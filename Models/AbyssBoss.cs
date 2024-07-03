using System.ComponentModel.DataAnnotations;

namespace MushroomPocket.Models
{
    public class AbyssBoss
    {
        //This assignment's advanced features are inspired by Genshin Impact.
        //This AbyssBoss.cs has no code references, only game design references.
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Skill { get; set; }
        public int AbyssBossHP { get; set; }
        public int AbyssBossAttack { get; set; }
        public int AbyssBossDefense { get; set; }
        public int AbyssBossCritRate { get; set; }
        public int AbyssBossCritDamage { get; set; }
    }
}