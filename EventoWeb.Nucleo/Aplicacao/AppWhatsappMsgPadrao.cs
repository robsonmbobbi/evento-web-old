using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppWhatsappMsgPadrao : AppBase, IComunicacao
    {
        private readonly AServicoWhatsapp m_ServicoComunicacao;
        private readonly AGeracaoMensagem m_GeradorMsg;

        public AppWhatsappMsgPadrao(IContexto contexto, AServicoWhatsapp servicoWhatsapp, AGeracaoMensagem geradorMsg) : base(contexto)
        {
            m_ServicoComunicacao = servicoWhatsapp;
            m_GeradorMsg = geradorMsg;
        }

        public void EnviarCodigoValidacao(int idEvento, DTOEnvioCodigoEmail dadosEnvio, string codigo)
        {
            var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
            var mensagem = ObterMensagem(idEvento);
            m_ServicoComunicacao.Configuracao = ObterConfiguracao(idEvento);
            m_ServicoComunicacao.Enviar(
                dadosEnvio.Whatsapp.FormatarCelular(),
                m_GeradorMsg.GerarMensagemModelo<DadosValidacaoEmail>(mensagem.MensagemInscricaoCodigoAcessoCriacao.Mensagem,
                    new DadosValidacaoEmail
                    {
                        Codigo = codigo,
                        Evento = evento.Nome
                    }
                )
            );
        }

        public void EnviarCodigoAcompanhamentoInscricao(Inscricao inscricao, string codigo)
        {
            var mensagem = ObterMensagem(inscricao.Evento.Id);
            m_ServicoComunicacao.Configuracao = ObterConfiguracao(inscricao.Evento.Id);
            m_ServicoComunicacao.Enviar(
                inscricao.Pessoa.Celular.FormatarCelular(),
                m_GeradorMsg.GerarMensagemModelo<DadosCodigoInscricao>(mensagem.MensagemInscricaoCodigoAcessoAcompanhamento.Mensagem,
                    new DadosCodigoInscricao
                    {
                        Codigo = codigo,
                        Evento = inscricao.Evento.Nome,
                        NomePessoa = inscricao.Pessoa.Nome,
                        Cidade = inscricao.Pessoa.Endereco.Cidade,
                        Identificacao = new AppInscOnLineIdentificacaoInscricao().GerarCodigo(inscricao.Id),
                        UF = inscricao.Pessoa.Endereco.UF
                    }
                )
            );
        }

        public void EnviarInscricaoRegistradaAdulto(InscricaoParticipante inscricao)
        {
            var mensagem = ObterMensagem(inscricao.Evento.Id);
            m_ServicoComunicacao.Configuracao = ObterConfiguracao(inscricao.Evento.Id);

            var dto = inscricao.ConverterComCodigo();
            dto.Codigo = new AppInscOnLineIdentificacaoInscricao().GerarCodigo(inscricao.Id);

            var idSarau = new AppInscOnLineIdentificacaoSarau();
            dto.Sarais = Contexto.RepositorioApresentacoesSarau.ListarPorInscricao(inscricao.Id)
                        .Select(x =>
                        {
                            var sarau = x.ConverterComCodigo();
                            sarau.Codigo = idSarau.GerarCodigo(x.Id);
                            return sarau;
                        })
                        .ToList();

            m_ServicoComunicacao.Enviar(
                inscricao.Pessoa.Celular.FormatarCelular(),
                m_GeradorMsg.GerarMensagemModelo<DTOInscricaoCompletaAdultoCodigo>(mensagem.MensagemInscricaoRegistradaAdulto.Mensagem, dto)
            );
        }

        public void EnviarInscricaoRegistradaInfantil(InscricaoInfantil inscricao)
        {
            var mensagem = ObterMensagem(inscricao.Evento.Id);
            m_ServicoComunicacao.Configuracao = ObterConfiguracao(inscricao.Evento.Id);

            var dto = inscricao.ConverterComCodigo();
            dto.Codigo = new AppInscOnLineIdentificacaoInscricao().GerarCodigo(inscricao.Id);

            var idSarau = new AppInscOnLineIdentificacaoSarau();
            dto.Sarais = Contexto.RepositorioApresentacoesSarau.ListarPorInscricao(inscricao.Id)
                        .Select(x =>
                        {
                            var sarau = x.ConverterComCodigo();
                            sarau.Codigo = idSarau.GerarCodigo(x.Id);
                            return sarau;
                        })
                        .ToList();

            m_ServicoComunicacao.Enviar(
                inscricao.Pessoa.Celular.FormatarCelular(),
                m_GeradorMsg.GerarMensagemModelo<DTOInscricaoCompletaInfantilCodigo>(mensagem.MensagemInscricaoRegistradaAdulto.Mensagem, dto)
            );
        }

        public void EnviarInscricaoAceita(Inscricao inscricao)
        {
            var mensagem = ObterMensagem(inscricao.Evento.Id);
            m_ServicoComunicacao.Configuracao = ObterConfiguracao(inscricao.Evento.Id);
            m_ServicoComunicacao.Enviar(
                inscricao.Pessoa.Celular.FormatarCelular(),
                m_GeradorMsg.GerarMensagemModelo<DadosConfirmacaoInscricao>(mensagem.MensagemInscricaoConfirmada.Mensagem,
                    new DadosConfirmacaoInscricao
                    {
                        Evento = inscricao.Evento.Nome,
                        NomePessoa = inscricao.Pessoa.Nome,
                    }
                )
            );
        }

        public void EnviarInscricaoRejeitada(Inscricao inscricao)
        {

        }

        private MensagemWhatsappPadrao ObterMensagem(int idEvento)
        {
            return Contexto.RepositorioMensagensWhatsappPadrao.Obter(idEvento) ??
                throw new ExcecaoAplicacao(nameof(AppWhatsappMsgPadrao), "Mensagem Padrão não foi cadastrada");
        }

        private ConfiguracaoWhatsapp ObterConfiguracao(int idEvento)
        {
            return Contexto.RepositorioConfiguracoesWhatsapp.Obter(idEvento) ??
                throw new ExcecaoAplicacao(nameof(AppWhatsappMsgPadrao), "Configuração de Whatsapp não foi cadastrada");
        }
    }

    public static class MetodosExtensaoWhatsApp
    {
        public static string FormatarCelular(this string celular)
        {
            return $"+55 {celular}";
        }
    }
}
