using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Player.Database;

namespace Player.Migrations
{
    [DbContext(typeof(MusicCollectionContext))]
    partial class MusicCollectionContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("Player.Database.Album", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("bandid");

                    b.Property<string>("name");

                    b.Property<uint>("year");

                    b.HasKey("id");
                });

            modelBuilder.Entity("Player.Database.Band", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("name");

                    b.HasKey("id");
                });

            modelBuilder.Entity("Player.Database.DeviceInfo", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("name");

                    b.HasKey("id");
                });

            modelBuilder.Entity("Player.Database.Track", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("albumid");

                    b.Property<string>("alias");

                    b.Property<string>("fileName");

                    b.Property<string>("name");

                    b.Property<string>("path");

                    b.HasKey("id");
                });

            modelBuilder.Entity("Player.Database.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("email");

                    b.Property<string>("password");

                    b.HasKey("id");
                });

            modelBuilder.Entity("Player.Database.Album", b =>
                {
                    b.HasOne("Player.Database.Band")
                        .WithMany()
                        .HasForeignKey("bandid");
                });

            modelBuilder.Entity("Player.Database.Track", b =>
                {
                    b.HasOne("Player.Database.Album")
                        .WithMany()
                        .HasForeignKey("albumid");
                });
        }
    }
}
