﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityModel.Migrations
{
    public partial class UpdateFrequency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpdateFrequency",
                table: "Organizations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateFrequency",
                table: "Organizations");
        }
    }
}
