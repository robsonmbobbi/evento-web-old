using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EventoWeb.BancoDados.Migracoes
{
    [Migration(02)]
    public class Migracao02 : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            /*CriarConta();
            CriarFaturamento();
            CriarTitulo();
            CriarTransacao();*/
            CriarSalasEstudoParticipantes();
            CriarOficinasParticipantes();
            CriarQuartos();
            CriarQuartosInscrito();
            CriarIndices();
        }

        private void CriarSalasEstudoParticipantes()
        {
            Create
                .Table("salas_estudo_participantes")
                .WithColumn("ID_SALA_ESTUDO").AsInt32().PrimaryKey().NotNullable()
                    .ForeignKey("FK_SEP_SALA", "salas_estudo", "ID_SALA_ESTUDO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                .WithColumn("ID_INSCRICAO").AsInt32().PrimaryKey().NotNullable()
                    .ForeignKey("FK_SEP_INSC", "inscricoes", "ID_INSCRICAO").OnDelete(Rule.None).OnUpdate(Rule.Cascade);
        }

        private void CriarOficinasParticipantes()
        {
            Create
                .Table("oficinas_participantes")
                .WithColumn("ID_OFICINA").AsInt32().PrimaryKey().NotNullable()
                    .ForeignKey("FK_OP_OFICINA", "oficinas", "ID_OFICINA").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                .WithColumn("ID_INSCRICAO").AsInt32().PrimaryKey().NotNullable()
                    .ForeignKey("FK_OP_INSC", "inscricoes", "ID_INSCRICAO").OnDelete(Rule.None).OnUpdate(Rule.Cascade);
        }

        private void CriarQuartos()
        {
            Create
                .Table("quartos")
                .WithColumn("ID_QUARTO").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("CAPACIDADE").AsInt32().Nullable()
                .WithColumn("EH_FAMILIA").AsBoolean().NotNullable()
                .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                    .ForeignKey("FK_QUARTO_EVENTO", "eventos", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                .WithColumn("NOME").AsString(100).NotNullable()
                .WithColumn("SEXO").AsInt16().NotNullable();

            Create
                .Index("IDX_QUARTO_1").OnTable("quartos")
                    .OnColumn("ID_EVENTO").Ascending()
                    .OnColumn("ID_QUARTO").Ascending();
        }

        private void CriarQuartosInscrito()
        {
            Create
                .Table("quartos_inscritos")
                .WithColumn("ID_QUARTO_INSCRITO").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("EH_COORDENADOR").AsBoolean().Nullable()
                .WithColumn("ID_INSCRICAO").AsInt32().NotNullable()
                    .ForeignKey("FK_QI_INSCRICAO", "inscricoes", "ID_INSCRICAO").OnDelete(Rule.None).OnUpdate(Rule.Cascade)
                .WithColumn("ID_QUARTO").AsInt32().NotNullable()
                    .ForeignKey("FK_QI_QUARTO", "quartos", "ID_QUARTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade);
        }

        private void CriarIndices()
        {
            Create
                .Index("IDX_INSCRICAO_1").OnTable("inscricoes")
                    .OnColumn("ID_EVENTO").Ascending()
                    .OnColumn("ID_INSCRICAO").Ascending();

            Create
                .Index("IDX_INSCRICAO_2").OnTable("inscricoes")
                    .OnColumn("ID_EVENTO").Ascending()
                    .OnColumn("ID_INSCRICAO").Ascending()
                    .OnColumn("SITUACAO").Ascending();

            Create
                .Index("IDX_SL_ESTUDO_1").OnTable("salas_estudo")
                    .OnColumn("ID_EVENTO").Ascending()
                    .OnColumn("ID_SALA_ESTUDO").Ascending();

            Create
                .Index("IDX_OFICINA_1").OnTable("oficinas")
                    .OnColumn("ID_EVENTO").Ascending()
                    .OnColumn("ID_OFICINA").Ascending();

            Create
                .Index("IDX_AP_SARAU_1").OnTable("apresentacoes_sarau")
                    .OnColumn("ID_EVENTO").Ascending()
                    .OnColumn("ID_APRESENTACAO_SARAU").Ascending();
        }

        private void CriarConta()
        {
            Create
                .Table("CONTAS")
                .WithColumn("ID_CONTA").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("DESCRICAO").AsString(100).NotNullable()
                .WithColumn("SALDO_INICIAL").AsCurrency().NotNullable()
                .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                    .ForeignKey("FK_CONTA_EVENTO", "EVENTOS", "ID_EVENTO").OnDelete(Rule.None).OnUpdate(Rule.None);
        }

        private void CriarFaturamento()
        {
            Create
                .Table("FATURAMENTOS")
                .WithColumn("ID_FATURAMENTO").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("TIPO_FATURAMENTO").AsString(30).NotNullable()
                .WithColumn("DESCRICAO").AsString(500).NotNullable()
                .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                    .ForeignKey("FK_FAT_EVENTO", "EVENTOS", "ID_EVENTO").OnDelete(Rule.None).OnUpdate(Rule.None)
                .WithColumn("FATURADO").AsBoolean().NotNullable()
                .WithColumn("MOTIVO_DESCONTO").AsString(500).Nullable()
                .WithColumn("TIPO").AsInt16().NotNullable()
                .WithColumn("VALOR_BRUTO").AsCurrency().NotNullable()
                .WithColumn("VALOR_DESCONTO").AsCurrency().NotNullable()
                .WithColumn("DATA").AsDateTime().NotNullable();

            Create
                .Index("IDX_FAT_1")
                .OnTable("FATURAMENTOS")
                .OnColumn("DESCRICAO").Ascending()
                .OnColumn("DATA").Ascending();

            Create
                .Table("FATURAMENTO_INSCRICOES")
                .WithColumn("ID_FATURAMENTO").AsInt32().PrimaryKey().NotNullable()
                    .ForeignKey("FK_FATINSC_FAT", "FATURAMENTOS", "ID_FATURAMENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                .WithColumn("ID_INSCRICAO").AsInt32().PrimaryKey().NotNullable()
                    .ForeignKey("FK_FATINSC_INSC", "INSCRICOES", "ID_INSCRICAO").OnDelete(Rule.None).OnUpdate(Rule.Cascade);
        }

        private void CriarTitulo()
        {
            Create
                .Table("TITULOS_FINANCEIROS")
                .WithColumn("ID_TITULO_FINANCEIRO").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("DATA_CRIACAO").AsDateTime().NotNullable()
                .WithColumn("DATA_VENCIMENTO").AsDate().NotNullable()
                .WithColumn("DESCRICAO").AsString(300).Nullable()
                .WithColumn("TIPO").AsInt16().NotNullable()
                .WithColumn("LIQUIDADO").AsBoolean().NotNullable()
                .WithColumn("VALOR").AsCurrency().NotNullable()
                .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                    .ForeignKey("FK_TIT_EVENTO", "EVENTOS", "ID_EVENTO").OnDelete(Rule.None).OnUpdate(Rule.None)
                .WithColumn("ID_FATURAMENTO").AsInt32().NotNullable()
                    .ForeignKey("FK_TIT_FAT", "FATURAMENTOS", "ID_FATURAMENTO").OnDelete(Rule.None).OnUpdate(Rule.None);

            Create
                .Index("IDX_TIT_1")
                .OnTable("TITULOS_FINANCEIROS")
                .OnColumn("ID_EVENTO").Ascending()
                .OnColumn("DATA_VENCIMENTO").Ascending()
                .OnColumn("DESCRICAO").Ascending()
                .OnColumn("TIPO").Ascending();
        }

        private void CriarTransacao()
        {
            Create
                .Table("TRANSACOES_FINANCEIRAS")
                .WithColumn("ID_TRANSACAO_FINACEIRA").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("DATA_HORA").AsDateTime().NotNullable()
                .WithColumn("O_QUE").AsString(500).NotNullable()
                .WithColumn("ID_CONTA").AsInt32().NotNullable()
                    .ForeignKey("FK_TRA_CONTA", "CONTAS", "ID_CONTA").OnDelete(Rule.None).OnUpdate(Rule.None)
                .WithColumn("TIPO").AsInt16().NotNullable()
                .WithColumn("VALOR").AsCurrency().NotNullable()
                .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                    .ForeignKey("FK_TRA_EVENTO", "EVENTOS", "ID_EVENTO").OnDelete(Rule.None).OnUpdate(Rule.None)
                .WithColumn("ID_ORIGEM").AsInt32().NotNullable();

            Create
                .Index("IDX_TRA_1")
                .OnTable("TRANSACOES_FINANCEIRAS")
                .OnColumn("ID_EVENTO").Ascending()
                .OnColumn("ID_CONTA").Ascending()
                .OnColumn("DATA_HORA").Ascending();

            Create
                .Index("IDX_TRA_2")
                .OnTable("TRANSACOES_FINANCEIRAS")
                .OnColumn("ID_CONTA").Ascending()
                .OnColumn("DATA_HORA").Ascending();
        }        
    }
}
