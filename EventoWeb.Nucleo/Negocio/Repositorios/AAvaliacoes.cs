using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface AAvaliacoes : IPersistencia<Avaliacao>
    {
        IList<Avaliacao> ListarTodas(int idEvento);
        Avaliacao ObterPorId(int idEvento, int id);
    }
}
