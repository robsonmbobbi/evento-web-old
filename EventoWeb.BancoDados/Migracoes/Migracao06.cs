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
            Delete.Column("TELEFONE_FIXO").FromTable("pessoas");
            Delete.Column("TIPOS_CARNE_NAO_COME").FromTable("pessoas");
            Delete.Column("MEDICAMENTOS_USADOS").FromTable("pessoas");
            Delete.Column("TEMPO_ESPIRITA").FromTable("inscricoes");
            Delete.Column("PRIMEIRO_ENCONTRO").FromTable("inscricoes");
            Delete.Column("NOME_RESP_CENTRO").FromTable("inscricoes");
            Delete.Column("TELEFONE_RESP_CENTRO").FromTable("inscricoes");
            Delete.Column("NOME_RESP_LEGAL").FromTable("inscricoes");
            Delete.Column("TELEFONE_RESP_LEGAL").FromTable("inscricoes");

            Create
                .Table("configuracoes_whatsapp")
                    .WithColumn("ID").AsInt32().PrimaryKey().Identity()
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_EVENTO_CNFWP", "eventos", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("INSTANCIA").AsString(400).NotNullable()
                    .WithColumn("HOST_API").AsString(400).NotNullable()
                    .WithColumn("CHAVE_API").AsString(400).NotNullable();

            Create
                .Table("mensagens_whatsapp_padrao")
                    .WithColumn("ID").AsInt32().PrimaryKey().Identity()
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_MSGWPP_EVENTO", "eventos", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
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
