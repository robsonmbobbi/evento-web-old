using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AEventos : ARepositorio<Evento>
    {
        public AEventos(IPersistencia<Evento> persistencia) : base(persistencia) { }

        public abstract IList<Evento> ObterTodosEventos();
        public abstract Evento ObterEventoPeloId(int id);
        public abstract IList<Evento> ObterTodosEventosEmPeriodoInscricaoOnline(DateTime now);
    }
}
