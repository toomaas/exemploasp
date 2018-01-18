namespace exemploasp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ligacoes_tabelas : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserAccountTema", newName: "TemaUserAccount");
            DropPrimaryKey("dbo.TemaUserAccount");
            CreateTable(
                "dbo.Disponibilidade",
                c => new
                    {
                        DisponibilidadeID = c.Int(nullable: false, identity: true),
                        DataDisponibilidade = c.DateTime(nullable: false),
                        UserAccount_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.DisponibilidadeID)
                .ForeignKey("dbo.UserAccount", t => t.UserAccount_UserID)
                .Index(t => t.UserAccount_UserID);
            
            CreateTable(
                "dbo.UserAccountExposicao",
                c => new
                    {
                        UserAccount_UserID = c.Int(nullable: false),
                        Exposicao_ExposicaoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserAccount_UserID, t.Exposicao_ExposicaoID })
                .ForeignKey("dbo.UserAccount", t => t.UserAccount_UserID, cascadeDelete: true)
                .ForeignKey("dbo.Exposicao", t => t.Exposicao_ExposicaoID, cascadeDelete: true)
                .Index(t => t.UserAccount_UserID)
                .Index(t => t.Exposicao_ExposicaoID);
            
            AddColumn("dbo.Marcacao", "Exposicao_ExposicaoID", c => c.Int());
            AddColumn("dbo.Marcacao", "UserAccount_UserID", c => c.Int());
            AddPrimaryKey("dbo.TemaUserAccount", new[] { "Tema_TemaID", "UserAccount_UserID" });
            CreateIndex("dbo.Marcacao", "Exposicao_ExposicaoID");
            CreateIndex("dbo.Marcacao", "UserAccount_UserID");
            AddForeignKey("dbo.Marcacao", "Exposicao_ExposicaoID", "dbo.Exposicao", "ExposicaoID");
            AddForeignKey("dbo.Marcacao", "UserAccount_UserID", "dbo.UserAccount", "UserID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Marcacao", "UserAccount_UserID", "dbo.UserAccount");
            DropForeignKey("dbo.UserAccountExposicao", "Exposicao_ExposicaoID", "dbo.Exposicao");
            DropForeignKey("dbo.UserAccountExposicao", "UserAccount_UserID", "dbo.UserAccount");
            DropForeignKey("dbo.Disponibilidade", "UserAccount_UserID", "dbo.UserAccount");
            DropForeignKey("dbo.Marcacao", "Exposicao_ExposicaoID", "dbo.Exposicao");
            DropIndex("dbo.UserAccountExposicao", new[] { "Exposicao_ExposicaoID" });
            DropIndex("dbo.UserAccountExposicao", new[] { "UserAccount_UserID" });
            DropIndex("dbo.Disponibilidade", new[] { "UserAccount_UserID" });
            DropIndex("dbo.Marcacao", new[] { "UserAccount_UserID" });
            DropIndex("dbo.Marcacao", new[] { "Exposicao_ExposicaoID" });
            DropPrimaryKey("dbo.TemaUserAccount");
            DropColumn("dbo.Marcacao", "UserAccount_UserID");
            DropColumn("dbo.Marcacao", "Exposicao_ExposicaoID");
            DropTable("dbo.UserAccountExposicao");
            DropTable("dbo.Disponibilidade");
            AddPrimaryKey("dbo.TemaUserAccount", new[] { "UserAccount_UserID", "Tema_TemaID" });
            RenameTable(name: "dbo.TemaUserAccount", newName: "UserAccountTema");
        }
    }
}
