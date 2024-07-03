using Microsoft.EntityFrameworkCore;
using MushroomPocket.Models;

namespace MushroomPocket
{
    public class MushroomDBContext : DbContext
    {
        public DbSet<AbyssBoss> AbyssBosses{ get; set; }
        public DbSet<Artifact> Artifacts { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Pocket> Pockets { get; set; }
        public DbSet<SpecialCharacter> SpecialCharacters { get; set; }

        //Some Database Classes(Models) are under personal development and are not fully developed for the current IT2154 Advanced Programming Assignment 1 Submission. Please ignore them, they include Artifacts, AbyssBosses.      
        public MushroomDBContext()
        {
        }

        public MushroomDBContext(DbContextOptions<MushroomDBContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use SQLite for migrations and runtime database
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=Database/MushroomPocket.db");
            }
        }

        //Characters are added here to ensure that Gacha can work at program start-time
        //Special Characters are based off a game called Genshin Impact by Mihoyo and Hoyoverse, this will be an ongoing personal project.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>().HasData(
                new Character { Id = 1, Name = "Waluigi", Skill = "Agility", Rarity = 4 },
                new Character { Id = 2, Name = "Daisy", Skill = "Leadership", Rarity = 4 },
                new Character { Id = 3, Name = "Wario", Skill = "Strength", Rarity = 4 },
                new Character { Id = 4, Name = "Luigi", Skill = "Precision and Accuracy", Rarity = 4 },
                new Character { Id = 5, Name = "Peach", Skill = "Magic Abilities", Rarity = 4 },
                new Character { Id = 6, Name = "Mario", Skill = "Combat Skills", Rarity = 4 }
            );

            modelBuilder.Entity<SpecialCharacter>().HasData(
                new SpecialCharacter {Id = 1, Name = "Furina", Skill = "Endless Solo of Solitude", Rarity = 5},
                new SpecialCharacter {Id = 2, Name = "Neuvillette", Skill = "Ordainer of Inexorable Judgement", Rarity = 5},
                new SpecialCharacter {Id = 3, Name = "Raiden Shogun", Skill = "Plane of Euthymia", Rarity = 5},
                new SpecialCharacter {Id = 4, Name = "Clorinde", Skill = "Champion Duelist", Rarity = 5}
            );
        }
    }
}