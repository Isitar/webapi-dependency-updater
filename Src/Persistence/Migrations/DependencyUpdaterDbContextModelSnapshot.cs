﻿// <auto-generated />
using System;
using Isitar.DependencyUpdater.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Isitar.DependencyUpdater.Persistence.Migrations
{
    [DbContext(typeof(DependencyUpdaterDbContext))]
    partial class DependencyUpdaterDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("Isitar.DependencyUpdater.Domain.Entities.Platform", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ApiBaseUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("GitUserEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("GitUserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("PlatformType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PrivateKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Platforms");
                });

            modelBuilder.Entity("Isitar.DependencyUpdater.Domain.Entities.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("CheckRequested")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsChecking")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsOutdated")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PlatformId")
                        .HasColumnType("TEXT");

                    b.Property<string>("PlatformProjectId")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProjectType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TargetBranch")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdateFrequency")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PlatformId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Isitar.DependencyUpdater.Domain.Entities.Project", b =>
                {
                    b.HasOne("Isitar.DependencyUpdater.Domain.Entities.Platform", "Platform")
                        .WithMany()
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Platform");
                });
#pragma warning restore 612, 618
        }
    }
}
