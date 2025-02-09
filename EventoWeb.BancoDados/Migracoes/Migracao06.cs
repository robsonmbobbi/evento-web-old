using FluentMigrator;
using System;
using System.Data;

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

            Create
                .Table("CONFIGURACOES_WHATSAPP")
                    .WithColumn("ID").AsInt32().PrimaryKey().Identity()
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_EVENTO_CNFWP", "EVENTOS", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("INSTANCIA").AsString(400).NotNullable()
                    .WithColumn("HOST_API").AsString(400).NotNullable()
                    .WithColumn("CHAVE_API").AsString(400).NotNullable();

            Create
                .Table("MENSAGENS_WHATSAPP_PADRAO")
                    .WithColumn("ID").AsInt32().PrimaryKey().Identity()
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_MSGWPP_EVENTO", "EVENTOS", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                     .WithColumn("ASSUNTO_INSC_CONFIRMADA").AsString(150).Nullable()
                    .WithColumn("MENSAGEM_INSC_CONFIRMADA").AsString(Int32.MaxValue).Nullable()
                    .WithColumn("ASSUNTO_INSC_REGISTRADA").AsString(150).Nullable()
                    .WithColumn("MENSAGEM_INSC_REGISTRADA").AsString(Int32.MaxValue).Nullable()
                    .WithColumn("ASSUNTO_INSC_COD_ACESSO_ACOMP").AsString(150).Nullable()
                    .WithColumn("MENSAGEM_INSC_COD_ACESSO_ACOMP").AsString(Int32.MaxValue).Nullable()
                    .WithColumn("ASSUNTO_INSC_COD_ACESSO_CRI").AsString(150).Nullable()
                    .WithColumn("MENSAGEM_INSC_COD_ACESSO_CRI").AsString(Int32.MaxValue).Nullable();

        }
    }
}
