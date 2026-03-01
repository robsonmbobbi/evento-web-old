using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppOficinas : AppBase
    {
        public AppOficinas(IContexto contexto) : base(contexto)
        {
        }

        public IList<DTOOficina> ObterTodos(int idEvento)
        {
            var lista = new List<DTOOficina>();
            ExecutarSeguramente(() =>
            {
                var oficinas = Contexto.RepositorioOficinas.ListarTodasPorEvento(idEvento);
                if (oficinas.Count > 0)
                    lista.AddRange(oficinas.Select(x => x.Converter()));
            });

            return lista;
        }

        public DTOOficina ObterPorId(int idEvento, int idOficina)
        {
            DTOOficina dto = null;
            ExecutarSeguramente(() =>
            {
                var oficina = Contexto.RepositorioOficinas.ObterPorId(idEvento, idOficina);

                if (oficina != null)
                    dto = oficina.Converter();
            });

            return dto;
        }

        public DTOId Incluir(int idEvento, DTOOficina dto)
        {
            DTOId retorno = new DTOId();

            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
                var oficina = new Oficina(evento, dto.Nome)
                {
                    DeveSerParNumeroTotalParticipantes = dto.DeveSerParNumeroTotalParticipantes,
                    NumeroTotalParticipantes = dto.NumeroTotalParticipantes
                };

                Contexto.RepositorioOficinas.Incluir(oficina);
                retorno.Id = oficina.Id;
            });

            return retorno;
        }

        public void Atualizar(int idEvento, int idOficina, DTOOficina dto)
        {
            ExecutarSeguramente(() =>
            {
                var oficina = ObterOficinaOuExcecaoSeNaoEncontrar(idEvento, idOficina);
                oficina.Nome = dto.Nome;
                oficina.DeveSerParNumeroTotalParticipantes = dto.DeveSerParNumeroTotalParticipantes;
                oficina.NumeroTotalParticipantes = dto.NumeroTotalParticipantes;

                Contexto.RepositorioOficinas.Atualizar(oficina);
            });
        }

        public void Excluir(int idEvento, int idOficina)
        {
            ExecutarSeguramente(() =>
            {
                var oficina = ObterOficinaOuExcecaoSeNaoEncontrar(idEvento, idOficina);

                Contexto.RepositorioOficinas.Excluir(oficina);
            });
        }

        private Oficina ObterOficinaOuExcecaoSeNaoEncontrar(int idEvento, int idOficina)
        {
            var oficina = Contexto.RepositorioOficinas.ObterPorId(idEvento, idOficina);

            if (oficina != null)
                return oficina;
            else
                throw new ExcecaoAplicacao("AppOficinas", "Não foi encontrada nenhuma oficina com o id informado.");
        }
    }
}
