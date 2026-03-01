using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public abstract class EntidadeFinanceira: Entidade
    {
        public EntidadeFinanceira(Evento evento, EnumTipoTransacao tipo)
        {
            Evento = evento ?? throw new ExcecaoNegocioAtributo("EntidadeFinanceira", "evento", "O evento precisa ser informado.");
            Tipo = tipo;
        }

        protected EntidadeFinanceira() { }

        public virtual Evento Evento { get; protected set; }

        public virtual EnumTipoTransacao Tipo { get; protected set; }
    }
}
