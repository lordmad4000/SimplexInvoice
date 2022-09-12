﻿// <auto-generated />
using System;
using Invoice.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Invoice.Infra.Migrations
{
    [DbContext(typeof(EFContext))]
    [Migration("20220701172418_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Invoice.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("Invoice.Domain.Entities.User", b =>
                {
                    b.OwnsOne("Invoice.Domain.ValueObjects.EmailAddress", "EmailAddress", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("Address")
                                .IsRequired()
                                .HasColumnName("EmailAddress")
                                .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                            b1.HasKey("UserId");

                            b1.HasIndex("Address")
                                .IsUnique();

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
