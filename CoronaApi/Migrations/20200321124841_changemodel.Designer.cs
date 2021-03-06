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
    [Migration("20200321124841_changemodel")]
    partial class changemodel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
