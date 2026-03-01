using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Excecoes;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AOficinas : ARepositorio<Oficina>
    {
        public AOficinas(IPersistencia<Oficina> persistencia) : base(persistencia)
        {
        }

        public abstract IList<Oficina> ListarTodasPorEvento(int idEvento);
        public abstract Oficina ObterPorId(int idEvento, int idOficina);
        public abstract IList<Oficina> ListarTodasComParticipantesPorEvento(Evento evento);
        public abstract IList<InscricaoParticipante> ListarParticipantesSemOficinaNoEvento(Evento evento);
        public abstract bool InscritoEhResponsavelPorOficina(Evento evento, InscricaoParticipante inscParticipante);

        public abstract bool HaAOficinasSemResponsavelDefinidoDoEvento(Evento evento);

        public abstract bool EhParticipanteDeOficinaNoEvento(Evento evento, InscricaoParticipante participante);
        protected abstract bool HaParticipatesOuResponsaveisEmOutraOficina(Oficina oficina);

        public abstract Oficina BuscarOficinaDoInscrito(int idEvento, int idInscricao);
        public abstract int ContarTotalOficinas(Evento mEvento);

        public override void Atualizar(Oficina objeto)
        {
            ValidarPessoasEmOutrasOficinas(objeto);
            base.Atualizar(objeto);
        }

        public override void Incluir(Oficina objeto)
        {
            ValidarPessoasEmOutrasOficinas(objeto);
            base.Incluir(objeto);
        }

        private void ValidarPessoasEmOutrasOficinas(Oficina oficina)
        {
            if (HaParticipatesOuResponsaveisEmOutraOficina(oficina))
                throw new ExcecaoNegocioRepositorio("AOficinas", "Existem participantes nesta oficina, como responsáveis ou participantes, que estão em outra oficina.");
        }
    }
}
