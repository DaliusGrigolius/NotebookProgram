﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NotebookProgram.Repository.DbContexts;

#nullable disable

namespace NotebookProgram.Repository.Migrations
{
    [DbContext(typeof(NotebookDbContext))]
    [Migration("20220711082443_AddRefreshTokenModel")]
    partial class AddRefreshTokenModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CategoryNote", b =>
                {
                    b.Property<Guid>("CategoriesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("NotesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CategoriesId", "NotesId");

                    b.HasIndex("NotesId");

                    b.ToTable("CategoryNote");
                });

            modelBuilder.Entity("NotebookProgram.Repository.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("NotebookProgram.Repository.Entities.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Byte")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<Guid>("NoteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("NoteId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("NotebookProgram.Repository.Entities.Note", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("NotebookProgram.Repository.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("NotebookProgram.Repository.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CategoryNote", b =>
                {
                    b.HasOne("NotebookProgram.Repository.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NotebookProgram.Repository.Entities.Note", null)
                        .WithMany()
                        .HasForeignKey("NotesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NotebookProgram.Repository.Entities.Image", b =>
                {
                    b.HasOne("NotebookProgram.Repository.Entities.Note", "Note")
                        .WithMany("Images")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Note");
                });

            modelBuilder.Entity("NotebookProgram.Repository.Entities.Note", b =>
                {
                    b.HasOne("NotebookProgram.Repository.Entities.User", "User")
                        .WithMany("Notes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NotebookProgram.Repository.Entities.RefreshToken", b =>
                {
                    b.HasOne("NotebookProgram.Repository.Entities.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NotebookProgram.Repository.Entities.Note", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("NotebookProgram.Repository.Entities.User", b =>
                {
                    b.Navigation("Notes");

                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
