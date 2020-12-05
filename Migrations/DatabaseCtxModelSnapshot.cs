﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Prism.Data;

namespace Prism.Migrations
{
    [DbContext(typeof(DatabaseCtx))]
    partial class DatabaseCtxModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
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

            modelBuilder.Entity("Prism.Models.NewsSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("NewsSources");
                });

            modelBuilder.Entity("Prism.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Salt")
                        .HasColumnType("varbinary(max)");

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
