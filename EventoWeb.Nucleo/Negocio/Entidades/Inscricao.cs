using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum TipoSituacaoPagamento { Pago, Pagar, Isento }

    public enum EnumSituacaoInscricao { Incompleta, Pendente, Aceita, Rejeitada }

    public abstract class Inscricao: Entidade
    {
        private Pessoa m_Pessoa;
        private Evento m_Evento;
        private EnumSituacaoInscricao m_Situacao;
        private Pagamento m_Pagamento;

        public Inscricao(Evento evento, Pessoa pessoa, DateTime dataRecebimento)
        {
            if (evento == null)
                throw new ArgumentNullException("Evento");

            if (pessoa == null)
                throw new ArgumentNullException("Pessoa");

            m_Pessoa = pessoa;
            m_Evento = evento;
            m_Situacao = EnumSituacaoInscricao.Incompleta;
            DataRecebimento = dataRecebimento;
            ConfirmadoNoEvento = false;
            DormeEvento = true;
            m_Pagamento = new Pagamento(this);

            if (!EhValidaIdade(pessoa.CalcularIdadeEmAnos(evento.PeriodoRealizacaoEvento.DataInicial)))
                throw new ArgumentException("A idade da pessoa é inválida para este tipo de inscrição.");

            IsentarInscricao();
        }

        protected Inscricao() { }

        public virtual Pessoa Pessoa { get { return m_Pessoa; } }
        public virtual Evento Evento { get { return m_Evento; } }        

        public virtual DateTime DataRecebimento { get; set; }

        public virtual Boolean DormeEvento { get; set; }

        public virtual Titulo TituloPagar { get; protected set; }

        public virtual Transacao TransacaoPagamento { get; protected set; }

        public virtual TipoSituacaoPagamento SituacaoFinanceiro { get; protected set; }

        public virtual bool ConfirmadoNoEvento { get; set; }

        public virtual EnumSituacaoInscricao Situacao { get => m_Situacao; }
        public virtual String NomeCracha { get; set; }
        public virtual string Observacoes { get; set; }
        public virtual Pagamento Pagamento { get => m_Pagamento; }


        public virtual void PagarDepois(Titulo titulo)
        {
            if (titulo == null)
                throw new ArgumentNullException("titulo");

            if (TituloPagar != null)
                throw new ArgumentException("Esta inscrição já foi vinculada a um título.");

            if (SituacaoFinanceiro == TipoSituacaoPagamento.Pago)
                throw new ArgumentException("Esta inscrição já foi paga.");

            TituloPagar = titulo;
            SituacaoFinanceiro = TipoSituacaoPagamento.Pagar;
        }

        public virtual void PagarAgora(Transacao transacao)
        {
            if (transacao == null)
                throw new ArgumentNullException("transacao");

            if (TransacaoPagamento != null)
                throw new ArgumentException("Esta inscrição já foi vinculada a uma Transação de Pagamento.");

            TransacaoPagamento = transacao;
            SituacaoFinanceiro = TipoSituacaoPagamento.Pago;
        }

        public virtual void IsentarInscricao()
        {
            TituloPagar = null;
            TransacaoPagamento = null;
            SituacaoFinanceiro = TipoSituacaoPagamento.Isento;
        }

        public abstract Boolean EhValidaIdade(int idade);

        public virtual void TornarPendente()
        {
            if (m_Situacao != EnumSituacaoInscricao.Incompleta)
                throw new ExcecaoNegocio("Inscricao", "Só se pode tornar uma inscrição pendente se ela estiver na situação Incompleta");

            ValidarInscricaoParaSeTornarPendente();

            m_Situacao = EnumSituacaoInscricao.Pendente;
        }

        protected abstract void ValidarInscricaoParaSeTornarPendente();

        public virtual void Aceitar()
        {
            if (m_Situacao != EnumSituacaoInscricao.Pendente)
                throw new ExcecaoNegocio("Inscricao", "Só se pode tornar uma inscrição aceita se ela estiver na situação Pendente");

            m_Situacao = EnumSituacaoInscricao.Aceita;
        }

        public virtual void Rejeitar()
        {
            if (m_Situacao != EnumSituacaoInscricao.Pendente)
                throw new ExcecaoNegocio("Inscricao", "Só se pode tornar uma inscrição rejeitada se ela estiver na situação Pendente");

            m_Situacao = EnumSituacaoInscricao.Rejeitada;
        }
    }
}
