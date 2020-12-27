using System;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Answers>? Answer { get; set; }
        public DbSet<Player>? Players { get; set; }
        public DbSet<Question>? Question { get; set; }
        public DbSet<Quiz>? Quizzes { get; set; }
        public DbSet<Statistic>? Statistics { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}