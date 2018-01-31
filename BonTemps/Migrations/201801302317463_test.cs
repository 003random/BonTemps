namespace BonTemps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Menus", "Allergy_Id", "dbo.Allergies");
            DropIndex("dbo.Menus", new[] { "Allergy_Id" });
            DropColumn("dbo.Menus", "Allergy_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Menus", "Allergy_Id", c => c.Int());
            CreateIndex("dbo.Menus", "Allergy_Id");
            AddForeignKey("dbo.Menus", "Allergy_Id", "dbo.Allergies", "Id");
        }
    }
}
