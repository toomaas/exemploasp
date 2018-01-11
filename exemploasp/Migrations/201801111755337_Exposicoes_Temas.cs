namespace exemploasp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Exposicoes_Temas : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserAccounts", newName: "UserAccount");
            CreateTable(
                "dbo.Exposicao",
                c => new
                    {
                        ExposicaoID = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        DataInicial = c.DateTime(nullable: false),
                        DataFinal = c.DateTime(nullable: false),
                        Duracao = c.Int(nullable: false),
                        NrItens = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExposicaoID);
            
            CreateTable(
                "dbo.Tema",
                c => new
                    {
                        TemaID = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Descricao = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TemaID);
            
            CreateTable(
                "dbo.TemaExposicao",
                c => new
                    {
                        Tema_TemaID = c.Int(nullable: false),
                        Exposicao_ExposicaoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tema_TemaID, t.Exposicao_ExposicaoID })
                .ForeignKey("dbo.Tema", t => t.Tema_TemaID, cascadeDelete: true)
                .ForeignKey("dbo.Exposicao", t => t.Exposicao_ExposicaoID, cascadeDelete: true)
                .Index(t => t.Tema_TemaID)
                .Index(t => t.Exposicao_ExposicaoID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TemaExposicao", "Exposicao_ExposicaoID", "dbo.Exposicao");
            DropForeignKey("dbo.TemaExposicao", "Tema_TemaID", "dbo.Tema");
            DropIndex("dbo.TemaExposicao", new[] { "Exposicao_ExposicaoID" });
            DropIndex("dbo.TemaExposicao", new[] { "Tema_TemaID" });
            DropTable("dbo.TemaExposicao");
            DropTable("dbo.Tema");
            DropTable("dbo.Exposicao");
            RenameTable(name: "dbo.UserAccount", newName: "UserAccounts");
        }
    }
}
