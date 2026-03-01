using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppEmailMsgPadrao : AppBase, IComunicacao
    {
        private readonly AServicoEmail m_ServicoEmail;
        private readonly AGeracaoMensagem m_GeradorMsgEmail;

        public AppEmailMsgPadrao(IContexto contexto, AServicoEmail servicoEmail, AGeracaoMensagem geradorMsgEmail) : base(contexto)
        {
            m_ServicoEmail = servicoEmail;
            m_GeradorMsgEmail = geradorMsgEmail;
        }

        public void EnviarCodigoValidacao(int idEvento, DTOEnvioCodigoEmail dadosEnvio, string codigo)
        {
            var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
            var mensagem = ObterMensagem(idEvento);
            m_ServicoEmail.Configuracao = ObterCnfEmail(idEvento);
            m_ServicoEmail.Enviar(new Email
            {
                Assunto = mensagem.MensagemInscricaoCodigoAcessoCriacao.Assunto,
                Conteudo = m_GeradorMsgEmail.GerarMensagemModelo<DadosValidacaoEmail>(mensagem.MensagemInscricaoCodigoAcessoCriacao.Mensagem, 
                    new DadosValidacaoEmail
                    {
                        Codigo = codigo,
                        Evento = evento.Nome
                    }
                ),
                Endereco = dadosEnvio.Email
            });
        }

        public void EnviarCodigoAcompanhamentoInscricao(Inscricao inscricao, string codigo)
        {
            var mensagem = ObterMensagem(inscricao.Evento.Id);
            m_ServicoEmail.Configuracao = ObterCnfEmail(inscricao.Evento.Id);
            m_ServicoEmail.Enviar(new Email
            {
                Assunto = mensagem.MensagemInscricaoCodigoAcessoAcompanhamento.Assunto,
                Conteudo = m_GeradorMsgEmail.GerarMensagemModelo<DadosCodigoInscricao>(mensagem.MensagemInscricaoCodigoAcessoAcompanhamento.Mensagem,
                    new DadosCodigoInscricao
                    {
                        Codigo = codigo,
                        Evento = inscricao.Evento.Nome,
                        NomePessoa = inscricao.Pessoa.Nome,
                        Cidade = inscricao.Pessoa.Endereco.Cidade,
                        Identificacao = new AppInscOnLineIdentificacaoInscricao().GerarCodigo(inscricao.Id),
                        UF = inscricao.Pessoa.Endereco.UF
                    }
                ),
                Endereco = inscricao.Pessoa.Email
            });
        }

        public void EnviarInscricaoRegistradaAdulto(InscricaoParticipante inscricao)
        {
            var mensagem = ObterMensagem(inscricao.Evento.Id);
            m_ServicoEmail.Configuracao = ObterCnfEmail(inscricao.Evento.Id);

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

            m_ServicoEmail.Enviar(new Email
            {
                Assunto = mensagem.MensagemInscricaoRegistradaAdulto.Assunto,
                Conteudo = m_GeradorMsgEmail.GerarMensagemModelo<DTOInscricaoCompletaAdultoCodigo>(mensagem.MensagemInscricaoRegistradaAdulto.Mensagem, dto),
                Endereco = inscricao.Pessoa.Email
            });
        }

        public void EnviarInscricaoRegistradaInfantil(InscricaoInfantil inscricao)
        {
            var mensagem = ObterMensagem(inscricao.Evento.Id);
            m_ServicoEmail.Configuracao = ObterCnfEmail(inscricao.Evento.Id);

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

            m_ServicoEmail.Enviar(new Email
            {
                Assunto = mensagem.MensagemInscricaoRegistradaAdulto.Assunto,
                Conteudo = m_GeradorMsgEmail.GerarMensagemModelo<DTOInscricaoCompletaInfantilCodigo>(mensagem.MensagemInscricaoRegistradaAdulto.Mensagem, dto),
                Endereco = inscricao.Pessoa.Email
            });
        }

        public void EnviarInscricaoAceita(Inscricao inscricao)
        {
            var mensagem = ObterMensagem(inscricao.Evento.Id);
            m_ServicoEmail.Configuracao = ObterCnfEmail(inscricao.Evento.Id);
            m_ServicoEmail.Enviar(new Email
            {
                Assunto = mensagem.MensagemInscricaoConfirmada.Assunto,
                Conteudo = m_GeradorMsgEmail.GerarMensagemModelo<DadosConfirmacaoInscricao>(mensagem.MensagemInscricaoConfirmada.Mensagem,
                    new DadosConfirmacaoInscricao
                    {
                        Evento = inscricao.Evento.Nome,
                        NomePessoa = inscricao.Pessoa.Nome,
                    }
                ),
                Endereco = inscricao.Pessoa.Email
            });
        }

        public void EnviarInscricaoRejeitada(Inscricao inscricao)
        {
            
        }

        private MensagemEmailPadrao ObterMensagem(int idEvento)
        {
            return Contexto.RepositorioMensagensEmailPadrao.Obter(idEvento) ??
                throw new ExcecaoAplicacao("AppEmailMsgPadrao", "Mensagem Padrão de Email não foi cadastrada");
        }

        private ConfiguracaoEmail ObterCnfEmail(int idEvento)
        {
            return Contexto.RepositorioConfiguracoesEmail.Obter(idEvento) ??
                throw new ExcecaoAplicacao("AppEmailMsgPadrao", "Configuração de Email não foi cadastrada");
        }
    }

    public class DadosValidacaoEmail
    {
        public string Evento { get; set; }
        public string Codigo { get; set; }
    }

    public class DadosCodigoInscricao
    {
        public string Evento { get; set; }
        public string Identificacao { get; set; }
        public string NomePessoa { get; set; }
        public string Codigo { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
    }

    public class DadosConfirmacaoInscricao
    {
        public string NomePessoa { get; set; }
        public string Evento { get; set; }
    }
}
