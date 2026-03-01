using System;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class ApresentacaoSarau : Entidade
    {
        private Evento m_Evento;
        private string m_Tipo;
        private int m_DuracaoMin;
        private IList<Inscricao> m_Inscritos;

        public ApresentacaoSarau(Evento evento, int duracaoMin, string tipo, IEnumerable<Inscricao> inscritos)
        {
            Evento = evento;
            DuracaoMin = duracaoMin;
            Tipo = tipo;

            m_Inscritos = new List<Inscricao>();
            AtualizarInscricoes(inscritos);
        }

        protected ApresentacaoSarau() { }        

        public virtual Evento Evento
        {
            get { return m_Evento; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("Evento", "Evento não pode ser nulo.");

                if (value.ConfiguracaoTempoSarauMin == null)
                    throw new InvalidOperationException("Este evento não está configurado para ter Sarau.");

                m_Evento = value;
            }
        }

        public virtual String Tipo
        {
            get { return m_Tipo; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Tipo", "O nome não pode ser nulo.");

                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Tipo não pode ser vazio.");

                m_Tipo = value;
            }
        }

        public virtual IEnumerable<Inscricao> Inscritos { get { return m_Inscritos; } }

        public virtual int DuracaoMin
        {
            get { return m_DuracaoMin; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("A Duração deve ser dada em minutos e o seu valor deve ser maior que zero.");

                m_DuracaoMin = value;
            }
        }

        public virtual void AdicionarInscricao(Inscricao inscricao)
        {
            if (inscricao == null)
                throw new ArgumentNullException("inscricao", "Inscrição não pode ser nula.");

            if (m_Inscritos.FirstOrDefault(x => x == inscricao) != null)
                throw new ArgumentException("Esta inscrição já consta nesta apresentação.");

            m_Inscritos.Add(inscricao);
        }

        public virtual void AtualizarInscricoes(IEnumerable<Inscricao> inscricoes)
        {
            if (inscricoes == null || inscricoes.Count() == 0)
                throw new ArgumentException("É preciso de pelo menos uma inscrição.", "inscritos");

            if (inscricoes.Count(x=> x == null) > 0)
                throw new ArgumentException("Há na lista inscrições nulas.", "inscritos");

            if (inscricoes != null && inscricoes.GroupBy(x => x.Id).Count(y => y.Count() > 1) > 0)
                throw new ArgumentException("Há inscrições mais de uma vez na lista.");

            m_Inscritos.Clear();
            foreach (var inscricao in inscricoes)
                m_Inscritos.Add(inscricao);
        }
    }
}
