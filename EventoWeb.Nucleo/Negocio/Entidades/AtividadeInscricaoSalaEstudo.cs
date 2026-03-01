using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class AtividadeInscricaoSalaEstudo : AAtividadeInscricao
    {
        public AtividadeInscricaoSalaEstudo(InscricaoParticipante inscrito)
            : base(inscrito)
        {
        }

        protected AtividadeInscricaoSalaEstudo() { }
    }

    public class AtividadeInscricaoSalaEstudoCoordenacao : AAtividadeInscricao
    {
        private SalaEstudo m_SalaEscolhida;

        public AtividadeInscricaoSalaEstudoCoordenacao(InscricaoParticipante inscrito, SalaEstudo sala)
            : base(inscrito)
        {
            SalaEscolhida = sala;
        }

        protected AtividadeInscricaoSalaEstudoCoordenacao() { }

        public virtual SalaEstudo SalaEscolhida
        {
            get { return m_SalaEscolhida; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("SalaEscolhida");

                m_SalaEscolhida = value;
            }
        }
    }
}
