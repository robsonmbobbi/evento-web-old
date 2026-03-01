using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class ACodigosAcessoInscricao : ARepositorio<CodigoAcessoInscricao>
    {
        public ACodigosAcessoInscricao(IPersistencia<CodigoAcessoInscricao> persistencia) : base(persistencia)
        {
        }

        public abstract void ExcluirCodigosVencidos();

        public abstract CodigoAcessoInscricao ObterPeloCodigo(string codigo);

        public abstract CodigoAcessoInscricao ObterIdInscricao(int idInscricao);
    }
}
