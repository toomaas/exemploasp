namespace exemploasp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tabelas_disp : DbMigration
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
                        Duracao = c.DateTime(nullable: false),
                        NrItens = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExposicaoID);
            
            CreateTable(
                "dbo.Disponibilidade",
                c => new
                    {
                        DisponibilidadeID = c.Int(nullable: false, identity: true),
                        DataDisponibilidade = c.DateTime(nullable: false),
                        ExposicaoID = c.Int(nullable: false),
                        UserAccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DisponibilidadeID)
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
                        Idade = c.DateTime(nullable: false),
                        Sexo = c.String(nullable: false),
                        NumTelefone = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        ConfirmPassword = c.String(),
                        TipoUtilizadorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserAccountID)
                .ForeignKey("dbo.TipoUtilizador", t => t.TipoUtilizadorID, cascadeDelete: true)
                .Index(t => t.TipoUtilizadorID);
            
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
                        UserAccountID = c.Int(),
                    })
                .PrimaryKey(t => t.MarcacaoID)
                .ForeignKey("dbo.Exposicao", t => t.ExposicaoID, cascadeDelete: true)
                .ForeignKey("dbo.UserAccount", t => t.UserAccountID)
                .Index(t => t.ExposicaoID)
                .Index(t => t.UserAccountID);
            
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
                "dbo.TipoUtilizador",
                c => new
                    {
                        TipoUtilizadorID = c.Int(nullable: false, identity: true),
                        Tipo = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TipoUtilizadorID);
            
            CreateTable(
                "dbo.UserAccountExposicao",
                c => new
                    {
                        UserAccountID = c.Int(nullable: false),
                        ExposicaoID = c.Int(nullable: false),
                        Assigned = c.Int(nullable: false),
                        InformacaoExtra = c.String(),
                    })
                .PrimaryKey(t => new { t.UserAccountID, t.ExposicaoID })
                .ForeignKey("dbo.Exposicao", t => t.ExposicaoID, cascadeDelete: true)
                .ForeignKey("dbo.UserAccount", t => t.UserAccountID, cascadeDelete: true)
                .Index(t => t.UserAccountID)
                .Index(t => t.ExposicaoID);
            
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
            DropForeignKey("dbo.UserAccountExposicao", "UserAccountID", "dbo.UserAccount");
            DropForeignKey("dbo.UserAccountExposicao", "ExposicaoID", "dbo.Exposicao");
            DropForeignKey("dbo.UserAccount", "TipoUtilizadorID", "dbo.TipoUtilizador");
            DropForeignKey("dbo.TemaUserAccount", "UserAccount_UserAccountID", "dbo.UserAccount");
            DropForeignKey("dbo.TemaUserAccount", "Tema_TemaID", "dbo.Tema");
            DropForeignKey("dbo.TemaExposicao", "Exposicao_ExposicaoID", "dbo.Exposicao");
            DropForeignKey("dbo.TemaExposicao", "Tema_TemaID", "dbo.Tema");
            DropForeignKey("dbo.Marcacao", "UserAccountID", "dbo.UserAccount");
            DropForeignKey("dbo.Marcacao", "ExposicaoID", "dbo.Exposicao");
            DropForeignKey("dbo.Disponibilidade", "UserAccountID", "dbo.UserAccount");
            DropForeignKey("dbo.Disponibilidade", "ExposicaoID", "dbo.Exposicao");
            DropIndex("dbo.TemaUserAccount", new[] { "UserAccount_UserAccountID" });
            DropIndex("dbo.TemaUserAccount", new[] { "Tema_TemaID" });
            DropIndex("dbo.TemaExposicao", new[] { "Exposicao_ExposicaoID" });
            DropIndex("dbo.TemaExposicao", new[] { "Tema_TemaID" });
            DropIndex("dbo.UserAccountExposicao", new[] { "ExposicaoID" });
            DropIndex("dbo.UserAccountExposicao", new[] { "UserAccountID" });
            DropIndex("dbo.Marcacao", new[] { "UserAccountID" });
            DropIndex("dbo.Marcacao", new[] { "ExposicaoID" });
            DropIndex("dbo.UserAccount", new[] { "TipoUtilizadorID" });
            DropIndex("dbo.Disponibilidade", new[] { "UserAccountID" });
            DropIndex("dbo.Disponibilidade", new[] { "ExposicaoID" });
            DropTable("dbo.TemaUserAccount");
            DropTable("dbo.TemaExposicao");
            DropTable("dbo.UserAccountExposicao");
            DropTable("dbo.TipoUtilizador");
            DropTable("dbo.Tema");
            DropTable("dbo.Marcacao");
            DropTable("dbo.UserAccount");
            DropTable("dbo.Disponibilidade");
            DropTable("dbo.Exposicao");
        }
    }
}
