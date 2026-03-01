using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Periodo
    {
        public Periodo(DateTime dataInicial, DateTime dataFinal)
        {
            if (dataFinal < dataInicial)
                throw new ExcecaoNegocioAtributo("Periodo", "dataFinal", "Data final deve ser maior ou igual a inicial");

            DataInicial = dataInicial;
            DataFinal = dataFinal;
        }

        protected Periodo() { }

        public virtual DateTime DataInicial { get; protected set; }
        public virtual DateTime DataFinal { get; protected set; }
    }
}
