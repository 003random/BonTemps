namespace BonTemps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Allergy_Id", c => c.Int());
            CreateIndex("dbo.Menus", "Allergy_Id");
            AddForeignKey("dbo.Menus", "Allergy_Id", "dbo.Allergies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Menus", "Allergy_Id", "dbo.Allergies");
            DropIndex("dbo.Menus", new[] { "Allergy_Id" });
            DropColumn("dbo.Menus", "Allergy_Id");
        }
    }
}
