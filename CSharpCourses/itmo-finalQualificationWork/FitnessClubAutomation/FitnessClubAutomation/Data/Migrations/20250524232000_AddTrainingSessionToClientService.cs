using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessClubAutomation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainingSessionToClientService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainingSessionId",
                table: "ClientServices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientServices_TrainingSessionId",
                table: "ClientServices",
                column: "TrainingSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientServices_TrainingSessions_TrainingSessionId",
                table: "ClientServices",
                column: "TrainingSessionId",
                principalTable: "TrainingSessions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientServices_TrainingSessions_TrainingSessionId",
                table: "ClientServices");

            migrationBuilder.DropIndex(
                name: "IX_ClientServices_TrainingSessionId",
                table: "ClientServices");

            migrationBuilder.DropColumn(
                name: "TrainingSessionId",
                table: "ClientServices");
        }
    }
}
