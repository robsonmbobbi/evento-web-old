using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Departamento: Entidade
    {
        private Evento m_Evento;
        private string m_Nome;

        public Departamento(Evento evento, String nome)
        {
            Evento = evento;

            Nome = nome;
        }

        protected Departamento() { }

        public virtual Evento Evento
        {
            get { return m_Evento; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("Evento", "Evento não pode ser nulo.");

                if (!value.TemDepartamentalizacao)
                    throw new InvalidOperationException("Este evento não está configurado para ter departamentos.");

                m_Evento = value;
            }
        }

        public virtual String Nome
        {
            get { return m_Nome; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Nome");

                m_Nome = value;
            }
        }
    }
}
