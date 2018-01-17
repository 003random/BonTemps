namespace BonTemps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcreatedateprop : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Allergies", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customers", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Menus", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Reservations", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "DateCreated");
            DropColumn("dbo.Orders", "DateCreated");
            DropColumn("dbo.Menus", "DateCreated");
            DropColumn("dbo.Customers", "DateCreated");
            DropColumn("dbo.Allergies", "DateCreated");
        }
    }
}
