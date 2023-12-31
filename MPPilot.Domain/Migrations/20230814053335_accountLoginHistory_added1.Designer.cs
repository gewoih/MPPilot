﻿// <auto-generated />
using System;
using MPPilot.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MPPilot.Domain.Migrations
{
    [DbContext(typeof(MPPilotContext))]
    [Migration("20230814053335_accountLoginHistory_added1")]
    partial class accountLoginHistory_added1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
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
                        .HasColumnType("timestamp with time zone");

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
                        .HasColumnType("timestamp with time zone");

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
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("WildberriesApiKey")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("WildberriesApiKey")
                        .IsUnique();

                    b.ToTable("AccountSettings");
                });

            modelBuilder.Entity("MPPilot.Domain.Models.Accounts.LoginHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<string>("BrowserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DeviceName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IPAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSuccessful")
                        .HasColumnType("boolean");

                    b.Property<string>("OSName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("LoginsHistory");
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

                    b.Property<Guid>("AutobidderId")
                        .HasColumnType("uuid");

                    b.Property<int>("AutobidderMode")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

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
                        .HasColumnType("timestamp with time zone");

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

                    b.Property<DateTimeOffset?>("BidsPausedTill")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

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
                        .HasColumnType("timestamp with time zone");

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

            modelBuilder.Entity("MPPilot.Domain.Models.Accounts.LoginHistory", b =>
                {
                    b.HasOne("MPPilot.Domain.Models.Accounts.Account", null)
                        .WithMany("LoginsHistory")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MPPilot.Domain.Models.Autobidders.AdvertBid", b =>
                {
                    b.HasOne("MPPilot.Domain.Models.Autobidders.Autobidder", null)
                        .WithMany("Bids")
                        .HasForeignKey("AutobidderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

                    b.Navigation("LoginsHistory");
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
