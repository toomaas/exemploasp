namespace exemploasp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
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
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserAccounts");
        }
    }
}
