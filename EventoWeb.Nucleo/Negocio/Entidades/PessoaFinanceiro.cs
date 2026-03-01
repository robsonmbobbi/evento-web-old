using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class PessoaFinanceiro: PessoaComum
    {
        public PessoaFinanceiro(String nome)
            : base(nome) { }

        protected PessoaFinanceiro() { }
    }
}
