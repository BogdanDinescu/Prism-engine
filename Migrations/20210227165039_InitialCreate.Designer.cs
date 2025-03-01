﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Prism.Data;

namespace Prism.Migrations
{
    [DbContext(typeof(DatabaseCtx))]
    [Migration("20210227165039_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("NewsSourceUserPreference", b =>
                {
                    b.Property<int>("NewsSourcesId")
                        .HasColumnType("int");

                    b.Property<int>("UserPreferencesUserId")
                        .HasColumnType("int");

                    b.HasKey("NewsSourcesId", "UserPreferencesUserId");

                    b.HasIndex("UserPreferencesUserId");

                    b.ToTable("NewsSourceUserPreference");
                });

            modelBuilder.Entity("Prism.Models.NewsArticle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Link")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("NewsSourceId")
                        .HasColumnType("int");

                    b.Property<uint>("SimHash")
                        .HasColumnType("int unsigned");

                    b.Property<string>("Source")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("NewsSourceId");

                    b.HasIndex("SimHash");

                    b.ToTable("NewsArticles");
                });

            modelBuilder.Entity("Prism.Models.NewsSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("NewsSources");
                });

            modelBuilder.Entity("Prism.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40) CHARACTER SET utf8mb4");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Role")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<byte[]>("Salt")
                        .HasColumnType("longblob");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Prism.Models.UserPreference", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("UserPreferences");
                });

            modelBuilder.Entity("NewsSourceUserPreference", b =>
                {
                    b.HasOne("Prism.Models.NewsSource", null)
                        .WithMany()
                        .HasForeignKey("NewsSourcesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Prism.Models.UserPreference", null)
                        .WithMany()
                        .HasForeignKey("UserPreferencesUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Prism.Models.NewsArticle", b =>
                {
                    b.HasOne("Prism.Models.NewsSource", "NewsSource")
                        .WithMany()
                        .HasForeignKey("NewsSourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NewsSource");
                });

            modelBuilder.Entity("Prism.Models.UserPreference", b =>
                {
                    b.HasOne("Prism.Models.User", "User")
                        .WithOne("UserPreference")
                        .HasForeignKey("Prism.Models.UserPreference", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Prism.Models.User", b =>
                {
                    b.Navigation("UserPreference");
                });
#pragma warning restore 612, 618
        }
    }
}
