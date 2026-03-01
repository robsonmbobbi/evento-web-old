using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class SalaEstudo: Entidade
    {
        private Evento m_Evento;
        private string m_Nome;
        private IList<InscricaoParticipante> m_Participantes;
        private FaixaEtaria m_FaixaEtaria;
        private bool m_DeveSerParNumeroTotalParticipantes;

        public SalaEstudo(Evento evento, string nome)
        {
            m_Participantes = new List<InscricaoParticipante>();

            if (evento == null)
                throw new ArgumentNullException("evento", "Evento não pode ser nulo.");

            if (evento.ConfiguracaoSalaEstudo == null)
                throw new InvalidOperationException("Este evento não está configurado para ter Salas de Estudo.");


            m_Evento = evento;
            Nome = nome;
        }

        protected SalaEstudo() { }

        public virtual Evento Evento { get { return m_Evento; } }

        public virtual string Nome 
        {
            get { return m_Nome; }
            set 
            { 
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException("Nome","O nome não pode ser vazio.");

                m_Nome = value; 
            }
        }

        public virtual IEnumerable<InscricaoParticipante> Participantes { get { return m_Participantes; } }

        public virtual FaixaEtaria FaixaEtaria
        {
            get { return m_FaixaEtaria; }
            set
            {
                if (m_Evento.ConfiguracaoSalaEstudo.Value == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao)
                    throw new ArgumentException("O modelo de divisão da salas de estudo não permite o uso da faixa etária.", "FaixaEtaria");

                m_FaixaEtaria = value;
            }
        }

        public virtual bool DeveSerParNumeroTotalParticipantes
        {
            get => m_DeveSerParNumeroTotalParticipantes; 
            set => m_DeveSerParNumeroTotalParticipantes = value;
        }

        public virtual void AdicionarParticipante(InscricaoParticipante participante)
        {
            ValidarSeParticipanteEhNulo(participante);
            ValidarSeParticipanteEhMesmoEvento(participante);

            if (m_Evento.ConfiguracaoSalaEstudo.Value == EnumModeloDivisaoSalasEstudo.PorIdadeCidade &&
                m_FaixaEtaria != null &&
                (participante.Pessoa.CalcularIdadeEmAnos(m_Evento.PeriodoRealizacaoEvento.DataInicial) < m_FaixaEtaria.IdadeMin ||
                participante.Pessoa.CalcularIdadeEmAnos(m_Evento.PeriodoRealizacaoEvento.DataInicial) > m_FaixaEtaria.IdadeMax))
                throw new ArgumentException("Participante fora da faixa etária definida para esta sala.");

            if (!EstaNaListaDeParticipantes(participante))
                m_Participantes.Add(participante);
        }

        public virtual void RemoverParticipante(InscricaoParticipante participante)
        {
            ValidarSeParticipanteEhNulo(participante);

            if (!EstaNaListaDeParticipantes(participante))
                throw new ArgumentException("Participante não existe na lista de participantes desta sala.", "participante");

            m_Participantes.Remove(participante);
        }

        public virtual void RemoverTodosParticipantes()
        {
            m_Participantes.Clear();
        }

        public virtual bool EstaNaListaDeParticipantes(InscricaoParticipante participante)
        {
            return m_Participantes.Where(x => x == participante).Count() > 0;
        }

        private void ValidarSeParticipanteEhNulo(InscricaoParticipante participante)
        {
            if (participante == null)
                throw new ArgumentNullException("participante", "Participante não pode ser nulo.");
        }

        private void ValidarSeParticipanteEhMesmoEvento(InscricaoParticipante participante)
        {
            if (participante.Evento != m_Evento)
                throw new ArgumentException("participante", "Participante deve ser do mesmo evento da sala.");
        }
    }
}
