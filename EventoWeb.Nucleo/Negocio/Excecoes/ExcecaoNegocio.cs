using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Excecoes
{
    public class ExcecaoNegocio : Exception
    {
        public ExcecaoNegocio(string classe, string erro)
            :base(erro)
        {
            Classe = classe;
        }

        public string Classe { get; }
    }
}
