using EventoWeb.Nucleo.Negocio.Entidades;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AConfiguracoesWhatsapp : ARepositorio<ConfiguracaoWhatsapp>
    {
        public AConfiguracoesWhatsapp(IPersistencia<ConfiguracaoWhatsapp> persistencia) : base(persistencia)
        {
        }

        public abstract ConfiguracaoWhatsapp Obter(int idEvento);
    }
}
