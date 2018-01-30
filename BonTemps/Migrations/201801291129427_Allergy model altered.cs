namespace BonTemps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Allergymodelaltered : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Allergies", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Allergies", "Image");
        }
    }
}
