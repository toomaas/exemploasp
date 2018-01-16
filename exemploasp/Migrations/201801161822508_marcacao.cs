namespace exemploasp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class marcacao : DbMigration
    {
        public override void Up()
        {
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
                    })
                .PrimaryKey(t => t.MarcacaoID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Marcacao");
        }
    }
}
