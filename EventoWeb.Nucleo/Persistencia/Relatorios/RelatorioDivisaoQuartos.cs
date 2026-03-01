using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventoWeb.Nucleo.Persistencia.Relatorios
{
    public class RelatorioDivisaoQuartos : IRelatorioDivisaoQuartos
    {
        public Stream Gerar(IEnumerable<Quarto> quartos)
        {
            var descricaoQuartos = new Dictionary<Boolean, String>() { { true, "Quarto Família" }, { false, "Quarto Geral" } };

            var lista = new List<DTORelDivisao>();

            foreach (var quarto in quartos)
            {
                string nomesCoordenadores = "";
                foreach (var coordenador in quarto.Inscritos.Where(x => x.EhCoordenador))
                {
                    if (string.IsNullOrEmpty(nomesCoordenadores))
                        nomesCoordenadores = coordenador.Inscricao.Pessoa.Nome;
                    else
                        nomesCoordenadores = nomesCoordenadores + ", " + coordenador.Inscricao.Pessoa.Nome;
                }

                var totalParticipantes = quarto.Inscritos.Count();

                lista.AddRange(
                    quarto.Inscritos
                        .Where(x=> !x.EhCoordenador)
                        .OrderBy(x => x.Inscricao.Pessoa.Nome)
                        .Select(x => new DTORelDivisao
                        {
                            coordenadores = nomesCoordenadores,
                            descricaoDivisao = descricaoQuartos[quarto.EhFamilia] + " - " + quarto.Sexo.ToString(),
                            idDivisao = quarto.Id,
                            nomeDivisao = quarto.Nome,
                            nomeInscrito = x.Inscricao.Pessoa.Nome,
                            totalParticipantes = totalParticipantes
                        })
                        .ToList()
                    );
            }

            return new MemoryStream(new ServicoWebRelatorios().SolicitarRelatorio<List<DTORelDivisao>>(lista, "rel-divisao"));     
        }
    }
}
