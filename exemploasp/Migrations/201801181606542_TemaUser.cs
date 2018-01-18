namespace exemploasp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemaUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAccountTema",
                c => new
                    {
                        UserAccount_UserID = c.Int(nullable: false),
                        Tema_TemaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserAccount_UserID, t.Tema_TemaID })
                .ForeignKey("dbo.UserAccount", t => t.UserAccount_UserID, cascadeDelete: true)
                .ForeignKey("dbo.Tema", t => t.Tema_TemaID, cascadeDelete: true)
                .Index(t => t.UserAccount_UserID)
                .Index(t => t.Tema_TemaID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAccountTema", "Tema_TemaID", "dbo.Tema");
            DropForeignKey("dbo.UserAccountTema", "UserAccount_UserID", "dbo.UserAccount");
            DropIndex("dbo.UserAccountTema", new[] { "Tema_TemaID" });
            DropIndex("dbo.UserAccountTema", new[] { "UserAccount_UserID" });
            DropTable("dbo.UserAccountTema");
        }
    }
}
