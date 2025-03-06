using FluentMigrator;

namespace EventoWeb.BancoDados.Migracoes
{
    [Migration(05)]
    public class Migracao05 : Migration
    {
        public override void Down()
        {
            Delete
                .Table("usuarios");
        }

        public override void Up()
        {
            Create.Table("usuarios")
                .WithColumn("LOGIN").AsString(150).PrimaryKey()
                .WithColumn("NOME").AsString(100).NotNullable()
                .WithColumn("SENHA").AsString(32).NotNullable()
                .WithColumn("EH_ADMINISTRADOR").AsInt16().NotNullable();
            
            Insert.IntoTable("usuarios").Row(
                new 
                { 
                    LOGIN = "admin",
                    NOME = "Administrador",
                    SENHA = "b92dacdf9555522caaf9d1900d1b546d", // md5 = admin_492!@P
                    EH_ADMINISTRADOR = 1
                });
        }        
    }
}
