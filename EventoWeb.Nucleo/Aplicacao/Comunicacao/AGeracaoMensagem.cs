using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao.Comunicacao
{
    public abstract class AGeracaoMensagem
    {
        public abstract string GerarMensagemModelo<T>(string modeloMensagem, T objetoDados);
    }
}
