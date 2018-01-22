namespace exemploasp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MergeProblem : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Exposicao", "Duracao", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Exposicao", "Duracao", c => c.Int(nullable: false));
        }
    }
}
