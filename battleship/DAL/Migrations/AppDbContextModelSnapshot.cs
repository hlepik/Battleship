﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Boat", b =>
                {
                    b.Property<int>("BoatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Inserted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.HasKey("BoatId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Boats");
                });

            modelBuilder.Entity("Domain.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<int>("GameOptionId")
                        .HasColumnType("int");

                    b.Property<bool>("NextMoveByX")
                        .HasColumnType("bit");

                    b.Property<int>("PlayerAId")
                        .HasColumnType("int");

                    b.Property<int>("PlayerBId")
                        .HasColumnType("int");

                    b.HasKey("GameId");

                    b.HasIndex("GameOptionId");

                    b.HasIndex("PlayerAId")
                        .IsUnique();

                    b.HasIndex("PlayerBId")
                        .IsUnique();

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Domain.GameBoat", b =>
                {
                    b.Property<int>("GameBoatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Direction")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsInserted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSunken")
                        .HasColumnType("bit");

                    b.Property<int>("LifeCount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("ShipId")
                        .HasColumnType("int");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.HasKey("GameBoatId");

                    b.HasIndex("PlayerId");

                    b.ToTable("GameBoats");
                });

            modelBuilder.Entity("Domain.GameOption", b =>
                {
                    b.Property<int>("GameOptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoardHeight")
                        .HasColumnType("int");

                    b.Property<int>("BoardWidth")
                        .HasColumnType("int");

                    b.Property<int>("EBoatsCanTouch")
                        .HasColumnType("int");

                    b.Property<int>("ENextMoveAfterHit")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("GameOptionId");

                    b.ToTable("GameOptions");
                });

            modelBuilder.Entity("Domain.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EPlayerType")
                        .HasColumnType("int");

                    b.Property<int?>("GameId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("PlayerId");

                    b.HasIndex("GameId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Domain.PlayerBoardState", b =>
                {
                    b.Property<int>("PlayerBoardStateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BoardState")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("PlayerBoardStateId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerBoardStates");
                });

            modelBuilder.Entity("Domain.Boat", b =>
                {
                    b.HasOne("Domain.Player", "Player")
                        .WithMany("Boats")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Game", b =>
                {
                    b.HasOne("Domain.GameOption", "GameOption")
                        .WithMany("Games")
                        .HasForeignKey("GameOptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Player", "PlayerA")
                        .WithOne()
                        .HasForeignKey("Domain.Game", "PlayerAId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Player", "PlayerB")
                        .WithOne()
                        .HasForeignKey("Domain.Game", "PlayerBId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.GameBoat", b =>
                {
                    b.HasOne("Domain.Player", "Player")
                        .WithMany("GameBoats")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Player", b =>
                {
                    b.HasOne("Domain.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("Domain.PlayerBoardState", b =>
                {
                    b.HasOne("Domain.Player", "Player")
                        .WithMany("PlayerBoardStates")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
