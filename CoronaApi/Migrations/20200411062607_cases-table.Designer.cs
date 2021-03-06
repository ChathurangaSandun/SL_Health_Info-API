﻿// <auto-generated />
using System;
using CoronaApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoronaApi.Migrations
{
    [DbContext(typeof(CoronaStatsDbContext))]
    [Migration("20200411062607_cases-table")]
    partial class casestable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoronaApi.Models.Case", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Area")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CaseNumber")
                        .HasColumnType("int");

                    b.Property<string>("DetectedFrom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("GovCreated")
                        .HasColumnType("datetime2");

                    b.Property<int>("GovId")
                        .HasColumnType("int");

                    b.Property<bool>("IsLocal")
                        .HasColumnType("bit");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<bool>("Local")
                        .HasColumnType("bit");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<DateTime>("ReportedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Cases");
                });

            modelBuilder.Entity("CoronaApi.Models.CoronaRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CasesCount")
                        .HasColumnType("bigint");

                    b.Property<long>("DeathCount")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("Date");

                    b.Property<long>("RecoverCount")
                        .HasColumnType("bigint");

                    b.Property<long>("SuspectCount")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("CoronaRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
