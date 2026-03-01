using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface ADepartamentos : IPersistencia<Departamento>
    {
        IList<Departamento> ListarTodosPorEvento(int idEvento);
        Departamento ObterPorId(int id);
    }
}
