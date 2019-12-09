using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreSelectManyTest.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Children",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    ParentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Children", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Children_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParentVersions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    ParentId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PreviousVersionId = table.Column<long>(nullable: true),
                    NextVersionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentVersions_ParentVersions_NextVersionId",
                        column: x => x.NextVersionId,
                        principalTable: "ParentVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParentVersions_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVersions_ParentVersions_PreviousVersionId",
                        column: x => x.PreviousVersionId,
                        principalTable: "ParentVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChildVersions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    ChildId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PreviousVersionId = table.Column<long>(nullable: true),
                    NextVersionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildVersions_Children_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Children",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChildVersions_ChildVersions_NextVersionId",
                        column: x => x.NextVersionId,
                        principalTable: "ChildVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChildVersions_ChildVersions_PreviousVersionId",
                        column: x => x.PreviousVersionId,
                        principalTable: "ChildVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Children_ParentId",
                table: "Children",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildVersions_ChildId",
                table: "ChildVersions",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildVersions_NextVersionId",
                table: "ChildVersions",
                column: "NextVersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChildVersions_PreviousVersionId",
                table: "ChildVersions",
                column: "PreviousVersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParentVersions_NextVersionId",
                table: "ParentVersions",
                column: "NextVersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParentVersions_ParentId",
                table: "ParentVersions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVersions_PreviousVersionId",
                table: "ParentVersions",
                column: "PreviousVersionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildVersions");

            migrationBuilder.DropTable(
                name: "ParentVersions");

            migrationBuilder.DropTable(
                name: "Children");

            migrationBuilder.DropTable(
                name: "Parents");
        }
    }
}
