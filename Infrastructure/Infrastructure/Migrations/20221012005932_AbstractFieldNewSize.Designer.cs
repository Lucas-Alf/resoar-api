﻿// <auto-generated />
using System;
using Infrastructure.DBConfiguration.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20221012005932_AbstractFieldNewSize")]
    partial class AbstractFieldNewSize
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

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

                    b.Property<string>("Abstract")
                        .HasMaxLength(2500)
                        .HasColumnType("character varying(2500)");

                    b.Property<DateTime?>("CreatedAt")
                        .IsRequired()
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("CreatedById")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<Guid?>("FileKey")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<int?>("InstitutionId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("RawContent")
                        .HasColumnType("text");

                    b.Property<NpgsqlTsVector>("SearchVector")
                        .HasColumnType("tsvector");

                    b.Property<Guid?>("ThumbnailKey")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(350)
                        .HasColumnType("character varying(350)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<int>("Visibility")
                        .HasColumnType("integer");

                    b.Property<int?>("Year")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("InstitutionId");

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

                    b.Property<bool>("Approved")
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
                        .HasColumnType("timestamp without time zone");

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

            modelBuilder.Entity("Domain.Entities.Research", b =>
                {
                    b.HasOne("Domain.Entities.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Institution", "Institution")
                        .WithMany("Researches")
                        .HasForeignKey("InstitutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

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
                        .WithMany("ResearchAuthors")
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
                    b.Navigation("ResearchAuthors");
                });
#pragma warning restore 612, 618
        }
    }
}
