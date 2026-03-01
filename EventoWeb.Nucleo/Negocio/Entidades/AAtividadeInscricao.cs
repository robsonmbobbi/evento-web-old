using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public abstract class AAtividadeInscricao: Entidade
    {
        private InscricaoParticipante m_Inscrito;

        public AAtividadeInscricao(InscricaoParticipante inscrito)
        {
            if (inscrito == null)
                throw new ArgumentNullException("inscrito");
            m_Inscrito = inscrito;
        }

        protected AAtividadeInscricao() { }

        public virtual InscricaoParticipante Inscrito { get { return m_Inscrito; } }
    }
}
