﻿// <auto-generated />
using System;
using CodeBidder.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace SQMS.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CodeBidder.Models.ProjectDetailsModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalFeatures")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BudgetRange")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ColorScheme")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Competitors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoreFeatures")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DbRequirements")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExpectedTimeline")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IntendedAudience")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Miscellaneous")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("OngoingMaintenance")
                        .HasColumnType("bit");

                    b.Property<string>("OtherPlatform")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PostLaunchSupport")
                        .HasColumnType("bit");

                    b.Property<string>("ProjectDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectScope")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TargetPlatform")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TechStack")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UiDesign")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProjectDetails");
                });

            modelBuilder.Entity("CodeBidder.Models.Quotation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DeveloperId")
                        .HasColumnType("int");

                    b.Property<decimal>("EstimatedCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Timeline")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DeveloperId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Quotations");
                });

            modelBuilder.Entity("CodeBidder.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CodeBidder.Models.UserProject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProjectDetailId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectDetailId");

                    b.HasIndex("UserId");

                    b.ToTable("UserProject");
                });

            modelBuilder.Entity("CodeBidder.Models.Quotation", b =>
                {
                    b.HasOne("CodeBidder.Models.User", "Developer")
                        .WithMany()
                        .HasForeignKey("DeveloperId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CodeBidder.Models.ProjectDetailsModel", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Developer");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("CodeBidder.Models.UserProject", b =>
                {
                    b.HasOne("CodeBidder.Models.ProjectDetailsModel", "ProjectDetail")
                        .WithMany("Enrollment")
                        .HasForeignKey("ProjectDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CodeBidder.Models.User", "User")
                        .WithMany("Enrollment")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProjectDetail");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CodeBidder.Models.ProjectDetailsModel", b =>
                {
                    b.Navigation("Enrollment");
                });

            modelBuilder.Entity("CodeBidder.Models.User", b =>
                {
                    b.Navigation("Enrollment");
                });
#pragma warning restore 612, 618
        }
    }
}
