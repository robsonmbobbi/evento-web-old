using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public abstract class PessoaComum: Entidade
    {
        private string m_Nome;
        public PessoaComum(string nome)
        {
            Nome = nome;
        }

        protected PessoaComum() { }

        public virtual string Nome
        {
            get
            {
                return m_Nome;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ExcecaoNegocioAtributo("PessoaComum", "Nome", "Nome esta vazio");

                m_Nome = value;
            }
        }

        public virtual string Celular { get; set; }

        public virtual string Email { get; set; }
    }
}
