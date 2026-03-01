using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppSalasEstudo : AppBase
    {
        public AppSalasEstudo(IContexto contexto) : base(contexto)
        {
        }

        public IList<DTOSalaEstudo> ObterTodos(int idEvento)
        {
            var lista = new List<DTOSalaEstudo>();
            ExecutarSeguramente(() =>
            {
                var salas = Contexto.RepositorioSalasEstudo.ListarTodasPorEvento(idEvento);
                if (salas.Count > 0)
                    lista.AddRange(salas.Select(x => x.Converter()));
            });

            return lista;
        }

        public DTOSalaEstudo ObterPorId(int idEvento, int id)
        {
            DTOSalaEstudo dto = null;
            ExecutarSeguramente(() =>
            {
                var sala = Contexto.RepositorioSalasEstudo.ObterPorId(idEvento, id);

                if (sala != null)
                    dto = sala.Converter();
            });

            return dto;
        }
        
        public DTOId Incluir(int idEvento, DTOSalaEstudo dto)
        {
            DTOId retorno = new DTOId();

            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
                var sala = new SalaEstudo(evento, dto.Nome);
                sala.DeveSerParNumeroTotalParticipantes = dto.DeveSerParNumeroTotalParticipantes;

                if (dto.IdadeMaxima != null && dto.IdadeMinima != null)
                    sala.FaixaEtaria = new FaixaEtaria(dto.IdadeMinima.Value, dto.IdadeMaxima.Value);
                else if (dto.IdadeMaxima != null || dto.IdadeMinima != null)
                    throw new ExcecaoAplicacao("AppSalasEstudo", "Ao definir a faixa etária, deve-se informar a idade mínima e máxima");

                Contexto.RepositorioSalasEstudo.Incluir(sala);
                retorno.Id = sala.Id;
            });

            return retorno;
        }

        public void Atualizar(int idEvento, int id, DTOSalaEstudo dto)
        {
            ExecutarSeguramente(() =>
            {
                var sala = ObterSalaOuExcecaoSeNaoEncontrar(idEvento, id);
                sala.Nome = dto.Nome;
                sala.DeveSerParNumeroTotalParticipantes = dto.DeveSerParNumeroTotalParticipantes;

                if (dto.IdadeMaxima != null && dto.IdadeMinima != null)
                    sala.FaixaEtaria = new FaixaEtaria(dto.IdadeMinima.Value, dto.IdadeMaxima.Value);
                else if (dto.IdadeMaxima != null || dto.IdadeMinima != null)
                    throw new ExcecaoAplicacao("AppSalasEstudo", "Ao definir a faixa etária, deve-se informar a idade mínima e máxima");

                Contexto.RepositorioSalasEstudo.Atualizar(sala);
            });
        }

        public void Excluir(int idEvento, int id)
        {
            ExecutarSeguramente(() =>
            {
                var sala = ObterSalaOuExcecaoSeNaoEncontrar(idEvento, id);

                Contexto.RepositorioSalasEstudo.Excluir(sala);
            });
        }

        private SalaEstudo ObterSalaOuExcecaoSeNaoEncontrar(int idEvento, int id)
        {
            var sala = Contexto.RepositorioSalasEstudo.ObterPorId(idEvento, id);

            if (sala != null)
                return sala;
            else
                throw new ExcecaoAplicacao("AppSalasEstudo", "Não foi encontrado nenhuma sala de estudo com o id informado.");
        }
    }
}
