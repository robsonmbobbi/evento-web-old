using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class APessoasFinanceiro : ARepositorio<PessoaFinanceiro>
    {
        public APessoasFinanceiro(IPersistencia<PessoaFinanceiro> persistencia) : base(persistencia) { }

        public abstract PessoaFinanceiro ObterPessoaId(int idPessoa);
        public abstract IList<PessoaFinanceiro> ListarTodosFinanceiro(int idInicial, int idFinal, string nome);
        public abstract IList<PessoaComum> ListarTodosGeral(int idInicial, int idFinal, string nome);
        public abstract PessoaComum ObterPessoaComumId(int idPessoa);
    }
}
