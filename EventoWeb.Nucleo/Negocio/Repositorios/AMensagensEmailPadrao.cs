using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AMensagensEmailPadrao : ARepositorio<MensagemEmailPadrao>
    {
        public AMensagensEmailPadrao(IPersistencia<MensagemEmailPadrao> persistencia) : base(persistencia)
        {
        }

        public abstract MensagemEmailPadrao Obter(int idEvento);
    }
}
