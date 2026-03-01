using EventoWeb.Nucleo.Negocio.Excecoes;
using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Endereco
    {
        private string m_Cidade;
        private string m_UF;

        public Endereco(string cidade, string uf)
        {
            Cidade = cidade;
            UF = uf;
        }

        protected Endereco() { }

        public virtual string Cidade 
        {
            get { return m_Cidade; }
            set
            {
                ValidarSeValorNuloOuVazio(value, "Cidade");
                m_Cidade = value;
            }
        }

        public virtual string UF 
        {
            get { return m_UF; }
            set
            {
                ValidarSeValorNuloOuVazio(value, "UF");
                m_UF = value;
            }
        }

        private void ValidarSeValorNuloOuVazio(String valor, String nomeCampo)
        {
            if (String.IsNullOrEmpty(valor))
                throw new ExcecaoNegocioAtributo("Endereco", nomeCampo, nomeCampo + " não foi informado");
        }
    }
}
