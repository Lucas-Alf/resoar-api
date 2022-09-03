using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Institution",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ImagePath = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institution", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeyWord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyWord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeArea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeArea", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    ImagePath = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FailLoginCount = table.Column<int>(type: "integer", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Research",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Visibility = table.Column<int>(type: "integer", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    ThumbnailPath = table.Column<string>(type: "text", nullable: false),
                    FileVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false),
                    RawContent = table.Column<string>(type: "text", nullable: true),
                    InstitutionId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Research", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Research_Institution_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchAdvisor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResearchId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchAdvisor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchAdvisor_Research_ResearchId",
                        column: x => x.ResearchId,
                        principalTable: "Research",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResearchAdvisor_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchAdvisorApproval",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResearchId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Approved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchAdvisorApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchAdvisorApproval_Research_ResearchId",
                        column: x => x.ResearchId,
                        principalTable: "Research",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResearchAdvisorApproval_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchAuthor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResearchId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchAuthor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchAuthor_Research_ResearchId",
                        column: x => x.ResearchId,
                        principalTable: "Research",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResearchAuthor_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchKeyWord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResearchId = table.Column<int>(type: "integer", nullable: false),
                    KeyWordId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchKeyWord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchKeyWord_KeyWord_KeyWordId",
                        column: x => x.KeyWordId,
                        principalTable: "KeyWord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResearchKeyWord_Research_ResearchId",
                        column: x => x.ResearchId,
                        principalTable: "Research",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchKnowledgeArea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResearchId = table.Column<int>(type: "integer", nullable: false),
                    KnowledgeAreaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchKnowledgeArea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchKnowledgeArea_KnowledgeArea_KnowledgeAreaId",
                        column: x => x.KnowledgeAreaId,
                        principalTable: "KnowledgeArea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResearchKnowledgeArea_Research_ResearchId",
                        column: x => x.ResearchId,
                        principalTable: "Research",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Institution_Name",
                table: "Institution",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Research_InstitutionId",
                table: "Research",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchAdvisor_ResearchId",
                table: "ResearchAdvisor",
                column: "ResearchId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchAdvisor_UserId",
                table: "ResearchAdvisor",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchAdvisorApproval_ResearchId",
                table: "ResearchAdvisorApproval",
                column: "ResearchId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchAdvisorApproval_UserId",
                table: "ResearchAdvisorApproval",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchAuthor_ResearchId",
                table: "ResearchAuthor",
                column: "ResearchId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchAuthor_UserId",
                table: "ResearchAuthor",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchKeyWord_KeyWordId",
                table: "ResearchKeyWord",
                column: "KeyWordId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchKeyWord_ResearchId",
                table: "ResearchKeyWord",
                column: "ResearchId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchKnowledgeArea_KnowledgeAreaId",
                table: "ResearchKnowledgeArea",
                column: "KnowledgeAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchKnowledgeArea_ResearchId",
                table: "ResearchKnowledgeArea",
                column: "ResearchId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResearchAdvisor");

            migrationBuilder.DropTable(
                name: "ResearchAdvisorApproval");

            migrationBuilder.DropTable(
                name: "ResearchAuthor");

            migrationBuilder.DropTable(
                name: "ResearchKeyWord");

            migrationBuilder.DropTable(
                name: "ResearchKnowledgeArea");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "KeyWord");

            migrationBuilder.DropTable(
                name: "KnowledgeArea");

            migrationBuilder.DropTable(
                name: "Research");

            migrationBuilder.DropTable(
                name: "Institution");
        }
    }
}
