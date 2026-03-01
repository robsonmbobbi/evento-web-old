using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppMensagensEmailInscricao : AppBase
    {
        public AppMensagensEmailInscricao(IContexto contexto) : base(contexto)
        {
        }

        public DTOMensagemEmailInscricao ObterPorEvento(int idEvento)
        {
            DTOMensagemEmailInscricao dto = null;
            ExecutarSeguramente(() =>
            {
                var mensagem = Contexto.RepositorioMensagensEmailPadrao.Obter(idEvento);

                if (mensagem != null)
                    dto = mensagem.Converter();
            });

            return dto;
        }

        public void Atualizar(int idEvento, DTOMensagemEmailInscricao dto)
        {
            ExecutarSeguramente(() =>
            {
                var mensagem = Contexto.RepositorioMensagensEmailPadrao.Obter(idEvento);
                var ehInclusao = false;
                if (mensagem == null)
                {
                    mensagem = new MensagemEmailPadrao(Contexto.RepositorioEventos.ObterEventoPeloId(idEvento));
                    ehInclusao = true;
                }

                if (dto.MensagemInscricaoCodigoAcessoAcompanhamento == null)
                    mensagem.MensagemInscricaoCodigoAcessoAcompanhamento = null;
                else
                    mensagem.MensagemInscricaoCodigoAcessoAcompanhamento = new ModeloMensagem(dto.MensagemInscricaoCodigoAcessoAcompanhamento.Assunto,
                        dto.MensagemInscricaoCodigoAcessoAcompanhamento.Mensagem);

                if (dto.MensagemInscricaoCodigoAcessoCriacao == null)
                    mensagem.MensagemInscricaoCodigoAcessoCriacao = null;
                else
                    mensagem.MensagemInscricaoCodigoAcessoCriacao = new ModeloMensagem(dto.MensagemInscricaoCodigoAcessoCriacao.Assunto,
                        dto.MensagemInscricaoCodigoAcessoCriacao.Mensagem);

                if (dto.MensagemInscricaoConfirmada == null)
                    mensagem.MensagemInscricaoConfirmada = null;
                else
                    mensagem.MensagemInscricaoConfirmada = new ModeloMensagem(dto.MensagemInscricaoConfirmada.Assunto,
                        dto.MensagemInscricaoConfirmada.Mensagem);

                if (dto.MensagemInscricaoRegistradaAdulto == null)
                    mensagem.MensagemInscricaoRegistradaAdulto = null;
                else
                    mensagem.MensagemInscricaoRegistradaAdulto = new ModeloMensagem(dto.MensagemInscricaoRegistradaAdulto.Assunto,
                        dto.MensagemInscricaoRegistradaAdulto.Mensagem);

                if (dto.MensagemInscricaoRegistradaInfantil == null)
                    mensagem.MensagemInscricaoRegistradaInfantil = null;
                else
                    mensagem.MensagemInscricaoRegistradaInfantil = new ModeloMensagem(dto.MensagemInscricaoRegistradaInfantil.Assunto,
                        dto.MensagemInscricaoRegistradaInfantil.Mensagem);

                if (ehInclusao)
                    Contexto.RepositorioMensagensEmailPadrao.Incluir(mensagem);
                else
                    Contexto.RepositorioMensagensEmailPadrao.Atualizar(mensagem);
            });
        }
    }
}
