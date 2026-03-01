using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AArquivosBinarios : ARepositorio<ArquivoBinario>
    {
        public AArquivosBinarios(IPersistencia<ArquivoBinario> persistencia) : base(persistencia) { }
    }
}
