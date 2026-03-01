using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioArquivosBinariosNH : AArquivosBinarios
    {
        public RepositorioArquivosBinariosNH(ISession sessao): base(new PersistenciaNH<ArquivoBinario>(sessao))
        {
        }
    }
}
