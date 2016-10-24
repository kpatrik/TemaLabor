namespace AlberletKereso.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alberlets",
                c => new
                    {
                        AlberletId = c.Int(nullable: false, identity: true),
                        Cim = c.String(nullable: false),
                        Szobak_szama = c.Int(nullable: false),
                        Emelet = c.Int(nullable: false),
                        Mosdok_szama = c.Int(nullable: false),
                        Alapterulet = c.Int(nullable: false),
                        Ar = c.Int(nullable: false),
                        Berendezett = c.Boolean(nullable: false),
                        Hirdeto_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AlberletId)
                .ForeignKey("dbo.AspNetUsers", t => t.Hirdeto_Id)
                .Index(t => t.Hirdeto_Id);
            
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
                "dbo.Filters",
                c => new
                    {
                        FilterId = c.Int(nullable: false, identity: true),
                        Cim = c.String(),
                        Szobak_szama = c.Int(),
                        Emelet = c.Int(),
                        Mosdok_szama = c.Int(),
                        Alapterulet = c.Int(),
                        MinAr = c.Int(),
                        MaxAr = c.Int(),
                        Berendezett = c.Boolean(),
                        feliratkozo_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FilterId)
                .ForeignKey("dbo.AspNetUsers", t => t.feliratkozo_Id)
                .Index(t => t.feliratkozo_Id);
            
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
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Keps",
                c => new
                    {
                        KepId = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false),
                        FileName = c.String(),
                        Alberlet_AlberletId = c.Int(),
                    })
                .PrimaryKey(t => t.KepId)
                .ForeignKey("dbo.Alberlets", t => t.Alberlet_AlberletId)
                .Index(t => t.Alberlet_AlberletId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Keps", "Alberlet_AlberletId", "dbo.Alberlets");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Alberlets", "Hirdeto_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Filters", "feliratkozo_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Keps", new[] { "Alberlet_AlberletId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Filters", new[] { "feliratkozo_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Alberlets", new[] { "Hirdeto_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Keps");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Filters");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Alberlets");
        }
    }
}
