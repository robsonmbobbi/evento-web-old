using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class DTOMensagemEmailInscricao
    {
        public DTOModeloMensagem MensagemInscricaoRegistradaAdulto { get; set; }
        public DTOModeloMensagem MensagemInscricaoConfirmada { get; set; }
        public DTOModeloMensagem MensagemInscricaoCodigoAcessoAcompanhamento { get; set; }
        public DTOModeloMensagem MensagemInscricaoCodigoAcessoCriacao { get; set; }
        public DTOModeloMensagem MensagemInscricaoRegistradaInfantil { get; set; }
    }

    public class DTOModeloMensagem
    {
        public string Assunto { get; set; }
        public string Mensagem { get; set; }
    }
}
