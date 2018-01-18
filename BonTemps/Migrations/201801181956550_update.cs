namespace BonTemps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "PhoneNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "PhoneNumber", c => c.String(nullable: false));
        }
    }
}
