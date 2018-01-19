namespace exemploasp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tabelas : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Marcacao",
                c => new
                    {
                        MarcacaoID = c.Int(nullable: false, identity: true),
                        NomeRequerente = c.String(nullable: false),
                        Idade = c.Int(nullable: false),
                        NumTelefoneRequerente = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false),
                        HoraDeInicio = c.DateTime(nullable: false),
                        HoraDeFim = c.DateTime(nullable: false),
                        NumPessoas = c.Int(nullable: false),
                        ExposicaoID = c.Int(nullable: false),
                        UserAccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MarcacaoID)
                .ForeignKey("dbo.Exposicao", t => t.ExposicaoID, cascadeDelete: true)
                .ForeignKey("dbo.UserAccount", t => t.UserAccountID, cascadeDelete: true)
                .Index(t => t.ExposicaoID)
                .Index(t => t.UserAccountID);
            
            CreateTable(
                "dbo.UserAccount",
                c => new
                    {
                        UserAccountID = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Morada = c.String(nullable: false),
                        Idade = c.Int(nullable: false),
                        Sexo = c.String(nullable: false),
                        NumTelefone = c.Int(nullable: false),
                        TipoUtilizador = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        ConfirmPassword = c.String(),
                    })
                .PrimaryKey(t => t.UserAccountID);
            
            CreateTable(
                "dbo.Disponibilidade",
                c => new
                    {
                        DisponibilidadeID = c.Int(nullable: false, identity: true),
                        DataDisponibilidade = c.DateTime(nullable: false),
                        UserAccount_UserAccountID = c.Int(),
                    })
                .PrimaryKey(t => t.DisponibilidadeID)
                .ForeignKey("dbo.UserAccount", t => t.UserAccount_UserAccountID)
                .Index(t => t.UserAccount_UserAccountID);
            
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
                "dbo.UserAccountExposicao",
                c => new
                    {
                        UserAccount_UserAccountID = c.Int(nullable: false),
                        Exposicao_ExposicaoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserAccount_UserAccountID, t.Exposicao_ExposicaoID })
                .ForeignKey("dbo.UserAccount", t => t.UserAccount_UserAccountID, cascadeDelete: true)
                .ForeignKey("dbo.Exposicao", t => t.Exposicao_ExposicaoID, cascadeDelete: true)
                .Index(t => t.UserAccount_UserAccountID)
                .Index(t => t.Exposicao_ExposicaoID);
            
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
            
            CreateTable(
                "dbo.TemaUserAccount",
                c => new
                    {
                        Tema_TemaID = c.Int(nullable: false),
                        UserAccount_UserAccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tema_TemaID, t.UserAccount_UserAccountID })
                .ForeignKey("dbo.Tema", t => t.Tema_TemaID, cascadeDelete: true)
                .ForeignKey("dbo.UserAccount", t => t.UserAccount_UserAccountID, cascadeDelete: true)
                .Index(t => t.Tema_TemaID)
                .Index(t => t.UserAccount_UserAccountID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TemaUserAccount", "UserAccount_UserAccountID", "dbo.UserAccount");
            DropForeignKey("dbo.TemaUserAccount", "Tema_TemaID", "dbo.Tema");
            DropForeignKey("dbo.TemaExposicao", "Exposicao_ExposicaoID", "dbo.Exposicao");
            DropForeignKey("dbo.TemaExposicao", "Tema_TemaID", "dbo.Tema");
            DropForeignKey("dbo.Marcacao", "UserAccountID", "dbo.UserAccount");
            DropForeignKey("dbo.UserAccountExposicao", "Exposicao_ExposicaoID", "dbo.Exposicao");
            DropForeignKey("dbo.UserAccountExposicao", "UserAccount_UserAccountID", "dbo.UserAccount");
            DropForeignKey("dbo.Disponibilidade", "UserAccount_UserAccountID", "dbo.UserAccount");
            DropForeignKey("dbo.Marcacao", "ExposicaoID", "dbo.Exposicao");
            DropIndex("dbo.TemaUserAccount", new[] { "UserAccount_UserAccountID" });
            DropIndex("dbo.TemaUserAccount", new[] { "Tema_TemaID" });
            DropIndex("dbo.TemaExposicao", new[] { "Exposicao_ExposicaoID" });
            DropIndex("dbo.TemaExposicao", new[] { "Tema_TemaID" });
            DropIndex("dbo.UserAccountExposicao", new[] { "Exposicao_ExposicaoID" });
            DropIndex("dbo.UserAccountExposicao", new[] { "UserAccount_UserAccountID" });
            DropIndex("dbo.Disponibilidade", new[] { "UserAccount_UserAccountID" });
            DropIndex("dbo.Marcacao", new[] { "UserAccountID" });
            DropIndex("dbo.Marcacao", new[] { "ExposicaoID" });
            DropTable("dbo.TemaUserAccount");
            DropTable("dbo.TemaExposicao");
            DropTable("dbo.UserAccountExposicao");
            DropTable("dbo.Tema");
            DropTable("dbo.Disponibilidade");
            DropTable("dbo.UserAccount");
            DropTable("dbo.Marcacao");
            DropTable("dbo.Exposicao");
        }
    }
}
