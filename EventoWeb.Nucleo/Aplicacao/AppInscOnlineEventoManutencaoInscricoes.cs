using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppInscOnlineEventoManutencaoInscricoes : AppBase
    {
        public AppInscOnlineEventoManutencaoInscricoes(IContexto contexto)
            : base(contexto)
        {
        }

        public DTOInscricaoCompletaAdulto ObterInscricao(int id)
        {
            DTOInscricaoCompletaAdulto dto = null;
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(id);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoInfantil)
                        throw new ExcecaoAplicacao("AppInscOnlineEventoManutencaoInscricoes", "A inscrição não pode ser infantil");

                    var inscParticipante = (InscricaoParticipante)inscricao;
                    dto = inscParticipante.Converter();

                    dto.Evento.Departamentos = Contexto.RepositorioDepartamentos.ListarTodosPorEvento(inscParticipante.Evento.Id)
                        .Select(x => x.Converter())
                        .ToList();
                    dto.Evento.Oficinas = Contexto.RepositorioOficinas.ListarTodasPorEvento(inscParticipante.Evento.Id)
                        .Select(x => x.Converter())
                        .ToList();
                    dto.Evento.SalasEstudo = Contexto.RepositorioSalasEstudo.ListarTodasPorEvento(inscParticipante.Evento.Id)
                        .Select(x => x.Converter())
                        .ToList();

                    dto.Sarais = Contexto.RepositorioApresentacoesSarau.ListarPorInscricao(inscParticipante.Id)
                        .Select(x => x.Converter()).ToList();
                }
            });
            return dto;
        }        

        public DTOInscricaoSimplificada ObterInscricaoAdultoPorCodigo(int idEvento, string codigo)
        {
            DTOInscricaoSimplificada dto = null;
            ExecutarSeguramente(() =>
            {
                var idInscricao = new AppInscOnLineIdentificacaoInscricao().ExtrarId(codigo);

                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(idInscricao);

                if (inscricao != null)
                {
                    if (inscricao.Evento.Id != idEvento)
                        throw new ExcecaoAplicacao("AppInscOnlineEventoManutencaoInscricoes", "Essa inscrição não pertence ao evento escolhido");

                    dto = inscricao.ConverterSimplificada();
                }
            });
            return dto;
        }

        public DTOSarau ObterSarau(int idEvento, string codigo)
        {
            DTOSarau dto = null;
            ExecutarSeguramente(() =>
            {
                var idSarau = new AppInscOnLineIdentificacaoSarau().ExtrarId(codigo);
                var sarau = Contexto.RepositorioApresentacoesSarau.ObterPorId(idEvento, idSarau);
                dto = sarau?.Converter();
            });
            return dto;
        }

        public DTOInscricaoCompletaInfantil ObterInscricaoInfantil(int id)
        {
            DTOInscricaoCompletaInfantil dto = null;
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(id);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoParticipante)
                        throw new ExcecaoAplicacao("AppInscOnlineEventoManutencaoInscricoes", "Inscrição informada não é de uma criança.");

                    dto = ((InscricaoInfantil)inscricao).Converter();

                    dto.Sarais = Contexto.RepositorioApresentacoesSarau.ListarPorInscricao(inscricao.Id)
                        .Select(x => x.Converter()).ToList();
                }
            });
            return dto;
        }
    }
}
