using System;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Answer> Answers { get; set; }  = default!;
        public DbSet<Player> Players { get; set; } = default!;
        public DbSet<Question> Question { get; set; } = default!;
        public DbSet<Quiz> Quizzes { get; set; } = default!;
        public DbSet<PlayerAnswer> PlayerAnswers { get; set; } = default!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}