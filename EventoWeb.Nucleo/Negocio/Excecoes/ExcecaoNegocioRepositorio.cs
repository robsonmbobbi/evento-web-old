using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Excecoes
{
    public class ExcecaoNegocioRepositorio : Exception
    {
        public ExcecaoNegocioRepositorio(string classe, string erro)
            : base(erro)
        {
            Classe = classe;
        }

        public string Classe { get; }
    }
}
