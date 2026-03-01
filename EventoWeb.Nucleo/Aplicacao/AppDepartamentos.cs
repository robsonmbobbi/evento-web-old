using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppDepartamentos : AppBase
    {
        public AppDepartamentos(IContexto contexto) : base(contexto)
        {
        }

        public IList<DTODepartamento> ObterTodos(int idEvento)
        {
            var lista = new List<DTODepartamento>();
            ExecutarSeguramente(() =>
            {
                var departamentos = Contexto.RepositorioDepartamentos.ListarTodosPorEvento(idEvento);
                if (departamentos.Count > 0)
                    lista.AddRange(departamentos.Select(x => x.Converter()));
            });

            return lista;
        }

        public DTODepartamento ObterPorId(int id)
        {
            DTODepartamento dto = null;
            ExecutarSeguramente(() =>
            {
                var departamento = Contexto.RepositorioDepartamentos.ObterPorId(id);

                if (departamento != null)
                    dto = departamento.Converter();
            });

            return dto;
        }
        
        public DTOId Incluir(int idEvento, string nome)
        {
            DTOId retorno = new DTOId();

            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
                var departamento = new Departamento(evento, nome);

                Contexto.RepositorioDepartamentos.Incluir(departamento);
                retorno.Id = departamento.Id;
            });

            return retorno;
        }

        public void Atualizar(int id, string nome)
        {
            ExecutarSeguramente(() =>
            {
                var departamento = ObterDepartamentoOuExcecaoSeNaoEncontrar(id);
                departamento.Nome = nome;

                Contexto.RepositorioDepartamentos.Atualizar(departamento);                
            });
        }

        public void Excluir(int id)
        {
            ExecutarSeguramente(() =>
            {
                var departamento = ObterDepartamentoOuExcecaoSeNaoEncontrar(id);

                Contexto.RepositorioDepartamentos.Excluir(departamento);
            });
        }

        private Departamento ObterDepartamentoOuExcecaoSeNaoEncontrar(int id)
        {
            var departamento = Contexto.RepositorioDepartamentos.ObterPorId(id);

            if (departamento != null)
                return departamento;
            else
                throw new ExcecaoAplicacao("AppDepartamentos", "Não foi encontrado nenhum departamento com o id informado.");
        }

        public Stream GerarImpressoPDFInscritos(int idEvento)
        {
            Stream relatorio = new MemoryStream();
            ExecutarSeguramente(() =>
            {
                var inscricoes = Contexto.RepositorioInscricoes;
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);

                relatorio = Contexto.RelatorioInscritosDepartamentos.Gerar(
                    inscricoes.ListarTodasInscricoesAceitasPorAtividade<AtividadeInscricaoDepartamento>(evento));
            });

            return relatorio;
        }
    }
}
