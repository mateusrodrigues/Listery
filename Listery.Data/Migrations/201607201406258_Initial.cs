namespace Listery.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Households",
                c => new
                    {
                        HouseholdID = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.HouseholdID);
            
            CreateTable(
                "dbo.GroceryLists",
                c => new
                    {
                        GroceryListID = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        Title = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        HouseholdID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.GroceryListID)
                .ForeignKey("dbo.Households", t => t.HouseholdID, cascadeDelete: true)
                .Index(t => t.HouseholdID);
            
            CreateTable(
                "dbo.GroceryItems",
                c => new
                    {
                        GroceryItemID = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        Content = c.String(),
                        IsDone = c.Boolean(nullable: false),
                        GroceryListID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.GroceryItemID)
                .ForeignKey("dbo.GroceryLists", t => t.GroceryListID, cascadeDelete: true)
                .Index(t => t.GroceryListID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Subject = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        Username = c.String(),
                        Password = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        HouseholdID = c.Guid(nullable: true),
                    })
                .PrimaryKey(t => t.Subject)
                .ForeignKey("dbo.Households", t => t.HouseholdID)
                .Index(t => t.HouseholdID);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        UserClaimID = c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"),
                        Subject = c.Guid(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.UserClaimID)
                .ForeignKey("dbo.Users", t => t.Subject, cascadeDelete: true)
                .Index(t => t.Subject);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "HouseholdID", "dbo.Households");
            DropForeignKey("dbo.UserClaims", "Subject", "dbo.Users");
            DropForeignKey("dbo.GroceryItems", "GroceryListID", "dbo.GroceryLists");
            DropForeignKey("dbo.GroceryLists", "HouseholdID", "dbo.Households");
            DropIndex("dbo.UserClaims", new[] { "Subject" });
            DropIndex("dbo.Users", new[] { "HouseholdID" });
            DropIndex("dbo.GroceryItems", new[] { "GroceryListID" });
            DropIndex("dbo.GroceryLists", new[] { "HouseholdID" });
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.GroceryItems");
            DropTable("dbo.GroceryLists");
            DropTable("dbo.Households");
        }
    }
}
