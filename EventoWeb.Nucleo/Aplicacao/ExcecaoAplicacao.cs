using System;
using System.Runtime.Serialization;

namespace EventoWeb.Nucleo.Aplicacao
{
    internal class ExcecaoAplicacao : Exception
    {
        public ExcecaoAplicacao(string app, string mensagem)
            :base(mensagem)
        {
            App = app;
        }

        public String App { get; }
    }
}