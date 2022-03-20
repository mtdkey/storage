﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MtdKey.Storage.Context.MySQL;

namespace MtdKey.Storage.Context.MySQL.Migrations
{
    [DbContext(typeof(MySQLContext))]
    partial class MySQLContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("MtdKey.Storage.DataModels.Bunch", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<byte>("ArchiveFlag")
                        .HasColumnType("tinyint(2)")
                        .HasColumnName("archive_flag");

                    b.Property<byte>("DeletedFlag")
                        .HasColumnType("tinyint(2)")
                        .HasColumnName("deleted_flag");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("bunch");
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

                    b.ToTable("bunch_ext");
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

                    b.ToTable("bunch_token");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Field", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<byte>("ArchiveFlag")
                        .HasColumnType("tinyint(2)")
                        .HasColumnName("archive_flag");

                    b.Property<byte>("DeletedFlag")
                        .HasColumnType("tinyint(2)")
                        .HasColumnName("deleted_flag");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("description");

                    b.Property<short>("FieldType")
                        .HasColumnType("smallint")
                        .HasColumnName("field_type");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("name");

                    b.Property<long>("ParentId")
                        .HasColumnType("bigint")
                        .HasColumnName("parent_id");

                    b.HasKey("Id");

                    b.HasIndex("ParentId")
                        .HasDatabaseName("fk_field_bunch_idx");

                    b.ToTable("field");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Node", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<byte>("ArchiveFlag")
                        .HasColumnType("tinyint(2)")
                        .HasColumnName("archive_flag");

                    b.Property<byte>("DeletedFlag")
                        .HasColumnType("tinyint(2)")
                        .HasColumnName("deleted_flag");

                    b.Property<long>("ParentId")
                        .HasColumnType("bigint")
                        .HasColumnName("parent_id");

                    b.HasKey("Id");

                    b.HasIndex("ParentId")
                        .HasDatabaseName("fk_node_bunch_idx");

                    b.ToTable("node");
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

                    b.ToTable("node_ext");
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

                    b.ToTable("node_token");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Register", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("vlue");

                    b.HasKey("Id");

                    b.ToTable("register");
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

                    b.ToTable("stack");
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

                    b.ToTable("stack_digital");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.StackList", b =>
                {
                    b.Property<long>("StackId")
                        .HasColumnType("bigint")
                        .HasColumnName("number");

                    b.Property<long>("NodeId")
                        .HasColumnType("bigint")
                        .HasColumnName("node_id");

                    b.HasKey("StackId");

                    b.HasIndex("NodeId")
                        .HasDatabaseName("fk_node_stack_list_idx");

                    b.ToTable("stack_list");
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

                    b.ToTable("stack_text");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.BunchExt", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Bunch", "Bunch")
                        .WithOne("BunchExt")
                        .HasForeignKey("MtdKey.Storage.DataModels.BunchExt", "BunchId")
                        .HasConstraintName("fk_bunch_bunch_ext")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bunch");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.BunchToken", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Bunch", "Bunch")
                        .WithOne("BunchToken")
                        .HasForeignKey("MtdKey.Storage.DataModels.BunchToken", "BunchId")
                        .HasConstraintName("fk_bunch_token")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bunch");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Field", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Bunch", "Bunch")
                        .WithMany("Fields")
                        .HasForeignKey("ParentId")
                        .HasConstraintName("fk_field_bunch")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bunch");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Node", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Bunch", "Bunch")
                        .WithMany("Nodes")
                        .HasForeignKey("ParentId")
                        .HasConstraintName("fk_node_bunch")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bunch");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.NodeExt", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Node", "Node")
                        .WithOne("NodeExt")
                        .HasForeignKey("MtdKey.Storage.DataModels.NodeExt", "NodeId")
                        .HasConstraintName("fk_node_node_ext")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Node");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.NodeToken", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Node", "Node")
                        .WithOne("NodeToken")
                        .HasForeignKey("MtdKey.Storage.DataModels.NodeToken", "NodeId")
                        .HasConstraintName("fk_node_token_for_rls")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Node");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Stack", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Field", "Field")
                        .WithMany("Stacks")
                        .HasForeignKey("FieldId")
                        .HasConstraintName("fk_stack_field")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MtdKey.Storage.DataModels.Node", "Node")
                        .WithMany("Stacks")
                        .HasForeignKey("NodeId")
                        .HasConstraintName("fk_stack_node")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");

                    b.Navigation("Node");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.StackDigital", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Stack", "Stack")
                        .WithOne("StackDigital")
                        .HasForeignKey("MtdKey.Storage.DataModels.StackDigital", "StackId")
                        .HasConstraintName("fk_stack_digital")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stack");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.StackList", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Node", "Node")
                        .WithMany("StackLists")
                        .HasForeignKey("NodeId")
                        .HasConstraintName("fk_node_stack_list")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MtdKey.Storage.DataModels.Stack", "Stack")
                        .WithOne("StackList")
                        .HasForeignKey("MtdKey.Storage.DataModels.StackList", "StackId")
                        .HasConstraintName("fk_stack_list")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Node");

                    b.Navigation("Stack");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.StackText", b =>
                {
                    b.HasOne("MtdKey.Storage.DataModels.Stack", "Stack")
                        .WithMany("StackTexts")
                        .HasForeignKey("StackId")
                        .HasConstraintName("fk_stack_text")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stack");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Bunch", b =>
                {
                    b.Navigation("BunchExt");

                    b.Navigation("BunchToken");

                    b.Navigation("Fields");

                    b.Navigation("Nodes");
                });

            modelBuilder.Entity("MtdKey.Storage.DataModels.Field", b =>
                {
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

                    b.Navigation("StackList");

                    b.Navigation("StackTexts");
                });
#pragma warning restore 612, 618
        }
    }
}
