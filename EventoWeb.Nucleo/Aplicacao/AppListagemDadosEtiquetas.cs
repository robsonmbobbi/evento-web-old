using EventoWeb.Nucleo.Negocio.Repositorios;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppListagemDadosEtiquetas : AppBase
    {
        public AppListagemDadosEtiquetas(IContexto contexto) : base(contexto)
        {
        }

        public IList<CrachaInscrito> Listar(int idEvento, EnumFiltroCracha filtro)
        {
            IList<CrachaInscrito> lista = new List<CrachaInscrito>();
            ExecutarSeguramente(() =>
            {
                lista = Contexto.RepositorioInscricoes.ListarCrachasInscritosPorEvento(idEvento, filtro);
            });

            return lista;
        }    
    }
}
