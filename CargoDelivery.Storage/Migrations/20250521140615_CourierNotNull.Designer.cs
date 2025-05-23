﻿// <auto-generated />
using System;
using CargoDelivery.Storage.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CargoDelivery.Storage.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20250521140615_CourierNotNull")]
    partial class CourierNotNull
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CargoDelivery.Storage.Entities.CargoDb", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Cargos");
                });

            modelBuilder.Entity("CargoDelivery.Storage.Entities.ClientDb", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("CargoDelivery.Storage.Entities.CourierDb", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Couriers");
                });

            modelBuilder.Entity("CargoDelivery.Storage.Entities.OrderDb", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CargoId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CourierId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("DeletedComment")
                        .HasColumnType("text");

                    b.Property<string>("DestinationAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DestinationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("TakeAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TakeDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CargoId");

                    b.HasIndex("ClientId");

                    b.HasIndex("CourierId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("CargoDelivery.Storage.Entities.OrderDb", b =>
                {
                    b.HasOne("CargoDelivery.Storage.Entities.CargoDb", "Cargo")
                        .WithMany()
                        .HasForeignKey("CargoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CargoDelivery.Storage.Entities.ClientDb", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CargoDelivery.Storage.Entities.CourierDb", "Courier")
                        .WithMany()
                        .HasForeignKey("CourierId");

                    b.Navigation("Cargo");

                    b.Navigation("Client");

                    b.Navigation("Courier");
                });
#pragma warning restore 612, 618
        }
    }
}
