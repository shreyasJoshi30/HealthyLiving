namespace HL_V1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reservationClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        ReservationID = c.Guid(nullable: false),
                        NutritionistID = c.String(nullable: false, maxLength: 128),
                        ReservedDate = c.DateTime(nullable: false),
                        ReservedBy = c.String(maxLength: 128),
                        Reservation_status = c.String(),
                        ModifiedBy = c.String(),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.ReservationID)
                .ForeignKey("dbo.AspNetUsers", t => t.NutritionistID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ReservedBy)
                .Index(t => t.NutritionistID)
                .Index(t => t.ReservedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "ReservedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reservations", "NutritionistID", "dbo.AspNetUsers");
            DropIndex("dbo.Reservations", new[] { "ReservedBy" });
            DropIndex("dbo.Reservations", new[] { "NutritionistID" });
            DropTable("dbo.Reservations");
        }
    }
}
