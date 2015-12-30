using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using DpControl.Domain.EFContext;

namespace DpControl.Migrations
{
    [DbContext(typeof(ShadingContext))]
    partial class ShadingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DpControl.Domain.Entities.Alarm", b =>
                {
                    b.Property<int>("AlarmId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlarmMessageId");

                    b.Property<int>("LocationId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken();

                    b.HasKey("AlarmId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "Alarms");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.AlarmMessage", b =>
                {
                    b.Property<int>("AlarmMessageId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ErrorNo");

                    b.Property<string>("Message")
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("AlarmMessageId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "AlarmMessages");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 60);

                    b.Property<string>("CustomerNo")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 60);

                    b.Property<string>("ProjectNo")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken();

                    b.HasKey("CustomerId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "Customers");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken();

                    b.Property<int?>("SceneSceneId");

                    b.HasKey("GroupId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "Groups");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.GroupLocation", b =>
                {
                    b.Property<int>("GroupId");

                    b.Property<int>("LocationId");

                    b.Property<int?>("GroupGroupId");

                    b.Property<int?>("LocationDeviceId");

                    b.HasKey("GroupId", "LocationId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "GroupLocations");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.GroupOperator", b =>
                {
                    b.Property<int>("GroupId");

                    b.Property<int>("OperatorId");

                    b.Property<int?>("GroupGroupId");

                    b.Property<int?>("OperatorOperatorId");

                    b.HasKey("GroupId", "OperatorId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "GroupOperators");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Holiday", b =>
                {
                    b.Property<int>("HolidayId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<int>("Day");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken();

                    b.HasKey("HolidayId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "Holidays");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Location", b =>
                {
                    b.Property<int>("DeviceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Building")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 80);

                    b.Property<string>("CommAddress")
                        .HasAnnotation("MaxLength", 40);

                    b.Property<int>("CommMode");

                    b.Property<int>("CurrentPosition");

                    b.Property<int>("CustomerId");

                    b.Property<string>("DeviceSerialNo")
                        .HasAnnotation("MaxLength", 16);

                    b.Property<int>("DeviceType");

                    b.Property<int>("FavorPositionFirst");

                    b.Property<int>("FavorPositionThird");

                    b.Property<int>("FavorPositionrSecond");

                    b.Property<string>("Floor")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<int>("InstallationNumber");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("Orientation");

                    b.Property<string>("RoomNo")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken();

                    b.HasKey("DeviceId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "DeviceLocations");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Log", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<int>("LocationId");

                    b.Property<int>("LogDescriptionId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken();

                    b.HasKey("LogId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "Logs");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.LogDescription", b =>
                {
                    b.Property<int>("LogDescriptionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<int>("DescriptionNo");

                    b.HasKey("LogDescriptionId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "LogDescription");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Operator", b =>
                {
                    b.Property<int>("OperatorId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<string>("Description");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("NickName");

                    b.Property<string>("Password");

                    b.Property<byte[]>("RowVersion");

                    b.HasKey("OperatorId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.OperatorLocation", b =>
                {
                    b.Property<int>("LocationId");

                    b.Property<int>("OperatorId");

                    b.Property<int?>("LocationDeviceId");

                    b.Property<int?>("OperatorOperatorId");

                    b.HasKey("LocationId", "OperatorId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "OperatorLocations");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Scene", b =>
                {
                    b.Property<int>("SceneId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<bool>("Enable");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken();

                    b.HasKey("SceneId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "Scenes");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.SceneSegment", b =>
                {
                    b.Property<int>("SceneSegmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken();

                    b.Property<int>("SenseId");

                    b.Property<int>("SequenceNo");

                    b.Property<string>("StartTime")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.Property<int>("Volumn");

                    b.HasKey("SceneSegmentId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "SceneSegments");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Alarm", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.AlarmMessage")
                        .WithMany()
                        .HasForeignKey("AlarmMessageId");

                    b.HasOne("DpControl.Domain.Entities.Location")
                        .WithMany()
                        .HasForeignKey("LocationId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Group", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.HasOne("DpControl.Domain.Entities.Scene")
                        .WithMany()
                        .HasForeignKey("SceneSceneId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.GroupLocation", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Group")
                        .WithMany()
                        .HasForeignKey("GroupGroupId");

                    b.HasOne("DpControl.Domain.Entities.Location")
                        .WithMany()
                        .HasForeignKey("LocationDeviceId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.GroupOperator", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Group")
                        .WithMany()
                        .HasForeignKey("GroupGroupId");

                    b.HasOne("DpControl.Domain.Entities.Operator")
                        .WithMany()
                        .HasForeignKey("OperatorOperatorId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Holiday", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Location", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Log", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("DpControl.Domain.Entities.LogDescription")
                        .WithMany()
                        .HasForeignKey("LogDescriptionId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Operator", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.OperatorLocation", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Location")
                        .WithMany()
                        .HasForeignKey("LocationDeviceId");

                    b.HasOne("DpControl.Domain.Entities.Operator")
                        .WithMany()
                        .HasForeignKey("OperatorOperatorId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Scene", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.SceneSegment", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Scene")
                        .WithMany()
                        .HasForeignKey("SenseId");
                });
        }
    }
}