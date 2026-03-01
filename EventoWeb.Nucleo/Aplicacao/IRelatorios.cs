using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.IO;

namespace EventoWeb.Nucleo.Aplicacao
{
    public interface IRelatorios
    {
        IRelatorioDivisaoSalasEstudo RelatorioDivisaoSalasEstudo { get; }
        IRelatorioDivisaoOficinas RelatorioDivisaoOficinas { get; }
        IRelatorioDivisaoQuartos RelatorioDivisaoQuartos { get; }
        IRelatorioInscritosDepartamentos RelatorioInscritosDepartamentos { get; }
        IRelatorioSarau RelatorioSarau { get; }
    }

    public interface IRelatorioDivisaoSalasEstudo
    {
        Stream Gerar(IEnumerable<SalaEstudo> salas, IList<AtividadeInscricaoSalaEstudoCoordenacao> coordenadores);
    }

    public interface IRelatorioDivisaoOficinas
    {
        Stream Gerar(IEnumerable<Oficina> oficinas, IList<AtividadeInscricaoOficinasCoordenacao> coordenadores);
    }

    public interface IRelatorioDivisaoQuartos
    {
        Stream Gerar(IEnumerable<Quarto> quartos);
    }

    public interface IRelatorioInscritosDepartamentos
    {
        Stream Gerar(IEnumerable<AtividadeInscricaoDepartamento> inscritos);
    }

    public interface IRelatorioSarau
    {
        Stream Gerar(IList<ApresentacaoSarau> apresentacoes);
    }
}
