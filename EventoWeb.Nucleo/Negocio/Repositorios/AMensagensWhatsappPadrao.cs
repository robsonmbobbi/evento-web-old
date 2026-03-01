using EventoWeb.Nucleo.Negocio.Entidades;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AMensagensWhatsappPadrao : ARepositorio<MensagemWhatsappPadrao>
    {
        public AMensagensWhatsappPadrao(IPersistencia<MensagemWhatsappPadrao> persistencia) : base(persistencia)
        {
        }

        public abstract MensagemWhatsappPadrao Obter(int idEvento);
    }
}
