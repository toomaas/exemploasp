namespace exemploasp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idadeuserdata : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserAccount", "Idade", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserAccount", "Idade", c => c.Int(nullable: false));
        }
    }
}
