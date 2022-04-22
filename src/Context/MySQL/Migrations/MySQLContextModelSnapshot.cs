﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MtdKey.Storage.Context.MySQL;

#nullable disable

namespace MtdKey.Storage.Context.MySQL.Migrations
{
    [DbContext(typeof(MySQLContext))]
    partial class MySQLContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MtdKey.Storage.DataModels.Bunch", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<sbyte>("DeletedFlag")
                        .HasColumnType("tinyint(2)")
                        .HasColumnName("deleted_flag");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("idx_bunch_name");

                    b.ToTable("bunch", (string)null);
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.BunchExt", b =>
                {
                    b.Property<long>("BunchId")
                        .HasColumnType("bigint")
                        .HasColumnName("bunch_id");

                    b.Property<int>("Counter")
                        .HasColumnType("int(10)")
                        .HasColumnName("counter");

                    b.HasKey("BunchId");

                    b.ToTable("bunch_ext", (string)null);
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.BunchToken", b =>
                {
                    b.Property<long>("BunchId")
                        .HasColumnType("bigint")
                        .HasColumnName("bunch_id");

                    b.Property<string>("TokenToCreate")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("token_to_create");

                    b.Property<string>("TokenToEdit")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("token_to_edit");

                    b.Property<string>("TokenoDelete")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("token_to_delete");

                    b.HasKey("BunchId");

                    b.ToTable("bunch_token", (string)null);
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Field", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<long>("BunchId")
                        .HasColumnType("bigint")
                        .HasColumnName("bunch_id");

                    b.Property<sbyte>("DeletedFlag")
                        .HasColumnType("tinyint(2)")
                        .HasColumnName("deleted_flag");

                    b.Property<short>("FieldType")
                        .HasColumnType("smallint")
                        .HasColumnName("field_type");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("BunchId")
                        .HasDatabaseName("fk_field_bunch_idx");

                    b.HasIndex("Name", "BunchId")
                        .IsUnique()
                        .HasDatabaseName("idx_field_name");

                    b.ToTable("field", (string)null);
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.FieldLink", b =>
                {
                    b.Property<long>("FieldId")
                        .HasColumnType("bigint")
                        .HasColumnName("field_id");

                    b.Property<long>("BunchId")
                        .HasColumnType("bigint")
                        .HasColumnName("bunch_id");

                    b.HasKey("FieldId");

                    b.HasIndex("BunchId")
                        .HasDatabaseName("fk_field_link_bunch_idx");

                    b.HasIndex("FieldId")
                        .HasDatabaseName("fk_field_link_field_idx");

                    b.ToTable("field_link", (string)null);
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Node", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<long>("BunchId")
                        .HasColumnType("bigint")
                        .HasColumnName("bunch_id");

                    b.Property<string>("CreatorInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("creator_info");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("DateTime")
                        .HasColumnName("date_created");

                    b.Property<sbyte>("DeletedFlag")
                        .HasColumnType("tinyint(2)")
                        .HasColumnName("deleted_flag");

                    b.HasKey("Id");

                    b.HasIndex("BunchId")
                        .HasDatabaseName("fk_node_bunch_idx");

                    b.ToTable("node", (string)null);
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.NodeExt", b =>
                {
                    b.Property<long>("NodeId")
                        .HasColumnType("bigint")
                        .HasColumnName("node_id");

                    b.Property<int>("Number")
                        .HasColumnType("int(10)")
                        .HasColumnName("number");

                    b.HasKey("NodeId");

                    b.ToTable("node_ext", (string)null);
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.NodeToken", b =>
                {
                    b.Property<long>("NodeId")
                        .HasColumnType("bigint")
                        .HasColumnName("node_id");

                    b.Property<string>("ForRLS")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("for_rls");

                    b.HasKey("NodeId");

                    b.HasIndex("ForRLS")
                        .HasDatabaseName("idx_rls_token");

                    b.ToTable("node_token", (string)null);
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Stack", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("CreatorInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("creator_info");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("DateTime")
                        .HasColumnName("date_created");

                    b.Property<long>("FieldId")
                        .HasColumnType("bigint")
                        .HasColumnName("field_id");

                    b.Property<long>("NodeId")
                        .HasColumnType("bigint")
                        .HasColumnName("node_id");

                    b.HasKey("Id");

                    b.HasIndex("FieldId")
                        .HasDatabaseName("fk_stack_field_idx");

                    b.HasIndex("NodeId")
                        .HasDatabaseName("fk_stack_node_idx");

                    b.ToTable("stack", (string)null);
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.StackDigital", b =>
                {
                    b.Property<long>("StackId")
                        .HasColumnType("bigint")
                        .HasColumnName("stack_id");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(20,2)")
                        .HasColumnName("value");

                    b.HasKey("StackId");

                    b.HasIndex("Value")
                        .HasDatabaseName("idx_stack_digital_value");

                    b.ToTable("stack_digital", (string)null);
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.StackFile", b =>
                {
                    b.Property<long>("StackId")
                        .HasColumnType("bigint")
                        .HasColumnName("stack_id");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("longblob")
                        .HasColumnName("data");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("file_name");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint")
                        .HasColumnName("file_size");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("file_type");

                    b.HasKey("StackId");

                    b.ToTable("stack_file", (string)null);
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.StackList", b =>
                {
                    b.Property<long>("StackId")
                        .HasColumnType("bigint")
                        .HasColumnName("stack_id");

                    b.Property<long>("NodeId")
                        .HasColumnType("bigint")
                        .HasColumnName("node_id");

                    b.HasKey("StackId");

                    b.HasIndex("NodeId")
                        .HasDatabaseName("fk_node_stack_list_idx");

                    b.ToTable("stack_list", (string)null);
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.StackText", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<long>("StackId")
                        .HasColumnType("bigint")
                        .HasColumnName("stack_id");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex("StackId")
                        .HasDatabaseName("fk_stack_text_stack_idx");

                    b.HasIndex("Value")
                        .HasDatabaseName("idx_stack_text_value");

                    b.ToTable("stack_text", (string)null);
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.BunchExt", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Bunch", "Bunch")
                        .WithOne("BunchExt")
                        .HasForeignKey("MtdKey.Storage.DataModels.BunchExt", "BunchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bunch_bunch_ext");

                    b.Navigation("Bunch");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.BunchToken", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Bunch", "Bunch")
                        .WithOne("BunchToken")
                        .HasForeignKey("MtdKey.Storage.DataModels.BunchToken", "BunchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bunch_token");

                    b.Navigation("Bunch");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Field", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Bunch", "Bunch")
                        .WithMany("Fields")
                        .HasForeignKey("BunchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_field_bunch");

                    b.Navigation("Bunch");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.FieldLink", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Bunch", "Bunch")
                        .WithMany("FieldLinks")
                        .HasForeignKey("BunchId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("fk_field_link_bunch");

                    b.HasOne("MtdKey.Storage.DataModels.Field", "Field")
                        .WithOne("FieldLink")
                        .HasForeignKey("MtdKey.Storage.DataModels.FieldLink", "FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_field_link_field");

                    b.Navigation("Bunch");

                    b.Navigation("Field");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Node", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Bunch", "Bunch")
                        .WithMany("Nodes")
                        .HasForeignKey("BunchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_node_bunch");

                    b.Navigation("Bunch");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.NodeExt", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Node", "Node")
                        .WithOne("NodeExt")
                        .HasForeignKey("MtdKey.Storage.DataModels.NodeExt", "NodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_node_node_ext");

                    b.Navigation("Node");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.NodeToken", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Node", "Node")
                        .WithOne("NodeToken")
                        .HasForeignKey("MtdKey.Storage.DataModels.NodeToken", "NodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_node_token_for_rls");

                    b.Navigation("Node");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Stack", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Field", "Field")
                        .WithMany("Stacks")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("fk_stack_field");

                    b.HasOne("MtdKey.Storage.DataModels.Node", "Node")
                        .WithMany("Stacks")
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_stack_node");

                    b.Navigation("Field");

                    b.Navigation("Node");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.StackDigital", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Stack", "Stack")
                        .WithOne("StackDigital")
                        .HasForeignKey("MtdKey.Storage.DataModels.StackDigital", "StackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_stack_digital");

                    b.Navigation("Stack");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.StackFile", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Stack", "Stack")
                        .WithOne("StackFile")
                        .HasForeignKey("MtdKey.Storage.DataModels.StackFile", "StackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_stack_file");

