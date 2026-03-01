using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class AtividadeInscricaoDepartamento: AAtividadeInscricao
    {
        private Departamento m_DepartamentoEscolhido;

        public AtividadeInscricaoDepartamento(InscricaoParticipante inscrito, Departamento departamentoEscolhido)
            : base(inscrito)
        {
            DepartamentoEscolhido = departamentoEscolhido;
            EhCoordenacao = false;
        }

        protected AtividadeInscricaoDepartamento() { }

        public virtual Departamento DepartamentoEscolhido
        {
            get { return m_DepartamentoEscolhido; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("DepartamentoEscolhido");

                m_DepartamentoEscolhido = value;
            }
        }

        public virtual Boolean EhCoordenacao { get; set; }
    }
}
