﻿// <auto-generated />
using System;
using MPPilot.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MPPilot.Domain.Migrations
{
    [DbContext(typeof(MPPilotContext))]
    partial class MPPilotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MPPilot.Domain.Models.Accounts.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AccountSettingsId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9167), new TimeSpan(0, 7, 0, 0, 0)));

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9371), new TimeSpan(0, 7, 0, 0, 0)));

                    b.HasKey("Id");

                    b.HasIndex("AccountSettingsId")
                        .IsUnique();

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("MPPilot.Domain.Models.Accounts.AccountSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9636), new TimeSpan(0, 7, 0, 0, 0)));

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9739), new TimeSpan(0, 7, 0, 0, 0)));

                    b.Property<string>("WildberriesApiKey")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("WildberriesApiKey")
                        .IsUnique();

                    b.ToTable("AccountSettings");
                });

            modelBuilder.Entity("MPPilot.Domain.Models.Autobidders.AdvertBid", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdvertKeyword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("AdvertPosition")
                        .HasColumnType("integer");

                    b.Property<Guid?>("AutobidderId")
                        .HasColumnType("uuid");

                    b.Property<int>("AutobidderMode")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 383, DateTimeKind.Unspecified).AddTicks(239), new TimeSpan(0, 7, 0, 0, 0)));

                    b.Property<int>("CurrentCPM")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("LastCPM")
                        .HasColumnType("integer");

                    b.Property<int>("Reason")
                        .HasColumnType("integer");

                    b.Property<int>("TargetPositionLeftBound")
                        .HasColumnType("integer");

                    b.Property<int>("TargetPositionRightBound")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 383, DateTimeKind.Unspecified).AddTicks(351), new TimeSpan(0, 7, 0, 0, 0)));

                    b.HasKey("Id");

                    b.HasIndex("AutobidderId");

                    b.ToTable("AdvertBids");
                });

            modelBuilder.Entity("MPPilot.Domain.Models.Autobidders.Autobidder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<int>("AdvertId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9917), new TimeSpan(0, 7, 0, 0, 0)));

                    b.Property<double>("DailyBudget")
                        .HasColumnType("double precision");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<int>("Mode")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 383, DateTimeKind.Unspecified).AddTicks(8), new TimeSpan(0, 7, 0, 0, 0)));

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("AdvertId")
                        .IsUnique();

                    b.ToTable("Autobidders");
                });

            modelBuilder.Entity("MPPilot.Domain.Models.Accounts.Account", b =>
                {
                    b.HasOne("MPPilot.Domain.Models.Accounts.AccountSettings", "Settings")
                        .WithOne("Account")
                        .HasForeignKey("MPPilot.Domain.Models.Accounts.Account", "AccountSettingsId");

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("MPPilot.Domain.Models.Autobidders.AdvertBid", b =>
                {
                    b.HasOne("MPPilot.Domain.Models.Autobidders.Autobidder", null)
                        .WithMany("Bids")
                        .HasForeignKey("AutobidderId");
                });

            modelBuilder.Entity("MPPilot.Domain.Models.Autobidders.Autobidder", b =>
                {
                    b.HasOne("MPPilot.Domain.Models.Accounts.Account", "Account")
                        .WithMany("Autobidders")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("MPPilot.Domain.Models.Accounts.Account", b =>
                {
                    b.Navigation("Autobidders");
                });

            modelBuilder.Entity("MPPilot.Domain.Models.Accounts.AccountSettings", b =>
                {
                    b.Navigation("Account")
                        .IsRequired();
                });

            modelBuilder.Entity("MPPilot.Domain.Models.Autobidders.Autobidder", b =>
                {
                    b.Navigation("Bids");
                });
#pragma warning restore 612, 618
        }
    }
}
