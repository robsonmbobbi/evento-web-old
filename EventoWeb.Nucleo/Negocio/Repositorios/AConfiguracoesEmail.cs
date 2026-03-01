using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AConfiguracoesEmail : ARepositorio<ConfiguracaoEmail>
    {
        public AConfiguracoesEmail(IPersistencia<ConfiguracaoEmail> persistencia) : base(persistencia)
        {
        }

        public abstract ConfiguracaoEmail Obter(int idEvento);
    }
}
