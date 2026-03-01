using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class InscricoesNH: PersistenciaNH<Inscricao>, AInscricoes
    {
        private ISession mSessao;

        public IResultTransformer Trasformers { get; private set; }

        public InscricoesNH(ISession sessao)
            : base(sessao)
        {
            mSessao = sessao;
        }

        public IList<Inscricao> ListarInscricoesComPessoasPorEventoENomePessoa(int idEvento, string nome)
        {
            return mSessao.QueryOver<Inscricao>()
                .Where(x => x.Evento.Id == idEvento)
                .JoinQueryOver(x => x.Pessoa)
                .Where(y => y.Nome.Upper().IsLike(nome.ToUpper(), MatchMode.Anywhere))                
                .List();
        }

        public IList<Pessoa> ListarPessoasNaoInscritasEventoPeloNomePessoa(int idEvento, string nome)
        {
            var subConsulta = QueryOver.Of<Inscricao>()
                        .Where(inscricao => inscricao.Evento.Id == idEvento)
                        .Select(inscricao => inscricao.Pessoa.Id);

            return mSessao.QueryOver<Pessoa>()
                .Where(pessoa => pessoa.Nome.Upper().IsLike(nome.ToUpper(), MatchMode.Anywhere))
                .WithSubquery.WhereProperty(pessoa => pessoa.Id).NotIn<Inscricao>(subConsulta)
                .List();
        }

        public Inscricao ObterInscricaoPeloId(int id)
        {
            return mSessao
                .QueryOver<Inscricao>()
                .Where(x => x.Id == id)
                .SingleOrDefault();
                
        }

        public Inscricao ObterInscricaoPeloIdEventoEInscricao(int idEvento, int idInscricao)
        {
            return mSessao.QueryOver<Inscricao>()
                .Where(i => i.Evento.Id == idEvento && i.Id == idInscricao)
                .SingleOrDefault();
        }

        public Boolean PessoaInscritaEvento(int idEvento, int idPessoa)
        {
            return mSessao.QueryOver<Inscricao>()
                .Where(inscricao => inscricao.Evento.Id == idEvento &&
                                  inscricao.Pessoa.Id == idPessoa)
                .RowCount() > 0;
        }

        public IList<Inscricao> ListarInscricoesParticipanteTrabalhadorPeloNomePessoaPorEvento(int idEvento, string nome)
        {
            return mSessao.QueryOver<Inscricao>()
                .Where(x => x.Evento.Id == idEvento && x.GetType() != typeof(InscricaoInfantil))
                .JoinQueryOver(x => x.Pessoa)
                .Where(p=>p.Nome.Upper().IsLike(nome.ToUpper(), MatchMode.Anywhere))
                .List();
        }

        public IList<InscricaoInfantil> ListarInscricoesInfantisDoResponsavel(Inscricao inscrito)
        {
            return mSessao.QueryOver<InscricaoInfantil>()
                .Where(i => i.InscricaoResponsavel1.Id == inscrito.Id ||
                    (i.InscricaoResponsavel2 != null && i.InscricaoResponsavel2.Id == inscrito.Id))
                .List();
        }

        public IList<InscricaoParticipante> ListarTodasInscricoesParticipantesComPessoasDoEvento(Evento evento)
        {
            var consulta =  mSessao.QueryOver<InscricaoParticipante>()
                .Where(x => x.Evento.Id == evento.Id)
                .JoinQueryOver(x => x.Pessoa);

            return consulta.List();
        }

        public IList<TAtividade> ListarTodasInscricoesAceitasPorAtividade<TAtividade>(Evento evento) where TAtividade: AAtividadeInscricao
        {
            var consulta = mSessao.QueryOver<TAtividade>()
                .JoinQueryOver(x => x.Inscrito)
                    .Where(x => x.Evento == evento && x.Situacao == EnumSituacaoInscricao.Aceita)
                .JoinQueryOver(y => y.Pessoa);

            return consulta.List();
        }


        public IList<Inscricao> ListarTodasInscricoesAceitasComPessoasDormemEvento(int idEvento)
        {
            return mSessao.QueryOver<Inscricao>()
                .Where(x => x.Evento.Id == idEvento && x.DormeEvento && x.Situacao == EnumSituacaoInscricao.Aceita)
                .JoinQueryOver(x => x.Pessoa)
                .List();
        }


        public bool HaInscricaoVinculadaTransacao(int idTransacao)
        {
            return mSessao.QueryOver<Inscricao>()
                .Where(x => x.TransacaoPagamento != null && x.TransacaoPagamento.Id == idTransacao)
                .RowCount() > 0;
        }

        public bool HaInscricaoVinculadaTitulo(int idTitulo)
        {
            return mSessao.QueryOver<Inscricao>()
                .Where(x => x.TituloPagar != null && x.TituloPagar.Id == idTitulo)
                .RowCount() > 0;
        }

        public Inscricao ObterInscricaoVinculadaTitulo(int idTitulo)
        {
            return mSessao.QueryOver<Inscricao>()
                .Where(x => x.TituloPagar != null && x.TituloPagar.Id == idTitulo)
                .SingleOrDefault();
        }

        public IList<Inscricao> ListarInscricoesDaPessoaComEvento(int idPessoa)
        {
            return mSessao.QueryOver<Inscricao>()
                .Where(x => x.Pessoa.Id == idPessoa)
                .JoinQueryOver(x => x.Evento)
                .List();
        }

        public IList<CrachaInscrito> ListarCrachasInscritosPorEvento(int idEvento, EnumFiltroCracha filtro)
        {
            InscricaoParticipante aliasInscricao = null;
            CrachaInscrito aliasCracha = null;
            Pessoa aliasPessoa = null;
            Oficina aliasAfrac = null;
            SalaEstudo aliasSala = null;

            var subConsultaParticipantesAfrac = QueryOver.Of<Oficina>()
                .Where(x => x.Id == aliasAfrac.Id)
                .JoinQueryOver(x => x.Participantes)
                    .Where(y => y.Id == aliasInscricao.Id)
                .SelectList(lista => lista
                    .Select(x => x.Id));

            var subConsultaCoordenadoresAfrac = QueryOver.Of<AtividadeInscricaoOficinasCoordenacao>()
                .Where(x => x.OficinaEscolhida.Id == aliasAfrac.Id && x.Inscrito.Id == aliasInscricao.Id)
                .SelectList(lista => lista
                    .Select(x => x.Inscrito.Id));

            var subConsultaAfrac = QueryOver.Of<Oficina>(() => aliasAfrac)
                .Where(Restrictions.Conjunction()
                    .Add<Oficina>(x => x.Evento.Id == idEvento)
                    .Add(Restrictions.Disjunction()
                        .Add(Subqueries.WhereExists(subConsultaCoordenadoresAfrac))
                        .Add(Subqueries.WhereExists(subConsultaParticipantesAfrac)
                        )
                    )
                )
                .Select(x => x.Nome);

            var subConsultaParticipantesSalaEstudo = QueryOver.Of<SalaEstudo>()
                .Where(x => x.Id == aliasSala.Id)
                .JoinQueryOver(x => x.Participantes)
                    .Where(y => y.Id == aliasInscricao.Id)
                .SelectList(lista => lista
                    .Select(x => x.Id));

            var subConsultaCoordenadoresSalaEstudo = QueryOver.Of<AtividadeInscricaoSalaEstudoCoordenacao>()
                .Where(x => x.SalaEscolhida.Id == aliasSala.Id && x.Inscrito.Id == aliasInscricao.Id)
                .SelectList(lista => lista
                    .Select(x => x.Inscrito.Id));

            var subConsultaSalaEstudo = QueryOver.Of<SalaEstudo>(() => aliasSala)
                .Where(Restrictions.Conjunction()
                    .Add<SalaEstudo>(x => x.Evento.Id == idEvento)
                    .Add(Restrictions.Disjunction()
                        .Add(Subqueries.WhereExists(subConsultaCoordenadoresSalaEstudo))
                        .Add(Subqueries.WhereExists(subConsultaParticipantesSalaEstudo)
                        )
                    )
                )
                .Select(x => x.Nome);


            var subConsultaQuarto = QueryOver.Of<Quarto>()
                .Where(x => x.Evento.Id == idEvento)
                .JoinQueryOver(x => x.Inscritos)
                    .Where(qi=>qi.Inscricao.Id == aliasInscricao.Id)
                .Select(x => x.Nome);

            var subConsultaDepartamento = QueryOver.Of<Departamento>()
                .Where(Restrictions.Conjunction()
                    .Add<Departamento>(x => x.Evento.Id == idEvento)
                    .Add(Subqueries.WhereProperty<Departamento>(x => x.Id).In(
                        QueryOver.Of<AtividadeInscricaoDepartamento>()
                        .Where(i=>i.Inscrito.Id == aliasInscricao.Id)
                        .Select(i=>i.DepartamentoEscolhido.Id))))
                .Select(x => x.Nome);

            var query = mSessao.QueryOver<InscricaoParticipante>(() => aliasInscricao)
                .Where(x => x.Evento.Id == idEvento);
            if (filtro == EnumFiltroCracha.ParticipantesEPartTrab)
            {
                query.Where(x=> x.Tipo == EnumTipoParticipante.Participante ||
                     x.Tipo == EnumTipoParticipante.ParticipanteTrabalhador);
            }

            return query
            .JoinQueryOver(x => x.Pessoa, () => aliasPessoa)
            .SelectList(lista => lista
                .Select(() => aliasInscricao.Id).WithAlias(() => aliasCracha.Id)
                .SelectSubQuery(subConsultaAfrac).WithAlias(() => aliasCracha.Afrac)
                .SelectSubQuery(subConsultaSalaEstudo).WithAlias(() => aliasCracha.SalaEstudo)
                .SelectSubQuery(subConsultaQuarto).WithAlias(() => aliasCracha.Quarto)
                .SelectSubQuery(subConsultaDepartamento).WithAlias(() => aliasCracha.Departamento)
                .Select(() => aliasPessoa.Nome).WithAlias(() => aliasCracha.Nome)
                .Select(() => aliasInscricao.NomeCracha).WithAlias(() => aliasCracha.NomeConhecido)
                .Select(() => aliasPessoa.Endereco.Cidade).WithAlias(() => aliasCracha.Cidade)
                .Select(() => aliasPessoa.Endereco.UF).WithAlias(() => aliasCracha.UF))
            .TransformUsing(Transformers.AliasToBean<CrachaInscrito>())
            .List<CrachaInscrito>();
        }

        public IList<Inscricao> ListarInscricoesPorEvento(int idEvento, EnumTipoBuscaInscricao tipoBusca)
        {
            var consulta = mSessao.QueryOver<Inscricao>()
                            .Where(x => x.Evento.Id == idEvento);
            var consultaPessoa = consulta.JoinQueryOver(x => x.Pessoa);

            switch (tipoBusca)
            {
                case EnumTipoBuscaInscricao.Diabeticos:
                    consultaPessoa.Where(x => x.EhDiabetico);
                    break;
                case EnumTipoBuscaInscricao.UsamAdocante:
                    consultaPessoa.Where(x => x.UsaAdocanteDiariamente);
                    break;
                case EnumTipoBuscaInscricao.Vegetarianos:
                    consultaPessoa.Where(x => x.EhVegetariano);
                    break;
                case EnumTipoBuscaInscricao.NaoDormem:
                    consulta.Where(x => !x.DormeEvento);
                    break;
            }

            return consulta.List();
        }

        public IList<InscricaoInfantil> ListarInscricoesInfantisPorEvento(int idEvento)
        {
            return mSessao
                .QueryOver<InscricaoInfantil>()
                .Where(x => x.Evento.Id == idEvento)
                .JoinQueryOver(x => x.Pessoa)
                .List();
        }

        public IList<Inscricao> ListarTodasPorEventoESituacao(int idEvento, EnumSituacaoInscricao situacao)
        {
            return mSessao
                .QueryOver<Inscricao>()
                .Where(x => x.Evento.Id == idEvento && x.Situacao == situacao)
                .JoinQueryOver(x => x.Pessoa)
                .List();
        }
    }
}
