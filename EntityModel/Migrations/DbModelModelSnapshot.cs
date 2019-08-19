﻿// <auto-generated />
using System;
using EntityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EntityModel.Migrations
{
    [DbContext(typeof(DbModel))]
    partial class DbModelModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EntityModel.CaseManagementData", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("HasWaitlist");

                    b.Property<bool>("IsComplete");

                    b.Property<string>("Name");

                    b.Property<int>("Open");

                    b.Property<string>("ServiceId");

                    b.Property<int>("Total");

                    b.Property<int>("WaitlistLength");

                    b.HasKey("Id");

                    b.ToTable("CaseManagementData");
                });

            modelBuilder.Entity("EntityModel.HousingData", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("EmergencyPrivateBedsOpen");

                    b.Property<int>("EmergencyPrivateBedsTotal");

                    b.Property<int>("EmergencyPrivateBedsWaitlistLength");

                    b.Property<int>("EmergencySharedBedsOpen");

                    b.Property<int>("EmergencySharedBedsTotal");

                    b.Property<int>("EmergencySharedBedsWaitlistLength");

                    b.Property<bool>("HasWaitlist");

                    b.Property<bool>("IsComplete");

                    b.Property<int>("LongTermPrivateBedsOpen");

                    b.Property<int>("LongTermPrivateBedsTotal");

                    b.Property<int>("LongTermPrivateBedsWaitlistLength");

                    b.Property<int>("LongTermSharedBedsOpen");

                    b.Property<int>("LongTermSharedBedsTotal");

                    b.Property<int>("LongTermSharedBedsWaitlistLength");

                    b.Property<string>("Name");

                    b.Property<string>("ServiceId");

                    b.HasKey("Id");

                    b.ToTable("HousingData");
                });

            modelBuilder.Entity("EntityModel.JobTrainingData", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("HasWaitlist");

                    b.Property<bool>("IsComplete");

                    b.Property<string>("Name");

                    b.Property<int>("Open");

                    b.Property<string>("ServiceId");

                    b.Property<int>("Total");

                    b.Property<int>("WaitlistLength");

                    b.HasKey("Id");

                    b.ToTable("JobTrainingData");
                });

            modelBuilder.Entity("EntityModel.MentalHealthData", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("HasWaitlist");

                    b.Property<int>("InPatientOpen");

                    b.Property<int>("InPatientTotal");

                    b.Property<int>("InPatientWaitlistLength");

                    b.Property<bool>("IsComplete");

                    b.Property<string>("Name");

                    b.Property<int>("OutPatientOpen");

                    b.Property<int>("OutPatientTotal");

                    b.Property<int>("OutPatientWaitlistLength");

                    b.Property<string>("ServiceId");

                    b.HasKey("Id");

                    b.ToTable("MentalHealthData");
                });

            modelBuilder.Entity("EntityModel.Organization", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsVerified");

                    b.Property<string>("Location");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("EntityModel.Service", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("EntityModel.SubstanceUseData", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("DetoxOpen");

                    b.Property<int>("DetoxTotal");

                    b.Property<int>("DetoxWaitlistLength");

                    b.Property<int>("GroupOpen");

                    b.Property<int>("GroupTotal");

                    b.Property<int>("GroupWaitlistLength");

                    b.Property<bool>("HasWaitlist");

                    b.Property<int>("InPatientOpen");

                    b.Property<int>("InPatientTotal");

                    b.Property<int>("InPatientWaitlistLength");

                    b.Property<bool>("IsComplete");

                    b.Property<string>("Name");

                    b.Property<int>("OutPatientOpen");

                    b.Property<int>("OutPatientTotal");

                    b.Property<int>("OutPatientWaitlistLength");

                    b.Property<string>("ServiceId");

                    b.HasKey("Id");

                    b.ToTable("SubstanceUseData");
                });

            modelBuilder.Entity("EntityModel.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ContactEnabled");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("PhoneNumber");

                    b.Property<int>("UpdateFrequency");

                    b.HasKey("Id");

                    b.HasIndex("PhoneNumber")
                        .IsUnique()
                        .HasFilter("[PhoneNumber] IS NOT NULL");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
