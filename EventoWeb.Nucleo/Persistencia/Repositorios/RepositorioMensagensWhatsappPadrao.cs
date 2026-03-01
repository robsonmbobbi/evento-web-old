using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioMensagensWhatsappPadrao : AMensagensWhatsappPadrao
    {
        private readonly ISession mSessao;

        public RepositorioMensagensWhatsappPadrao(ISession sessao) : base(new PersistenciaNH<MensagemWhatsappPadrao>(sessao))
        {
            mSessao = sessao;
        }

        public override MensagemWhatsappPadrao Obter(int idEvento)
        {
            return mSessao
                .QueryOver<MensagemWhatsappPadrao>()
                .Where(x => x.Evento.Id == idEvento)
                .SingleOrDefault();
        }
    }
}
