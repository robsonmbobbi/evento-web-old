using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventoWeb.Nucleo.Persistencia.Relatorios
{
    public class RelatorioDivisaoOficinas : IRelatorioDivisaoOficinas
    {
        public Stream Gerar(IEnumerable<Oficina> oficinas, IList<AtividadeInscricaoOficinasCoordenacao> coordenadores)
        {
            var lista = new List<DTORelDivisao>();

            foreach (var oficina in oficinas)
            {
                string nomesCoordenadores = "";
                foreach (var coordenador in coordenadores.Where(x => x.OficinaEscolhida == oficina))
                {
                    if (string.IsNullOrEmpty(nomesCoordenadores))
                        nomesCoordenadores = coordenador.Inscrito.Pessoa.Nome;
                    else
                        nomesCoordenadores = nomesCoordenadores + ", " + coordenador.Inscrito.Pessoa.Nome;
                }

                var totalParticipantes = oficina.Participantes.Count();

                lista.AddRange(
                    oficina.Participantes.OrderBy(x => x.Pessoa.Nome)
                        .Select(x => new DTORelDivisao
                        {
                            coordenadores = nomesCoordenadores,
                            descricaoDivisao = "Oficinas",
                            idDivisao = oficina.Id,
                            nomeDivisao = oficina.Nome,
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
