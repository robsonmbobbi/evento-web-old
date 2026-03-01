using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class APessoas: ARepositorio<Pessoa>
    {
        public APessoas(IPersistencia<Pessoa> persistencia) : base(persistencia) { }

        public abstract IList<Pessoa> ListarPessoasPeloNome(string nome);
        public abstract Pessoa ObterPessoaId(int idPessoa);
    }
}
