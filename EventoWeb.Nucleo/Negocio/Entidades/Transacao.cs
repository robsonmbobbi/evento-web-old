using EventoWeb.Nucleo.Negocio.Excecoes;
using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum EnumTipoTransacao { Receita, Despesa }

    public class Transacao : Entidade 
    {
        private string m_OQue;
        private decimal m_Valor;

        public Transacao(Evento evento, Conta conta, EntidadeFinanceira origem, DateTime dataHora, String oQue, decimal valor,
            EnumTipoTransacao tipo)
        {
            QualEvento = evento ?? throw new ExcecaoNegocioAtributo("Transacao", "evento", "O evento precisa ser informado.");
            QualConta = conta ?? throw new ExcecaoNegocioAtributo("Transacao", "conta", "A conta precisa ser informada.");
            Origem = origem ?? throw new ExcecaoNegocioAtributo("Transacao", "origem", "A origem precisa ser informada.");

            if (QualConta.QualEvento != QualEvento)
                throw new ExcecaoNegocioAtributo("Transacao", "conta", "A conta deve ser do mesmo evento da transação.");

            if (Origem.Evento != QualEvento)
                throw new ExcecaoNegocioAtributo("Transacao", "origem", "A origem deve ser do mesmo evento da transação.");

            if (origem.Tipo != tipo)
                throw new ExcecaoNegocioAtributo("Transacao", "origem", "A origem deve ser do mesmo tipo da transação.");

            DataHora = dataHora;
            OQue = oQue;
            Valor = valor;
            Tipo = tipo;
        }

        protected Transacao() { }

        public virtual Evento QualEvento { get; protected set; }

        public virtual Conta QualConta { get; protected set; }

        public virtual EntidadeFinanceira Origem { get; protected set; }

        public virtual DateTime DataHora { get; protected set; }

        public virtual String OQue
        {
            get { return m_OQue; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ExcecaoNegocioAtributo("Transacao", "OQue", "O que é esta transação precisa ser informado.");

                m_OQue = value;
            }
        }

        public virtual Decimal Valor
        {
            get { return m_Valor; }
            set
            {
                if (value <= 0)
                    throw new ExcecaoNegocioAtributo("Transacao", "Valor", "O valor deve ser maior que zero.");

                m_Valor = value;
            }
        }

        public virtual EnumTipoTransacao Tipo { get; protected set; }
    }
}
