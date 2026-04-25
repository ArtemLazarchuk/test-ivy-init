using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TestIvyInit.Connections.TestIvyInit.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "github_stargazers",
                columns: table => new
                {
                    repo_name = table.Column<string>(type: "text", nullable: false),
                    user_login = table.Column<string>(type: "text", nullable: false),
                    unstarred_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    starred_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("github_stargazers_pkey", x => new { x.repo_name, x.user_login });
                });

            migrationBuilder.CreateTable(
                name: "github_stargazers_daily",
                columns: table => new
                {
                    repo_name = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    new_count = table.Column<int>(type: "integer", nullable: true),
                    unstar_count = table.Column<int>(type: "integer", nullable: true),
                    reactivated_count = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("github_stargazers_daily_pkey", x => new { x.repo_name, x.date });
                });

            migrationBuilder.CreateTable(
                name: "github_stars_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    repo_name = table.Column<string>(type: "text", nullable: true),
                    stars = table.Column<long>(type: "bigint", nullable: true),
                    date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_DATE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("github_stars_history_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ivy_ask_questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Widget = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''::character varying"),
                    Difficulty = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    QuestionText = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValueSql: "'manual'::character varying"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ivy_ask_questions_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ivy_ask_test_runs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    IvyVersion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "''::character varying"),
                    Environment = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "'production'::character varying"),
                    TotalQuestions = table.Column<int>(type: "integer", nullable: false),
                    SuccessCount = table.Column<int>(type: "integer", nullable: false),
                    NoAnswerCount = table.Column<int>(type: "integer", nullable: false),
                    ErrorCount = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DifficultyFilter = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValueSql: "'all'::character varying"),
                    Concurrency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValueSql: "''::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ivy_ask_test_runs_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "nuget_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    package_name = table.Column<string>(type: "text", nullable: true),
                    downloads = table.Column<long>(type: "bigint", nullable: true),
                    date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_DATE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("nuget_history_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ivy_ask_test_results",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    TestRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponseText = table.Column<string>(type: "text", nullable: false, defaultValueSql: "''::text"),
                    ResponseTimeMs = table.Column<int>(type: "integer", nullable: false),
                    IsSuccess = table.Column<bool>(type: "boolean", nullable: false),
                    HttpStatus = table.Column<int>(type: "integer", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ivy_ask_test_results_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "ivy_ask_test_results_QuestionId_fkey",
                        column: x => x.QuestionId,
                        principalTable: "ivy_ask_questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ivy_ask_test_results_TestRunId_fkey",
                        column: x => x.TestRunId,
                        principalTable: "ivy_ask_test_runs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "github_stars_history_repo_name_date_key",
                table: "github_stars_history",
                columns: new[] { "repo_name", "date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_test_results_question_id",
                table: "ivy_ask_test_results",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "ix_test_results_run_id",
                table: "ivy_ask_test_results",
                column: "TestRunId");

            migrationBuilder.CreateIndex(
                name: "ix_test_runs_ivy_version",
                table: "ivy_ask_test_runs",
                column: "IvyVersion");

            migrationBuilder.CreateIndex(
                name: "nuget_history_package_name_date_key",
                table: "nuget_history",
                columns: new[] { "package_name", "date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "github_stargazers");

            migrationBuilder.DropTable(
                name: "github_stargazers_daily");

            migrationBuilder.DropTable(
                name: "github_stars_history");

            migrationBuilder.DropTable(
                name: "ivy_ask_test_results");

            migrationBuilder.DropTable(
                name: "nuget_history");

            migrationBuilder.DropTable(
                name: "ivy_ask_questions");

            migrationBuilder.DropTable(
                name: "ivy_ask_test_runs");
        }
    }
}
