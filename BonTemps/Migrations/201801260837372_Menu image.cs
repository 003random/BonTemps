namespace BonTemps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Menuimage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Menus", "Image");
        }
    }
}
