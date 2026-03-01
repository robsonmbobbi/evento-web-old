using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventoWeb.Nucleo.Persistencia.Relatorios
{
    public class RelatorioInscritosDepartamentos : IRelatorioInscritosDepartamentos
    {
        public Stream Gerar(IEnumerable<AtividadeInscricaoDepartamento> inscritos)
        {
            var lista = new List<DTORelDivisao>();

            foreach (var departamento in inscritos.GroupBy(x => x.DepartamentoEscolhido))
            {
                string nomesCoordenadores = "";
                foreach (var coordenador in departamento.Where(x => x.EhCoordenacao))
                {
                    if (string.IsNullOrEmpty(nomesCoordenadores))
                        nomesCoordenadores = coordenador.Inscrito.Pessoa.Nome;
                    else
                        nomesCoordenadores = nomesCoordenadores + ", " + coordenador.Inscrito.Pessoa.Nome;
                }

                var totalParticipantes = departamento.Count();

                lista.AddRange(
                    departamento
                        .Where(x => !x.EhCoordenacao)
                        .OrderBy(x => x.Inscrito.Pessoa.Nome)
                        .Select(x => new DTORelDivisao
                        {
                            coordenadores = nomesCoordenadores,
                            descricaoDivisao = "Departamentos Doutrinários",
                            idDivisao = departamento.Key.Id,
                            nomeDivisao = departamento.Key.Nome,
                            nomeInscrito = x.Inscrito.Pessoa.Nome,
                            totalParticipantes = totalParticipantes
                        })
                        .ToList()
                    );
            }

            return new MemoryStream(new ServicoWebRelatorios().SolicitarRelatorio<List<DTORelDivisao>>(lista, "rel-divisao"));
        }
    }
}
