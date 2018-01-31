namespace BonTemps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Allergies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Gender = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        Prefix = c.String(),
                        LastName = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false, maxLength: 14),
                        Email = c.String(nullable: false),
                        NewsLetter = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Mail_credentials",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        SMTPServer = c.String(nullable: false),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Port = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Menus_Allergies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Allergie_Id = c.Int(),
                        Menu_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Allergies", t => t.Allergie_Id)
                .ForeignKey("dbo.Menus", t => t.Menu_Id)
                .Index(t => t.Allergie_Id)
                .Index(t => t.Menu_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Menu_Id = c.Int(),
                        Reservation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Menus", t => t.Menu_Id)
                .ForeignKey("dbo.Reservations", t => t.Reservation_Id)
                .Index(t => t.Menu_Id)
                .Index(t => t.Reservation_Id);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Persons = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Reservations_Table_Layout",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reservation_Id = c.Int(),
                        Table_layout_TableLayoutId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Reservations", t => t.Reservation_Id)
                .ForeignKey("dbo.Table_layout", t => t.Table_layout_TableLayoutId)
                .Index(t => t.Reservation_Id)
                .Index(t => t.Table_layout_TableLayoutId);
            
            CreateTable(
                "dbo.Table_layout",
                c => new
                    {
                        TableLayoutId = c.Int(nullable: false, identity: true),
                        LayoutX = c.Int(nullable: false),
                        LayoutY = c.Int(nullable: false),
                        IsTable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TableLayoutId)
                .Index(t => new { t.LayoutX, t.LayoutY }, unique: true, name: "Layouts");
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Reservations_Table_Layout", "Table_layout_TableLayoutId", "dbo.Table_layout");
            DropForeignKey("dbo.Reservations_Table_Layout", "Reservation_Id", "dbo.Reservations");
            DropForeignKey("dbo.Orders", "Reservation_Id", "dbo.Reservations");
            DropForeignKey("dbo.Reservations", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Orders", "Menu_Id", "dbo.Menus");
            DropForeignKey("dbo.Menus_Allergies", "Menu_Id", "dbo.Menus");
            DropForeignKey("dbo.Menus_Allergies", "Allergie_Id", "dbo.Allergies");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Table_layout", "Layouts");
            DropIndex("dbo.Reservations_Table_Layout", new[] { "Table_layout_TableLayoutId" });
            DropIndex("dbo.Reservations_Table_Layout", new[] { "Reservation_Id" });
            DropIndex("dbo.Reservations", new[] { "Customer_Id" });
            DropIndex("dbo.Orders", new[] { "Reservation_Id" });
            DropIndex("dbo.Orders", new[] { "Menu_Id" });
            DropIndex("dbo.Menus_Allergies", new[] { "Menu_Id" });
            DropIndex("dbo.Menus_Allergies", new[] { "Allergie_Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Table_layout");
            DropTable("dbo.Reservations_Table_Layout");
            DropTable("dbo.Reservations");
            DropTable("dbo.Orders");
            DropTable("dbo.Menus_Allergies");
            DropTable("dbo.Menus");
            DropTable("dbo.Mail_credentials");
            DropTable("dbo.Customers");
            DropTable("dbo.Allergies");
        }
    }
}
