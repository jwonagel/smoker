﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api.Model.Dababase;

namespace api.Migrations
{
    [DbContext(typeof(SmokerDBContext))]
    [Migration("20200418102757_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("api.Model.Measurement", b =>
                {
                    b.Property<Guid>("MeasurementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("MEASUREMENT_ID")
                        .HasColumnType("char(36)");

                    b.Property<double>("ContentSensor")
                        .HasColumnName("CONTENT_SENSOR")
                        .HasColumnType("double");

                    b.Property<double>("FireSensor")
                        .HasColumnName("FIRE_SENSOR")
                        .HasColumnType("double");

                    b.Property<double>("Sensor1")
                        .HasColumnName("SENSOR_1")
                        .HasColumnType("double");

                    b.Property<double>("Sensor2")
                        .HasColumnName("SENSOR_2")
                        .HasColumnType("double");

                    b.Property<double>("Sensor3")
                        .HasColumnName("SENSOR_3")
                        .HasColumnType("double");

                    b.Property<double>("Sensor4")
                        .HasColumnName("SENSOR_4")
                        .HasColumnType("double");

                    b.HasKey("MeasurementId");

                    b.ToTable("MEASUREMENT");
                });
#pragma warning restore 612, 618
        }
    }
}