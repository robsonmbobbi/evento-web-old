using FluentMigrator;

namespace EventoWeb.BancoDados.Migracoes
{
    [Migration(06)]
    public class Migracao06 : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Delete.Column("TELEFONE_FIXO").FromTable("PESSOAS");
            Delete.Column("TIPOS_CARNE_NAO_COME").FromTable("PESSOAS");
            Delete.Column("MEDICAMENTOS_USADOS").FromTable("PESSOAS");
            Delete.Column("TEMPO_ESPIRITA").FromTable("INSCRICOES");
            Delete.Column("PRIMEIRO_ENCONTRO").FromTable("INSCRICOES");
            Delete.Column("NOME_RESP_CENTRO").FromTable("INSCRICOES");
            Delete.Column("TELEFONE_RESP_CENTRO").FromTable("INSCRICOES");
            Delete.Column("NOME_RESP_LEGAL").FromTable("INSCRICOES");
            Delete.Column("TELEFONE_RESP_LEGAL").FromTable("INSCRICOES");
        }
    }
}
