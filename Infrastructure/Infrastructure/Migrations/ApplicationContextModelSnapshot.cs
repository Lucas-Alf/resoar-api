﻿// <auto-generated />
using System;
using Infrastructure.DBConfiguration.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Institution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ImagePath")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Institution");
                });

            modelBuilder.Entity("Domain.Entities.KeyWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("KeyWord");
                });

            modelBuilder.Entity("Domain.Entities.KnowledgeArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("KnowledgeArea");
                });

            modelBuilder.Entity("Domain.Entities.Research", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<NpgsqlTsVector>("FileVector")
                        .IsRequired()
                        .HasColumnType("tsvector");

                    b.Property<int?>("InstitutionId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TextContent")
                        .HasColumnType("text");

                    b.Property<string>("ThumbnailPath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(350)
                        .HasColumnType("character varying(350)");

                    b.Property<int?>("Type")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.Property<int?>("Visibility")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("Year")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("InstitutionId");

                    b.HasIndex("UserId");

                    b.ToTable("Research");
                });

            modelBuilder.Entity("Domain.Entities.ResearchAdvisor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ResearchId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("UserId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ResearchId");

                    b.HasIndex("UserId");

                    b.ToTable("ResearchAdvisor");
                });

            modelBuilder.Entity("Domain.Entities.ResearchAdvisorApproval", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Approved")
                        .IsRequired()
                        .HasColumnType("boolean");

                    b.Property<int?>("ResearchId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("UserId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ResearchId");

                    b.HasIndex("UserId");

                    b.ToTable("ResearchAdvisorApproval");
                });

            modelBuilder.Entity("Domain.Entities.ResearchAuthor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ResearchId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("UserId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ResearchId");

                    b.HasIndex("UserId");

                    b.ToTable("ResearchAuthor");
                });

            modelBuilder.Entity("Domain.Entities.ResearchKeyWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("KeyWordId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("ResearchId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("KeyWordId");

                    b.HasIndex("ResearchId");

                    b.ToTable("ResearchKeyWord");
                });

            modelBuilder.Entity("Domain.Entities.ResearchKnowledgeArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("KnowledgeAreaId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("ResearchId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("KnowledgeAreaId");

                    b.HasIndex("ResearchId");

                    b.ToTable("ResearchKnowledgeArea");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("FailLoginCount")
                        .HasColumnType("integer");

                    b.Property<string>("ImagePath")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("Domain.Entities.Institution", b =>
                {
                    b.HasOne("Domain.Entities.User", null)
                        .WithMany("Institutions")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Domain.Entities.Research", b =>
                {
                    b.HasOne("Domain.Entities.Institution", "Institution")
                        .WithMany("Researches")
                        .HasForeignKey("InstitutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", null)
                        .WithMany("Researches")
                        .HasForeignKey("UserId");

                    b.Navigation("Institution");
                });

            modelBuilder.Entity("Domain.Entities.ResearchAdvisor", b =>
                {
                    b.HasOne("Domain.Entities.Research", "Research")
                        .WithMany("Advisors")
                        .HasForeignKey("ResearchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Research");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.ResearchAdvisorApproval", b =>
                {
                    b.HasOne("Domain.Entities.Research", "Research")
                        .WithMany("ResearchAdvisorApprovals")
                        .HasForeignKey("ResearchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Research");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.ResearchAuthor", b =>
                {
                    b.HasOne("Domain.Entities.Research", "Research")
                        .WithMany("Authors")
                        .HasForeignKey("ResearchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Research");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.ResearchKeyWord", b =>
                {
                    b.HasOne("Domain.Entities.KeyWord", "KeyWord")
                        .WithMany()
                        .HasForeignKey("KeyWordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Research", "Research")
                        .WithMany("ResearchKeyWords")
                        .HasForeignKey("ResearchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KeyWord");

                    b.Navigation("Research");
                });

            modelBuilder.Entity("Domain.Entities.ResearchKnowledgeArea", b =>
                {
                    b.HasOne("Domain.Entities.KnowledgeArea", "KnowledgeArea")
                        .WithMany()
                        .HasForeignKey("KnowledgeAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Research", "Research")
                        .WithMany("ResearchKnowledgeAreas")
                        .HasForeignKey("ResearchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KnowledgeArea");

                    b.Navigation("Research");
                });

            modelBuilder.Entity("Domain.Entities.Institution", b =>
                {
                    b.Navigation("Researches");
                });

            modelBuilder.Entity("Domain.Entities.Research", b =>
                {
                    b.Navigation("Advisors");

                    b.Navigation("Authors");

                    b.Navigation("ResearchAdvisorApprovals");

                    b.Navigation("ResearchKeyWords");

                    b.Navigation("ResearchKnowledgeAreas");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("Institutions");

                    b.Navigation("Researches");
                });
#pragma warning restore 612, 618
        }
    }
}
