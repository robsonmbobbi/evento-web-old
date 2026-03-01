using System;

namespace EventoWeb.WS.Secretaria
{
    public class ExcecaoAPI: Exception
    {
        public ExcecaoAPI(string api, string erro)
            :base(erro)
        {
            API = api;
        }

        public string API { get; }
    }
}