                    b.Navigation("Stack");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.StackList", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Node", "Node")
                        .WithMany("StackLists")
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("fk_node_stack_list");

                    b.HasOne("MtdKey.Storage.DataModels.Stack", "Stack")
                        .WithOne("StackList")
                        .HasForeignKey("MtdKey.Storage.DataModels.StackList", "StackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_stack_list");

                    b.Navigation("Node");

                    b.Navigation("Stack");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.StackText", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Stack", "Stack")
                        .WithMany("StackTexts")
                        .HasForeignKey("StackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_stack_text");

                    b.Navigation("Stack");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Bunch", b =>
                {
                    b.Navigation("BunchExt");

                    b.Navigation("BunchToken");

                    b.Navigation("FieldLinks");

                    b.Navigation("Fields");

                    b.Navigation("Nodes");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Field", b =>
                {
                    b.Navigation("FieldLink");

                    b.Navigation("Stacks");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Node", b =>
                {
                    b.Navigation("NodeExt");

                    b.Navigation("NodeToken");

                    b.Navigation("StackLists");

                    b.Navigation("Stacks");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Stack", b =>
                {
                    b.Navigation("StackDigital");

                    b.Navigation("StackFile");

                    b.Navigation("StackList");

                    b.Navigation("StackTexts");
                });
#pragma warning restore 612, 618
        }
    }
}
