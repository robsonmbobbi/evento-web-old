using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao.ConversoresDTO
{
    public static class ConversorMensagensEmailInscricao
    {
        public static DTOMensagemEmailInscricao Converter(this MensagemEmailPadrao mensagem)
        {
            if (mensagem == null)
                return null;
            else
                return new DTOMensagemEmailInscricao
                {
                    MensagemInscricaoCodigoAcessoAcompanhamento = mensagem.MensagemInscricaoCodigoAcessoAcompanhamento.Converter(),
                    MensagemInscricaoConfirmada = mensagem.MensagemInscricaoConfirmada.Converter(),
                    MensagemInscricaoCodigoAcessoCriacao = mensagem.MensagemInscricaoCodigoAcessoCriacao.Converter(),
                    MensagemInscricaoRegistradaAdulto = mensagem.MensagemInscricaoRegistradaAdulto.Converter(),
                    MensagemInscricaoRegistradaInfantil = mensagem.MensagemInscricaoRegistradaInfantil.Converter(),
                };
        }

        public static DTOModeloMensagem Converter(this ModeloMensagem modelo)
        {
            if (modelo == null)
                return null;
            else
                return new DTOModeloMensagem
                {
                    Assunto = modelo.Assunto,
                    Mensagem = modelo.Mensagem
                };
        }
    }
}
