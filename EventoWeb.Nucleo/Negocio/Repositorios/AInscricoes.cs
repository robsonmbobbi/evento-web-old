using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public enum EnumTipoBuscaInscricao { Diabeticos, Vegetarianos, UsamAdocante, NaoDormem }

    public class CrachaInscrito
    {
        public int Id { get; set; }
        public String Nome { get; set; }
        public String NomeConhecido { get; set; }
        public String Cidade { get; set; }
        public String UF { get; set; }
        public String Afrac { get; set; }
        public String SalaEstudo { get; set; }
        public String Quarto { get; set; }
        public String Departamento { get; set; }
    }

    public enum EnumFiltroCracha { ParticipantesEPartTrab, ParticipantesEPartTrabETrabalhadores }
    public interface AInscricoes : IPersistencia<Inscricao>
    {
        IList<Inscricao> ListarInscricoesPorEvento(int idEvento, EnumTipoBuscaInscricao tipoBusca);
        IList<Inscricao> ListarInscricoesComPessoasPorEventoENomePessoa(int idEvento, string nome);
        IList<Inscricao> ListarInscricoesParticipanteTrabalhadorPeloNomePessoaPorEvento(int idEvento, String nome);
        IList<InscricaoInfantil> ListarInscricoesInfantisPorEvento(int idEvento);
        IList<Pessoa> ListarPessoasNaoInscritasEventoPeloNomePessoa(int idEvento, string nome);
        Inscricao ObterInscricaoPeloIdEventoEInscricao(int idEvento, int idInscricao);
        Boolean PessoaInscritaEvento(int idEvento, int idPessoa);
        IList<InscricaoInfantil> ListarInscricoesInfantisDoResponsavel(Inscricao inscrito);
        IList<InscricaoParticipante> ListarTodasInscricoesParticipantesComPessoasDoEvento(Evento evento);
        IList<TAtividade> ListarTodasInscricoesAceitasPorAtividade<TAtividade>(Evento evento) where TAtividade : AAtividadeInscricao;
        IList<Inscricao> ListarTodasInscricoesAceitasComPessoasDormemEvento(int idEvento);
        bool HaInscricaoVinculadaTransacao(int idTransacao);
        bool HaInscricaoVinculadaTitulo(int idTitulo);

        Inscricao ObterInscricaoVinculadaTitulo(int idTitulo);
        IList<Inscricao> ListarInscricoesDaPessoaComEvento(int idPessoa);

        IList<CrachaInscrito> ListarCrachasInscritosPorEvento(int idEvento, EnumFiltroCracha filtro);
        Inscricao ObterInscricaoPeloId(int id);
        IList<Inscricao> ListarTodasPorEventoESituacao(int idEvento, EnumSituacaoInscricao situacao);
    }
}
