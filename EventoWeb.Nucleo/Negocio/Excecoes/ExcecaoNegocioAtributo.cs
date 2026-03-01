using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Excecoes
{
    public class ExcecaoNegocioAtributo: Exception
    {
        public ExcecaoNegocioAtributo(string classe, string atributo, string erro)
            :base(erro)
        {
            Classe = classe;
            Atributo = atributo;
        }

        public string Classe { get; }
        public string Atributo { get; }
    }
}
