using EventoWeb.Nucleo.Negocio.Excecoes;
using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Titulo : EntidadeFinanceira
    {
        private Decimal m_Valor;

        public Titulo(Evento evento, EnumTipoTransacao tipo, Decimal valor, DateTime dataVencimento, Faturamento origem)
            :base(evento, tipo)
        {
            if (origem == null)
                throw new ExcecaoNegocioAtributo("Transacao", "origem", "A origem precisa ser informada.");

            if (origem.Evento != evento)
                throw new ExcecaoNegocioAtributo("Transacao", "origem", "A origem deve ser do mesmo evento do titulo.");

            if (origem.Tipo != tipo)
                throw new ExcecaoNegocioAtributo("Transacao", "origem", "A origem deve ser do mesmo tipo do titulo.");

            Origem = origem;
            DataCriado = DateTime.Now;
            Liquidado = false;
            DataVencimento = dataVencimento;
            Valor = valor;
        }

        protected Titulo() { }

        public virtual decimal Valor 
        {
            get => m_Valor;
            set
            {
                if (Liquidado)
                    throw new ExcecaoNegocio("Titulo", "Não é possível alterar o valor de um titulo liquidado");

                if (value <= 0)
                    throw new ExcecaoNegocioAtributo("Titulo", "Valor", "O valor deve ser maior que zero");

                m_Valor = value;
            }
        }

        public virtual bool Liquidado { get; set; }

        public virtual DateTime DataCriado { get; protected set; }

        public virtual string Descricao { get; set; }

        public virtual DateTime DataVencimento { get; set; }

        public virtual Faturamento Origem { get; protected set; }
    }
}
