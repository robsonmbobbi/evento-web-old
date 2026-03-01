using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppApresentacaoSarau : AppBase
    {
        public AppApresentacaoSarau(IContexto contexto)
            : base(contexto) { }

        public IList<DTOSarau> Listar(int idEvento)
        {
            var lista = new List<DTOSarau>();
            ExecutarSeguramente(() =>
            {
                var sarais = Contexto.RepositorioApresentacoesSarau.ListarTodas(idEvento);
                if (sarais.Count > 0)
                    lista.AddRange(sarais.Select(x => x.Converter()));
            });

            return lista;
        }

        public DTOSarau Obter(int idEvento, int idSarau)
        {
            DTOSarau dto = null;
            ExecutarSeguramente(() =>
            {
                var sarau = Contexto.RepositorioApresentacoesSarau.ObterPorId(idEvento, idSarau);

                if (sarau != null)
                    dto = sarau.Converter();
            });

            return dto;
        }

        public DTOId Incluir(int idEvento, DTOSarau dto)
        {
            DTOId retorno = new DTOId();
            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);

                var inscritos = dto
                    .Participantes
                    .Select(x => Contexto.RepositorioInscricoes
                        .ObterInscricaoPeloIdEventoEInscricao(idEvento, x.Id))
                    .ToList();

                var sarau = new ApresentacaoSarau(evento, dto.DuracaoMin, dto.Tipo, inscritos);

                Contexto.RepositorioApresentacoesSarau.Incluir(sarau);

                retorno.Id = sarau.Id;
            });

            return retorno;
        }

        public void Atualizar(int idEvento, int idSarau, DTOSarau dto)
        {
            ExecutarSeguramente(() =>
            {
                var sarau = ObterSarauOuExcecaoSeNaoEncontrar(idEvento, idSarau);

                sarau.DuracaoMin = dto.DuracaoMin;
                sarau.Tipo = dto.Tipo;
                sarau.AtualizarInscricoes(
                    dto
                        .Participantes
                        .Select(x => Contexto.RepositorioInscricoes
                            .ObterInscricaoPeloIdEventoEInscricao(idEvento, x.Id))
                        .ToList());

                Contexto.RepositorioApresentacoesSarau.Atualizar(sarau);
            });
        }

        public void Excluir(int idEvento, int idSarau)
        {
            ExecutarSeguramente(() =>
            {
                var sarau = ObterSarauOuExcecaoSeNaoEncontrar(idEvento, idSarau);

                Contexto.RepositorioApresentacoesSarau.Excluir(sarau);
            });
        }

        public Stream GerarImpressoPDF(int idEvento)
        {
            Stream relatorio = new MemoryStream();
            ExecutarSeguramente(() =>
            {
                relatorio = Contexto.RelatorioSarau.Gerar(Contexto.RepositorioApresentacoesSarau.ListarTodas(idEvento));
            });

            return relatorio;
        }

        private ApresentacaoSarau ObterSarauOuExcecaoSeNaoEncontrar(int idEvento, int id)
        {
            var sarau = Contexto.RepositorioApresentacoesSarau.ObterPorId(idEvento, id);

            if (sarau != null)
                return sarau;
            else
                throw new ExcecaoAplicacao("AppApresentacaoSarau", "Não foi encontrado nenhum sarau com o id informado.");
        }

        internal void IncluirOuAtualizarPorParticipanteSemExecucaoSegura(Inscricao inscricao, IEnumerable<DTOSarau> dtoApresentacoes)
        {
            var repApresentacoes = Contexto.RepositorioApresentacoesSarau;
            var apresentacoes = repApresentacoes.ListarPorInscricao(inscricao.Id);
            var apresentacoesRemovidas = apresentacoes.Where(x => dtoApresentacoes.Count(y => y.Id == x.Id) == 0).ToList();
            foreach (var apresentacao in apresentacoesRemovidas)
            {
                if (apresentacao.Inscritos.Count() == 1)
                    repApresentacoes.Excluir(apresentacao);
                else
                {
                    var inscritos = new List<Inscricao>(apresentacao.Inscritos);
                    inscritos.Remove(inscricao);

                    apresentacao.AtualizarInscricoes(inscritos);

                    repApresentacoes.Atualizar(apresentacao);
                }
            }

            foreach (var dtoSarau in dtoApresentacoes)
            {
                if (dtoSarau.Id != null)
                {
                    var apresentacao = repApresentacoes.ObterPorId(inscricao.Evento.Id, dtoSarau.Id.Value);
                    apresentacao.DuracaoMin = dtoSarau.DuracaoMin;
                    apresentacao.Tipo = dtoSarau.Tipo;
                    if (apresentacao.Inscritos.Count(x => x == inscricao) == 0)
                    {
                        var inscritos = new List<Inscricao>(apresentacao.Inscritos)
                        {
                            inscricao
                        };

                        apresentacao.AtualizarInscricoes(inscritos);
                    }

                    repApresentacoes.Atualizar(apresentacao);
                }
                else
                {
                    var apresentacao = new ApresentacaoSarau(inscricao.Evento, dtoSarau.DuracaoMin, dtoSarau.Tipo,
                        new List<Inscricao>() { inscricao });
                    repApresentacoes.Incluir(apresentacao);
                }
            }
        }
    }
}
