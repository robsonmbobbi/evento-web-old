using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EventoWeb.BancoDados.Migracoes
{
    [Migration(01)]
    public class Migracao01 : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            //CriarTabelaUsuarios();

            CriarTabelaArquivoBinario();
            CriarTabelaEventos();            
            CriarTabelaSalasEstudo();
            CriarTabelaOficinas();
            CriarTabelaDepartamentos();
            CriarTabelaConfiguracaoEmail();
            CriarTabelaMensagemEmailPadrao();            
            CriarTabelaPessoa();
            CriarTabelaInscricao();
            CriarTabelaAtividadesInscricao();
            CriarTabelaApresentacaoSarau();
            CriarTabelaCodigoAcessoInscricao();
        }        

        private void CriarTabelaArquivoBinario()
        {
            Create.Table("arquivos_binarios")
                .WithColumn("ID_ARQUIVO").AsInt32().PrimaryKey().Identity()
                .WithColumn("ARQUIVO").AsBinary().NotNullable()
                .WithColumn("TIPO_ARQUIVO").AsInt16().NotNullable();
        }

        private void CriarTabelaEventos()
        {
            Create.Table("eventos")
                .WithColumn("ID_EVENTO").AsInt32().PrimaryKey().Identity()
                .WithColumn("NOME").AsString(250).NotNullable()
                .WithColumn("DATA_INICIO_INSC").AsDateTime().NotNullable()
                .WithColumn("DATA_FIM_INSC").AsDateTime().NotNullable()
                .WithColumn("DATA_INICIO_EVENTO").AsDateTime().NotNullable()
                .WithColumn("DATA_FIM_EVENTO").AsDateTime().NotNullable()
                .WithColumn("DATA_REGISTRO").AsDateTime().NotNullable()
                .WithColumn("ID_ARQUIVO_LOGOTIPO").AsInt32().Nullable()
                    .ForeignKey("FK_EVENTO_ARQBIN", "arquivos_binarios", "ID_ARQUIVO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                .WithColumn("TEM_DEPARTAMENTALIZACAO").AsBoolean().NotNullable()
                .WithColumn("MODELO_DIV_SL_ESTUDO").AsInt16().Nullable()
                .WithColumn("TEM_OFICINAS").AsBoolean().NotNullable()
                .WithColumn("TEM_DORMITORIOS").AsBoolean().NotNullable()
                .WithColumn("PUBLICO_EVANGELIZACAO").AsInt16().Nullable()
                .WithColumn("TEMPO_SARAU_MIN").AsInt32().Nullable()
                .WithColumn("IDADE_MINIMA_INSC_ADULTO").AsInt32().NotNullable()
                .WithColumn("VALOR_INSC_ADULTO").AsDecimal(6, 2).NotNullable()
                .WithColumn("VALOR_INSC_CRIANCA").AsDecimal(6, 2).NotNullable();
        }

        private void CriarTabelaUsuarios()
        {
            Create.Table("usuarios")
                .WithColumn("LOGIN").AsString(150).PrimaryKey();

            Insert.IntoTable("USUARIOS").Row(new { LOGIN = "robsonmbobbi@gmail.com" });
        }

        private void CriarTabelaSalasEstudo()
        {
            Create
                .Table("salas_estudo")
                    .WithColumn("ID_SALA_ESTUDO").AsInt32().PrimaryKey().Identity()
                    .WithColumn("PAR_TOTAL_PARTICIPANTES").AsInt16().NotNullable().WithDefaultValue(0)
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_EVENTO_SALA", "eventos", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("IDADE_MAX").AsInt32().Nullable()
                    .WithColumn("IDADE_MIN").AsInt32().Nullable()
                    .WithColumn("NOME").AsString(250).NotNullable();                
        }

        private void CriarTabelaOficinas()
        {
            Create
                .Table("oficinas")
                    .WithColumn("ID_OFICINA").AsInt32().PrimaryKey().Identity()
                    .WithColumn("PAR_TOTAL_PARTICIPANTES").AsInt16().NotNullable().WithDefaultValue(0)
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_OFICINA_EVENTO", "eventos", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("NOME").AsString(250).NotNullable()
                    .WithColumn("NUM_MAX_PARTICIPANTES").AsInt32().Nullable();
        }

        private void CriarTabelaDepartamentos()
        {
            Create
                .Table("departamentos")
                    .WithColumn("ID_DEPARTAMENTO").AsInt32().PrimaryKey().Identity()
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_EVENTO_DEP", "eventos", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("NOME").AsString(250).NotNullable();
        }

        private void CriarTabelaConfiguracaoEmail()
        {
            Create
                .Table("configuracoes_email")
                    .WithColumn("ID_CONFIGURACAO_EMAIL").AsInt32().PrimaryKey().Identity()
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_EVENTO_CNFEMAIL", "eventos", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("ENDERECO_EMAIL").AsString(150).NotNullable()
                    .WithColumn("USUARIO_EMAIL").AsString(100).NotNullable()
                    .WithColumn("SENHA_EMAIL").AsString(100).NotNullable()
                    .WithColumn("SERVIDOR_EMAIL").AsString(50).NotNullable()
                    .WithColumn("PORTA_SERVIDOR").AsInt32().NotNullable()
                    .WithColumn("TIPO_SEGURANCA").AsInt16().NotNullable();
        }

        private void CriarTabelaMensagemEmailPadrao()
        {
            Create
                .Table("mensagens_email_padrao")
                    .WithColumn("ID_MENSAGEM_EMAIL_PADRAO").AsInt32().PrimaryKey().Identity()
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_MSGEMAILP_EVENTO", "eventos", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                     .WithColumn("ASSUNTO_INSC_CONFIRMADA").AsString(150).Nullable()
                    .WithColumn("MENSAGEM_INSC_CONFIRMADA").AsString(Int32.MaxValue).Nullable()
                    .WithColumn("ASSUNTO_INSC_REGISTRADA").AsString(150).Nullable()
                    .WithColumn("MENSAGEM_INSC_REGISTRADA").AsString(Int32.MaxValue).Nullable()
                    .WithColumn("ASSUNTO_INSC_COD_ACESSO_ACOMP").AsString(150).Nullable()
                    .WithColumn("MENSAGEM_INSC_COD_ACESSO_ACOMP").AsString(Int32.MaxValue).Nullable()
                    .WithColumn("ASSUNTO_INSC_COD_ACESSO_CRI").AsString(150).Nullable()
                    .WithColumn("MENSAGEM_INSC_COD_ACESSO_CRI").AsString(Int32.MaxValue).Nullable();
        }

        private void CriarTabelaPessoa()
        {
            Create
                .Table("pessoas")
                    .WithColumn("ID_PESSOA").AsInt32().PrimaryKey().Identity()
                     .WithColumn("TIPO_PESSOA").AsString(30).NotNullable()
                    .WithColumn("NOME").AsString(150).NotNullable()
                    .WithColumn("CELULAR").AsString(15).Nullable()
                    .WithColumn("EMAIL").AsString(100).Nullable()
                    .WithColumn("TELEFONE_FIXO").AsString(15).Nullable()
                    .WithColumn("ALERGIA_ALIMENTOS").AsString(100).Nullable()
                    .WithColumn("DATA_NASCIMENTO").AsDate().Nullable()
                    .WithColumn("EH_DIABETICO").AsBoolean().Nullable()
                    .WithColumn("EH_VEGETARIANO").AsBoolean().Nullable()
                    .WithColumn("CIDADE").AsString(100).Nullable()
                    .WithColumn("UF").AsString(2).Nullable()
                    .WithColumn("MEDICAMENTOS_USADOS").AsString(150).Nullable()
                    .WithColumn("SEXO").AsInt16().Nullable()
                    .WithColumn("TIPOS_CARNE_NAO_COME").AsString(150).Nullable()
                    .WithColumn("USA_ADOCANTE_DIAR").AsBoolean().Nullable();
        }

        private void CriarTabelaInscricao()
        {
            Create
                .Table("inscricoes")
                    .WithColumn("ID_INSCRICAO").AsInt32().PrimaryKey().Identity()
                    .WithColumn("TIPO_INSCRICAO").AsString(30).NotNullable()
                    .WithColumn("CONFIRMADO").AsBoolean().NotNullable()
                    .WithColumn("DATA_RECEBIMENTO").AsDate().NotNullable()
                    .WithColumn("DORMIRA").AsBoolean().NotNullable()
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_INSC_EVENTO", "eventos", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("INSTITUICOES_ESPIRITAS_FREQ").AsString(300).Nullable()
                    .WithColumn("NOME_CRACHA").AsString(150).Nullable()
                    .WithColumn("NOME_RESP_CENTRO").AsString(150).Nullable()
                    .WithColumn("NOME_RESP_LEGAL").AsString(150).Nullable()
                    .WithColumn("OBSERVACOES").AsString(Int32.MaxValue).Nullable()
                    .WithColumn("ID_PESSOA").AsInt32().NotNullable()
                        .ForeignKey("FK_INSC_PESSOA", "pessoas", "ID_PESSOA").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("PRIMEIRO_ENCONTRO").AsBoolean().NotNullable()
                    .WithColumn("SITUACAO").AsInt16().NotNullable()
                    .WithColumn("TELEFONE_RESP_CENTRO").AsString(15).Nullable()
                    .WithColumn("TELEFONE_RESP_LEGAL").AsString(15).Nullable()
                    .WithColumn("TEMPO_ESPIRITA").AsString(20).Nullable()
                    .WithColumn("ID_INSC_RESPONSAVEL_1").AsInt32().Nullable()
                        .ForeignKey("FK_INSC_INF_1", "inscricoes", "ID_INSCRICAO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("ID_INSC_RESPONSAVEL_2").AsInt32().Nullable()
                        .ForeignKey("FK_INSC_INF_2", "inscricoes", "ID_INSCRICAO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("TIPO").AsInt16().Nullable()
                    .WithColumn("FORMA_PAGAMENTO").AsInt16().Nullable()
                    .WithColumn("OBS_PAGAMENTO").AsString(Int32.MaxValue).Nullable();

            Create
               .Table("pagamento_inscricao_comprovantes")
                   .WithColumn("ID_PAG_INSC_COMPROVANTES").AsInt32().PrimaryKey().Identity()
                   .WithColumn("ID_ARQUIVO").AsInt32()
                      .ForeignKey("FK_PAGINSC_ARQ", "arquivos_binarios", "ID_ARQUIVO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                   .WithColumn("ID_INSCRICAO").AsInt32()
                      .ForeignKey("FK_PAGINSC_INSC", "inscricoes", "ID_INSCRICAO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade);
        }

        private void CriarTabelaAtividadesInscricao()
        {
            Create
                .Table("atividades_inscricao")
                    .WithColumn("ID_ATIVIDADE_INSCRICAO").AsInt32().PrimaryKey().Identity()
                    .WithColumn("TIPO_ATIVIDADE").AsString(30).NotNullable()
                    .WithColumn("ID_INSCRICAO").AsInt32().NotNullable()
                        .ForeignKey("FK_ATIVIDADE_INSC", "inscricoes", "ID_INSCRICAO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("ID_DEPARTAMENTO").AsInt32().Nullable()
                        .ForeignKey("FK_ATIVIDADE_DEP", "departamentos", "ID_DEPARTAMENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("EH_COORD_DEP").AsBoolean().Nullable()
                    .WithColumn("ID_OFICINA").AsInt32().Nullable()
                        .ForeignKey("FK_ATIVIDADE_OFICINA", "oficinas", "ID_OFICINA").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("ID_SALA_ESTUDO").AsInt32().Nullable()
                        .ForeignKey("FK_ATIVIDADE_SALA", "salas_estudo", "ID_SALA_ESTUDO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade);

            Create
               .Table("oficinas_escolhidas")
                   .WithColumn("ID_ATIVIDADE_INSCRICAO").AsInt32().PrimaryKey()
                      .ForeignKey("FK_OFIC_ESC_ATIVIDADE", "atividades_inscricao", "ID_ATIVIDADE_INSCRICAO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                   .WithColumn("ID_OFICINA").AsInt32().PrimaryKey()
                      .ForeignKey("FK_OFIC_ESC_INSCRICAO", "oficinas", "ID_OFICINA").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                   .WithColumn("POSICAO").AsInt32();

            Create
               .Table("salas_estudo_escolhidas")
                   .WithColumn("ID_ATIVIDADE_INSCRICAO").AsInt32().PrimaryKey()
                      .ForeignKey("FK_SALA_ESC_ATIVIDADE", "atividades_inscricao", "ID_ATIVIDADE_INSCRICAO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                   .WithColumn("ID_SALA_ESTUDO").AsInt32().PrimaryKey()
                      .ForeignKey("FK_SALA_ESC_INSCRICAO", "salas_estudo", "ID_SALA_ESTUDO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                   .WithColumn("POSICAO").AsInt32();
        }

        private void CriarTabelaApresentacaoSarau()
        {
            Create
                .Table("apresentacoes_sarau")
                    .WithColumn("ID_APRESENTACAO_SARAU").AsInt32().PrimaryKey().Identity()
                    .WithColumn("ID_EVENTO").AsInt32().NotNullable()
                        .ForeignKey("FK_APRESENTACAO_EVENTO", "eventos", "ID_EVENTO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                    .WithColumn("DURACAO_MIN").AsInt32().NotNullable()
                    .WithColumn("TIPO").AsString(200).NotNullable();

            Create
               .Table("apresentacoes_sarau_inscritos")
                   .WithColumn("ID_APRESENTACAO_SARAU").AsInt32().PrimaryKey()
                      .ForeignKey("FK_ASI_APRESENTACAO", "apresentacoes_sarau", "ID_APRESENTACAO_SARAU").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade)
                   .WithColumn("ID_INSCRICAO").AsInt32().PrimaryKey()
                      .ForeignKey("FK_ASI_INSCRICAO", "inscricoes", "ID_INSCRICAO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade);
        }

        private void CriarTabelaCodigoAcessoInscricao()
        {
            Create
                .Table("codigos_acesso_inscricao")
                    .WithColumn("ID_CODIGO_ACESSO_INS").AsInt32().PrimaryKey().Identity()
                    .WithColumn("CODIGO").AsString(100).NotNullable()
                    .WithColumn("DATA_HORA_VALIDADE").AsDateTime().NotNullable()
                    .WithColumn("ID_INSCRICAO").AsInt32().NotNullable()
                        .ForeignKey("FK_CAI_INSCRICAO", "inscricoes", "ID_INSCRICAO").OnDelete(Rule.Cascade).OnUpdate(Rule.Cascade);
        }
    }
}
