namespace exemploasp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usernull : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Marcacao", "UserAccountID", "dbo.UserAccount");
            DropIndex("dbo.Marcacao", new[] { "UserAccountID" });
            AlterColumn("dbo.Marcacao", "UserAccountID", c => c.Int());
            CreateIndex("dbo.Marcacao", "UserAccountID");
            AddForeignKey("dbo.Marcacao", "UserAccountID", "dbo.UserAccount", "UserAccountID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Marcacao", "UserAccountID", "dbo.UserAccount");
            DropIndex("dbo.Marcacao", new[] { "UserAccountID" });
            AlterColumn("dbo.Marcacao", "UserAccountID", c => c.Int(nullable: false));
            CreateIndex("dbo.Marcacao", "UserAccountID");
            AddForeignKey("dbo.Marcacao", "UserAccountID", "dbo.UserAccount", "UserAccountID", cascadeDelete: true);
        }
    }
}
