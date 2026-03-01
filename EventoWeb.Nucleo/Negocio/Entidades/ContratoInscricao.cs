using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class ContratoInscricao : Entidade
    {
        private Evento m_Evento;
        private string m_Regulamento;
        private string m_PassoAPassoInscricao;
        private string m_InstrucoesPagamento;

        public ContratoInscricao(Evento evento, string regulamento, string instrucoesPagamento, string passoAPassoInscricao)
        {
            m_Evento = evento ?? throw new ExcecaoNegocioAtributo("ContratoInscricao", "evento", "Evento não pode ser nulo");
            Regulamento = regulamento;
            InstrucoesPagamento = instrucoesPagamento;
            PassoAPassoInscricao = passoAPassoInscricao;
        }

        protected ContratoInscricao() { }

        public virtual Evento Evento { get => m_Evento; }
        public virtual string Regulamento 
        {
            get => m_Regulamento;
            set
            {
                if (value == null || value.Trim().Length == 0)
                    throw new ExcecaoNegocioAtributo("ContratoInscricao", "Regulamento", "Regulamento não pode ser vazio");
                m_Regulamento = value;
            }
        }
        public virtual string InstrucoesPagamento
        {
            get => m_InstrucoesPagamento;
            set
            {
                if (value == null || value.Trim().Length == 0)
                    throw new ExcecaoNegocioAtributo("ContratoInscricao", "InstrucoesPagamento", "InstrucoesPagamento não pode ser vazio");
                m_InstrucoesPagamento = value;
            }
        }
        public virtual string PassoAPassoInscricao
        {
            get => m_PassoAPassoInscricao;
            set
            {
                if (value == null || value.Trim().Length == 0)
                    throw new ExcecaoNegocioAtributo("ContratoInscricao", "PassoAPassoInscricao", "PassoAPassoInscricao não pode ser vazio");
                m_PassoAPassoInscricao = value;
            }
        }
    }
}
