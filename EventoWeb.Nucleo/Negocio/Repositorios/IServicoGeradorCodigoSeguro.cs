using System;
using System.Collections.Generic;
using System.Text;
using EventoWeb.Nucleo.Negocio.Entidades;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface IServicoGeradorCodigoSeguro
    {
        string GerarCodigo5Caracteres();
    }
}
