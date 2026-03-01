using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.IO;

namespace EventoWeb.Nucleo.Persistencia.Relatorios
{
    public class RelatorioSarau : IRelatorioSarau
    {
        public Stream Gerar(IList<ApresentacaoSarau> apresentacoes)
        {
            var lista = new List<DTORelApresentacaoSarau>();

            foreach (var apresentacao in apresentacoes)
            {
                string inscritos = "";
                foreach (var inscrito in apresentacao.Inscritos)
                {
                    if (string.IsNullOrEmpty(inscritos))
                        inscritos = inscrito.Pessoa.Nome;
                    else
                        inscritos = inscritos + ", " + inscrito.Pessoa.Nome;
                }

                lista.Add(
                    new DTORelApresentacaoSarau
                    {
                        duracaoApresentacaoMin = apresentacao.DuracaoMin,
                        idApresentacao = apresentacao.Id,
                        idEvento = apresentacao.Evento.Id,
                        inscritos = inscritos,
                        nomeEvento = apresentacao.Evento.Nome,
                        tempoSarauMin = apresentacao.Evento.ConfiguracaoTempoSarauMin.Value,
                        tipoApresentacao = apresentacao.Tipo
                    });                
            }

            return new MemoryStream(new ServicoWebRelatorios().SolicitarRelatorio<List<DTORelApresentacaoSarau>>(lista, "rel-sarau"));
        }
    }    
}
