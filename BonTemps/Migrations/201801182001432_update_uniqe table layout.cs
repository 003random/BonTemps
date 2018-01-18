namespace BonTemps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_uniqetablelayout : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Table_layout", new[] { "LayoutX", "LayoutY" }, unique: true, name: "Layouts");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Table_layout", "Layouts");
        }
    }
}
