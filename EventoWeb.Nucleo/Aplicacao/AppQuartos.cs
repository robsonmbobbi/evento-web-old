using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppQuartos : AppBase
    {
        public AppQuartos(IContexto contexto) : base(contexto)
        {
        }

        public IList<DTOQuarto> ObterTodos(int idEvento)
        {
            var lista = new List<DTOQuarto>();
            ExecutarSeguramente(() =>
            {
                var quartos = Contexto.RepositorioQuartos.ListarTodosQuartosPorEvento(idEvento);
                if (quartos.Count > 0)
                    lista.AddRange(quartos.Select(x => x.Converter()));
            });

            return lista;
        }

        public DTOQuarto ObterPorId(int idEvento, int id)
        {
            DTOQuarto dto = null;
            ExecutarSeguramente(() =>
            {
                var quarto = Contexto.RepositorioQuartos.ObterQuartoPorIdEventoEQuarto(idEvento, id);

                if (quarto != null)
                    dto = quarto.Converter();
            });

            return dto;
        }

        public DTOId Incluir(int idEvento, DTOQuarto dto)
        {
            DTOId retorno = new DTOId();

            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
                var quarto = new Quarto(evento, dto.Nome, dto.EhFamilia, dto.Sexo)
                {
                    Capacidade = dto.Capacidade,                    
                };

                Contexto.RepositorioQuartos.Incluir(quarto);
                retorno.Id = quarto.Id;
            });

            return retorno;
        }

        public void Atualizar(int idEvento, int idQuarto, DTOQuarto dto)
        {
            ExecutarSeguramente(() =>
            {
                var quarto = ObterOficinaOuExcecaoSeNaoEncontrar(idEvento, idQuarto);
                quarto.Nome = dto.Nome;
                quarto.Capacidade = dto.Capacidade;
                quarto.AtribuirSexoEEhFamilia(dto.EhFamilia, dto.Sexo);

                Contexto.RepositorioQuartos.Atualizar(quarto);
            });
        }

        public void Excluir(int idEvento, int id)
        {
            ExecutarSeguramente(() =>
            {
                var quarto = ObterOficinaOuExcecaoSeNaoEncontrar(idEvento, id);

                Contexto.RepositorioQuartos.Excluir(quarto);
            });
        }

        private Quarto ObterOficinaOuExcecaoSeNaoEncontrar(int idEvento, int id)
        {
            var quarto = Contexto.RepositorioQuartos.ObterQuartoPorIdEventoEQuarto(idEvento, id);

            if (quarto != null)
                return quarto;
            else
                throw new ExcecaoAplicacao("AppQuartos", "Não foi encontrado nenhum quarto com o id informado.");
        }
    }
}
