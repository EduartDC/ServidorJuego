using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataBaseConnect
{
    public partial class DataBase : DbContext
    {
        public DataBase()
            : base("name=DataBaseConnection")
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>()
                .HasMany(e => e.Questions)
                .WithMany(e => e.Answers)
                .Map(m => m.ToTable("AnswerQuestion").MapLeftKey("Answers_idAnswer").MapRightKey("Questions_idQuestion"));

            modelBuilder.Entity<Match>()
                .HasMany(e => e.Players)
                .WithMany(e => e.Matches)
                .Map(m => m.ToTable("MatchPlayer").MapLeftKey("Matches_idMatch").MapRightKey("Players_idPlayer"));

            modelBuilder.Entity<Match>()
                .HasMany(e => e.Questions)
                .WithMany(e => e.Matches)
                .Map(m => m.ToTable("MatchQuestion").MapLeftKey("Matches_idMatch").MapRightKey("Questions_idQuestion"));

            modelBuilder.Entity<Player>()
                .HasMany(e => e.Friends)
                .WithRequired(e => e.Player)
                .HasForeignKey(e => e.Player_idPlayer)
                .WillCascadeOnDelete(false);
        }
    }
}
