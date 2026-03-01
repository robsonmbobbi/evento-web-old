using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventoWeb.Nucleo.Persistencia.Relatorios
{
    public class RelatorioDivisaoSalasEstudo : IRelatorioDivisaoSalasEstudo
    {
        public Stream Gerar(IEnumerable<SalaEstudo> salas, IList<AtividadeInscricaoSalaEstudoCoordenacao> coordenadores)
        {
            var lista = new List<DTORelDivisao>();

            foreach (var sala in salas)
            {
                string nomesCoordenadores = "";
                foreach (var coordenador in coordenadores.Where(x => x.SalaEscolhida == sala))
                {
                    if (string.IsNullOrEmpty(nomesCoordenadores))
                        nomesCoordenadores = coordenador.Inscrito.Pessoa.Nome;
                    else
                        nomesCoordenadores = nomesCoordenadores + ", " + coordenador.Inscrito.Pessoa.Nome;
                }

                var totalParticipantes = sala.Participantes.Count();

                lista.AddRange(
                    sala.Participantes.OrderBy(x => x.Pessoa.Nome)
                        .Select(x => new DTORelDivisao
                        {
                            coordenadores = nomesCoordenadores,
                            descricaoDivisao = "Salas Estudo",
                            idDivisao = sala.Id,
                            nomeDivisao = sala.Nome,
                            nomeInscrito = x.Pessoa.Nome,
                            totalParticipantes = totalParticipantes
                        })
                        .ToList()
                    );
            }

            return new MemoryStream(new ServicoWebRelatorios().SolicitarRelatorio<List<DTORelDivisao>>(lista, "rel-divisao"));
        }
    }
}
