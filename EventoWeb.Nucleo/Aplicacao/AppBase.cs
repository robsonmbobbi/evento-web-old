using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public abstract class AppBase
    {
        public AppBase(IContexto contexto)
        {
            Contexto = contexto;
        }

        public IContexto Contexto { get; }

        protected virtual void ExecutarSeguramente(Action acaoExecutar)
        {
            try
            {
                Contexto.IniciarTransacao();

                acaoExecutar();

                Contexto.SalvarTransacao();
            }
            catch (Exception ex)
            {
                Contexto.CancelarTransacao();

                throw ex;
            }
        }
    }
}
