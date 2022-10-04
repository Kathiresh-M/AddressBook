﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository;

#nullable disable

namespace Repository.Migrations
{
    [DbContext(typeof(BookRepository))]
    partial class BookRepositoryModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Entities.Models.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Line1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Line2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RefTerm_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("State_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("User_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RefTerm_Id");

                    b.HasIndex("User_Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("Entities.Models.Assert", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Asserts")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Profile_Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Profile_Id");

                    b.ToTable("Assert");
                });

            modelBuilder.Entity("Entities.Models.Email", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Profile_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RefTerm_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("User_Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Profile_Id");

                    b.HasIndex("RefTerm_Id");

                    b.ToTable("Email");
                });

            modelBuilder.Entity("Entities.Models.Phone", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Phone_Number")
                        .HasColumnType("int");

                    b.Property<string>("Phone_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Profile_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RefTerm_Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Profile_Id");

                    b.HasIndex("RefTerm_Id");

                    b.ToTable("Phone");
                });

            modelBuilder.Entity("Entities.Models.Profile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("First_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Last_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("User_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Profile");
                });

            modelBuilder.Entity("Entities.Models.RefSet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RefSet_Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RefSet");
                });

            modelBuilder.Entity("Entities.Models.RefSetTerm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RefSet_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RefTerm_Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RefSet_Id");

                    b.HasIndex("RefTerm_Id");

                    b.ToTable("RefSetTerm");
                });

            modelBuilder.Entity("Entities.Models.RefTerm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RefTerm_Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RefTerm");
                });

            modelBuilder.Entity("Entities.Models.Address", b =>
                {
                    b.HasOne("Entities.Models.RefTerm", "RefTerm")
                        .WithMany()
                        .HasForeignKey("RefTerm_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Models.Profile", "Profile")
                        .WithMany("Address")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");

                    b.Navigation("RefTerm");
                });

            modelBuilder.Entity("Entities.Models.Assert", b =>
                {
                    b.HasOne("Entities.Models.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("Profile_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Entities.Models.Email", b =>
                {
                    b.HasOne("Entities.Models.Profile", "Profile")
                        .WithMany("Email")
                        .HasForeignKey("Profile_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Models.RefTerm", "RefTerm")
                        .WithMany()
                        .HasForeignKey("RefTerm_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");

                    b.Navigation("RefTerm");
                });

            modelBuilder.Entity("Entities.Models.Phone", b =>
                {
                    b.HasOne("Entities.Models.Profile", "Profile")
                        .WithMany("Phone")
                        .HasForeignKey("Profile_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Models.RefTerm", "RefTerm")
                        .WithMany()
                        .HasForeignKey("RefTerm_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");

                    b.Navigation("RefTerm");
                });

            modelBuilder.Entity("Entities.Models.RefSetTerm", b =>
                {
                    b.HasOne("Entities.Models.RefSet", "RefSet")
                        .WithMany()
                        .HasForeignKey("RefSet_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Models.RefTerm", "RefTerm")
                        .WithMany()
                        .HasForeignKey("RefTerm_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RefSet");

                    b.Navigation("RefTerm");
                });

            modelBuilder.Entity("Entities.Models.Profile", b =>
                {
                    b.Navigation("Address");

                    b.Navigation("Email");

                    b.Navigation("Phone");
                });
#pragma warning restore 612, 618
        }
    }
}
