﻿// <auto-generated />
using ExchangeRates.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ExchangeRates.Domain.Entities.ExchangeRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Ask")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Bid")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("ExchangeRates");
                });

            modelBuilder.Entity("ExchangeRates.Domain.Entities.ExchangeRate", b =>
                {
                    b.OwnsOne("ExchangeRates.Domain.Entities.CurrencyPair", "Pair", b1 =>
                        {
                            b1.Property<int>("ExchangeRateId")
                                .HasColumnType("integer");

                            b1.Property<string>("BaseCurrency")
                                .HasColumnType("text");

                            b1.Property<string>("QuoteCurrency")
                                .HasColumnType("text");

                            b1.HasKey("ExchangeRateId");

                            b1.ToTable("ExchangeRates");

                            b1.WithOwner()
                                .HasForeignKey("ExchangeRateId");
                        });

                    b.Navigation("Pair");
                });
#pragma warning restore 612, 618
        }
    }
}
