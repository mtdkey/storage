﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MtdKey.Storage.Context.MSSQL.Migrations
{
    public partial class InitData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bunch",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    archive_flag = table.Column<byte>(type: "tinyint", nullable: false),
                    deleted_flag = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bunch", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "register",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    vlue = table.Column<string>(type: "nvarchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_register", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bunch_ext",
                columns: table => new
                {
                    bunch_id = table.Column<long>(type: "bigint", nullable: false),
                    counter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bunch_ext", x => x.bunch_id);
                    table.ForeignKey(
                        name: "fk_bunch_bunch_ext",
                        column: x => x.bunch_id,
                        principalTable: "bunch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bunch_token",
                columns: table => new
                {
                    bunch_id = table.Column<long>(type: "bigint", nullable: false),
                    token_to_create = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    token_to_edit = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    token_to_delete = table.Column<string>(type: "nvarchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bunch_token", x => x.bunch_id);
                    table.ForeignKey(
                        name: "fk_bunch_token",
                        column: x => x.bunch_id,
                        principalTable: "bunch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "field",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    parent_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    field_type = table.Column<short>(type: "smallint", nullable: false),
                    archive_flag = table.Column<byte>(type: "tinyint", nullable: false),
                    deleted_flag = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_field", x => x.id);
                    table.ForeignKey(
                        name: "fk_field_bunch",
                        column: x => x.parent_id,
                        principalTable: "bunch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "node",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    parent_id = table.Column<long>(type: "bigint", nullable: false),
                    archive_flag = table.Column<byte>(type: "tinyint", nullable: false),
                    deleted_flag = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_node", x => x.id);
                    table.ForeignKey(
                        name: "fk_node_bunch",
                        column: x => x.parent_id,
                        principalTable: "bunch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "node_ext",
                columns: table => new
                {
                    node_id = table.Column<long>(type: "bigint", nullable: false),
                    number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_node_ext", x => x.node_id);
                    table.ForeignKey(
                        name: "fk_node_node_ext",
                        column: x => x.node_id,
                        principalTable: "node",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "node_token",
                columns: table => new
                {
                    node_id = table.Column<long>(type: "bigint", nullable: false),
                    for_rls = table.Column<string>(type: "nvarchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_node_token", x => x.node_id);
                    table.ForeignKey(
                        name: "fk_node_token_for_rls",
                        column: x => x.node_id,
                        principalTable: "node",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stack",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    node_id = table.Column<long>(type: "bigint", nullable: false),
                    field_id = table.Column<long>(type: "bigint", nullable: false),
                    date_created = table.Column<DateTime>(type: "DateTime", nullable: false),
                    creator_info = table.Column<string>(type: "nvarchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stack", x => x.id);
                    table.ForeignKey(
                        name: "fk_stack_field",
                        column: x => x.field_id,
                        principalTable: "field",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_stack_node",
                        column: x => x.node_id,
                        principalTable: "node",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stack_digital",
                columns: table => new
                {
                    stack_id = table.Column<long>(type: "bigint", nullable: false),
                    value = table.Column<decimal>(type: "decimal(20,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stack_digital", x => x.stack_id);
                    table.ForeignKey(
                        name: "fk_stack_digital",
                        column: x => x.stack_id,
                        principalTable: "stack",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stack_list",
                columns: table => new
                {
                    number = table.Column<long>(type: "bigint", nullable: false),
                    node_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stack_list", x => x.number);
                    table.ForeignKey(
                        name: "fk_node_stack_list",
                        column: x => x.node_id,
                        principalTable: "node",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_stack_list",
                        column: x => x.number,
                        principalTable: "stack",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stack_text",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stack_id = table.Column<long>(type: "bigint", nullable: false),
                    value = table.Column<string>(type: "nvarchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stack_text", x => x.id);
                    table.ForeignKey(
                        name: "fk_stack_text",
                        column: x => x.stack_id,
                        principalTable: "stack",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "fk_field_bunch_idx",
                table: "field",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "fk_node_bunch_idx",
                table: "node",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "idx_rls_token",
                table: "node_token",
                column: "for_rls");

            migrationBuilder.CreateIndex(
                name: "fk_stack_field_idx",
                table: "stack",
                column: "field_id");

            migrationBuilder.CreateIndex(
                name: "fk_stack_node_idx",
                table: "stack",
                column: "node_id");

            migrationBuilder.CreateIndex(
                name: "idx_stack_digital_value",
                table: "stack_digital",
                column: "value");

            migrationBuilder.CreateIndex(
                name: "fk_node_stack_list_idx",
                table: "stack_list",
                column: "node_id");

            migrationBuilder.CreateIndex(
                name: "fk_stack_text_stack_idx",
                table: "stack_text",
                column: "stack_id");

            migrationBuilder.CreateIndex(
                name: "idx_stack_text_value",
                table: "stack_text",
                column: "value");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bunch_ext");

            migrationBuilder.DropTable(
                name: "bunch_token");

            migrationBuilder.DropTable(
                name: "node_ext");

            migrationBuilder.DropTable(
                name: "node_token");

            migrationBuilder.DropTable(
                name: "register");

            migrationBuilder.DropTable(
                name: "stack_digital");

            migrationBuilder.DropTable(
                name: "stack_list");

            migrationBuilder.DropTable(
                name: "stack_text");

            migrationBuilder.DropTable(
                name: "stack");

            migrationBuilder.DropTable(
                name: "field");

            migrationBuilder.DropTable(
                name: "node");

            migrationBuilder.DropTable(
                name: "bunch");
        }
    }
}
