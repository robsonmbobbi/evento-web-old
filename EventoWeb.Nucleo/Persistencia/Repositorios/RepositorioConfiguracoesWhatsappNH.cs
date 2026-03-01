using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioConfiguracoesWhatsappNH : AConfiguracoesWhatsapp
    {
        private ISession mSessao;

        public RepositorioConfiguracoesWhatsappNH(ISession sessao) : base(new PersistenciaNH<ConfiguracaoWhatsapp>(sessao))
        {
            mSessao = sessao;
        }

        public override ConfiguracaoWhatsapp Obter(int idEvento)
        {
            var lista = mSessao
                .QueryOver<ConfiguracaoWhatsapp>()
                .Where(x => x.Evento.Id == idEvento)
                .List();

            if (lista.Count == 0)
                return null;
            else
                return lista[0];
        }
    }
}
