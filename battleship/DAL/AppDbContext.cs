﻿using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Boat> Boats { get; set; } = null!;
        public DbSet<Game>? Games { get; set; } = null!;
        public DbSet<GameBoat>? GameBoats { get; set; } = null!;
        public DbSet<GameOption> GameOptions { get; set; } = null!;
        public DbSet<Player> Players{ get; set; } = null!;
        public DbSet<PlayerBoardState> PlayerBoardStates { get; set; } = null!;


        private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(
            builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Information);

            }
        );

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseLoggerFactory(_loggerFactory)
                .EnableSensitiveDataLogging()
                .UseSqlServer(@"
                    Server=barrel.itcollege.ee,1533;
                    User Id=student;
                    Password=Student.Bad.password.0;
                    Database=hlepik_battleship;
                    MultipleActiveResultSets=true;
                    "
                );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Game>()
                .HasOne(game => game.PlayerA).WithOne()
                .HasForeignKey<Game>(g => g.PlayerAId)
                .OnDelete(DeleteBehavior.Restrict);

          // remove the cascade delete
            modelBuilder.Entity<Game>()
                .HasOne(game => game.PlayerB).WithOne()
                .HasForeignKey<Game>(g => g.PlayerBId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}