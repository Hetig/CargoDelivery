﻿// <auto-generated />
using System;
using CargoDelivery.Storage.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CargoDelivery.Storage.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

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

                    b.HasData(
                        new
                        {
                            Id = new Guid("7c9a412f-409e-47e1-b2c0-93961f6f4853"),
                            Name = "Client1"
                        },
                        new
                        {
                            Id = new Guid("c9cd28cc-b080-4509-96c8-145049f7908c"),
                            Name = "Client2"
                        },
                        new
                        {
                            Id = new Guid("6b0dd6ef-fa49-4b24-8471-b7b74a10c5e6"),
                            Name = "Client3"
                        });
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

                    b.HasData(
                        new
                        {
                            Id = new Guid("9bab18df-c059-414b-9e8c-7e3825c9eb28"),
                            Name = "Courier1"
                        },
                        new
                        {
                            Id = new Guid("8a9df209-331b-417b-87eb-ff2c2762b47c"),
                            Name = "Courier2"
                        },
                        new
                        {
                            Id = new Guid("deaedbb9-8e88-44a4-a639-8d6f5c43ca5f"),
                            Name = "Courier3"
                        });
                });

            modelBuilder.Entity("CargoDelivery.Storage.Entities.OrderDb", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CancelledComment")
                        .HasColumnType("text");

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

                    b.HasIndex("CargoId")
                        .IsUnique();

                    b.HasIndex("ClientId");

                    b.HasIndex("CourierId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("CargoDelivery.Storage.Entities.OrderDb", b =>
                {
                    b.HasOne("CargoDelivery.Storage.Entities.CargoDb", "Cargo")
                        .WithOne("Order")
                        .HasForeignKey("CargoDelivery.Storage.Entities.OrderDb", "CargoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CargoDelivery.Storage.Entities.ClientDb", "Client")
                        .WithMany("Orders")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CargoDelivery.Storage.Entities.CourierDb", "Courier")
                        .WithMany("Orders")
                        .HasForeignKey("CourierId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Cargo");

                    b.Navigation("Client");

                    b.Navigation("Courier");
                });

            modelBuilder.Entity("CargoDelivery.Storage.Entities.CargoDb", b =>
                {
                    b.Navigation("Order")
                        .IsRequired();
                });

            modelBuilder.Entity("CargoDelivery.Storage.Entities.ClientDb", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("CargoDelivery.Storage.Entities.CourierDb", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
