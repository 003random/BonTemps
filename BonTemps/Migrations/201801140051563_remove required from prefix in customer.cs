namespace BonTemps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removerequiredfromprefixincustomer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "Prefix", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "Prefix", c => c.String(nullable: false));
        }
    }
}
