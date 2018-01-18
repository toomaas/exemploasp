namespace exemploasp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class marcacaoIDs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Marcacao", "Exposicao_ExposicaoID", "dbo.Exposicao");
            DropIndex("dbo.Marcacao", new[] { "Exposicao_ExposicaoID" });
            RenameColumn(table: "dbo.Marcacao", name: "Exposicao_ExposicaoID", newName: "ExposicaoID");
            AddColumn("dbo.Marcacao", "UserAccountID", c => c.Int(nullable: false));
            AlterColumn("dbo.Marcacao", "ExposicaoID", c => c.Int(nullable: false));
            CreateIndex("dbo.Marcacao", "ExposicaoID");
            AddForeignKey("dbo.Marcacao", "ExposicaoID", "dbo.Exposicao", "ExposicaoID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Marcacao", "ExposicaoID", "dbo.Exposicao");
            DropIndex("dbo.Marcacao", new[] { "ExposicaoID" });
            AlterColumn("dbo.Marcacao", "ExposicaoID", c => c.Int());
            DropColumn("dbo.Marcacao", "UserAccountID");
            RenameColumn(table: "dbo.Marcacao", name: "ExposicaoID", newName: "Exposicao_ExposicaoID");
            CreateIndex("dbo.Marcacao", "Exposicao_ExposicaoID");
            AddForeignKey("dbo.Marcacao", "Exposicao_ExposicaoID", "dbo.Exposicao", "ExposicaoID");
        }
    }
}
