using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using EventoWeb.Nucleo.Persistencia.Repositorios;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Persistencia
{
    public class RepositorioOficinasNH: AOficinas
    {
        private ISession mSessao;

        public RepositorioOficinasNH(ISession sessao)
            : base(new PersistenciaNH<Oficina>(sessao))
        {
            mSessao = sessao;
        }

        public override IList<Oficina> ListarTodasPorEvento(int idEvento)
        {
            return mSessao.QueryOver<Oficina>()
                .Where(afrac => afrac.Evento.Id == idEvento)
                .List();
        }

        public override Oficina ObterPorId(int idEvento, int idOficina)
        {
            return mSessao.QueryOver<Oficina>()
                .Where(afrac => afrac.Id == idOficina && afrac.Evento.Id == idEvento)
                .SingleOrDefault();
        }

        public override bool InscritoEhResponsavelPorOficina(Evento evento, InscricaoParticipante inscParticipante)
        {
            return mSessao.QueryOver<AtividadeInscricaoOficinasCoordenacao>()
                .Where(x => x.Inscrito.Id == inscParticipante.Id)
                .JoinQueryOver(x => x.Inscrito)
                    .JoinQueryOver(y => y.Evento)
                        .Where(y => y.Id == evento.Id)
                .RowCount() > 0;
        }

        public override IList<Oficina> ListarTodasComParticipantesPorEvento(Evento evento)
        {
            var consulta = mSessao.QueryOver<Oficina>()
                .Where(afrac => afrac.Evento == evento)
                .Future();

            mSessao.QueryOver<Oficina>()
                .Where(afrac => afrac.Evento == evento)
                .Left.JoinQueryOver(x => x.Participantes)
                .Left.JoinQueryOver(y=>y.Pessoa)
                .Future();

            return consulta.ToList();
        }

        public override IList<InscricaoParticipante> ListarParticipantesSemOficinaNoEvento(Evento evento)
        {
            InscricaoParticipante aliasParticipante = null;

            if (evento.ConfiguracaoOficinas == EnumModeloDivisaoOficinas.PorOrdemEscolhaInscricao)
            {
                AtividadeInscricaoOficinas aliasAtividade = null;

                var subQueryParticipantes = QueryOver.Of<Oficina>()
                    .JoinQueryOver<InscricaoParticipante>(x => x.Participantes, () => aliasParticipante)
                    .Where(x => x.Id == aliasAtividade.Inscrito.Id && x.Situacao == EnumSituacaoInscricao.Aceita)
                    .SelectList(x => x.Select(() => aliasParticipante.Id));

                var subQueryCoordenadores = QueryOver.Of<AtividadeInscricaoOficinasCoordenacao>()
                    .Where(x => x.Inscrito.Id == aliasAtividade.Id)
                    .Select(x => x.Inscrito.Id);

                return mSessao.QueryOver<AtividadeInscricaoOficinas>(() => aliasAtividade)
                    .JoinQueryOver(x => x.Inscrito)
                        .Where(x => x.Situacao == EnumSituacaoInscricao.Aceita)
                        .JoinQueryOver(y => y.Evento)
                            .Where(y => y.Id == evento.Id)
                    .WithSubquery.WhereNotExists(subQueryParticipantes)
                    .Select(x => x.Inscrito)
                    .List<InscricaoParticipante>();
            }
            else
            {
                AtividadeInscricaoOficinaSemEscolha aliasAtividadeSemEscolha = null;

                var subQueryParticipantes = QueryOver.Of<Oficina>()
                    .JoinQueryOver<InscricaoParticipante>(x => x.Participantes, () => aliasParticipante)
                    .Where(x => x.Id == aliasAtividadeSemEscolha.Inscrito.Id && x.Situacao == EnumSituacaoInscricao.Aceita)
                    .SelectList(x => x.Select(() => aliasParticipante.Id));

                var subQueryCoordenadores = QueryOver.Of<AtividadeInscricaoOficinasCoordenacao>()
                    .Where(x => x.Inscrito.Id == aliasAtividadeSemEscolha.Id)
                    .Select(x => x.Inscrito.Id);

                return mSessao.QueryOver<AtividadeInscricaoOficinaSemEscolha>(() => aliasAtividadeSemEscolha)
                    .JoinQueryOver(x => x.Inscrito)
                        .Where(x => x.Situacao == EnumSituacaoInscricao.Aceita)
                        .JoinQueryOver(y => y.Evento)
                            .Where(y => y.Id == evento.Id)
                    .WithSubquery.WhereNotExists(subQueryParticipantes)
                    .Select(x => x.Inscrito)
                    .List<InscricaoParticipante>();
            }
        }

        public override bool HaAOficinasSemResponsavelDefinidoDoEvento(Evento evento)
        {
            Oficina aliasAfrac = null;

            var consultaResponsaveis =
                QueryOver.Of<AtividadeInscricaoOficinasCoordenacao>()
                    .Where(x => x.OficinaEscolhida.Id == aliasAfrac.Id)
                    .SelectList(x => x.Select(y => y.Inscrito));

            return mSessao.QueryOver<Oficina>(() => aliasAfrac)
                .Where(x => x.Evento.Id == evento.Id)
                .WithSubquery.WhereNotExists(consultaResponsaveis)
                .RowCount() > 0;
        }

        public override bool EhParticipanteDeOficinaNoEvento(Evento evento, InscricaoParticipante participante)
        {
            return mSessao.QueryOver<Oficina>()
                .Where(x => x.Evento == evento)
                    .JoinQueryOver(x => x.Participantes)
                        .Where(c => c.Id == participante.Id)
                .RowCount() > 0;
        }

        protected override bool HaParticipatesOuResponsaveisEmOutraOficina(Oficina afrac)
        {
            InscricaoParticipante aliasParticipante = null;

            var queryParticipantes = mSessao.QueryOver<Oficina>()
                .Where(x => x.Id != afrac.Id)
                .JoinQueryOver<InscricaoParticipante>(x => x.Participantes, () => aliasParticipante)
                .SelectList(x => x.Select(() => aliasParticipante.Id))
                .Future<int>();
                
            return queryParticipantes.Where(x => afrac.Participantes.Select(i => i.Id).Contains(x)).Count() > 0;
        }

        public override Oficina BuscarOficinaDoInscrito(int idEvento, int idInscricao)
        {
            Oficina aliasAfrac = null;

            var consultaParticipantes = QueryOver.Of<Oficina>()
                .Where(x => x.Id == aliasAfrac.Id)
                .JoinQueryOver(x => x.Participantes)
                    .Where(y => y.Id == idInscricao)
                .SelectList(lista => lista
                    .Select(x => x.Id));

            var consultaCoordenadores = QueryOver.Of<AtividadeInscricaoOficinasCoordenacao>()
                .Where(x => x.OficinaEscolhida.Id == aliasAfrac.Id && x.Inscrito.Id == idInscricao)
                .SelectList(lista => lista
                    .Select(x => x.Inscrito.Id));

            return mSessao.QueryOver<Oficina>(() => aliasAfrac)
                .Where(Restrictions.Conjunction()
                    .Add<Oficina>(x => x.Evento.Id == idEvento)
                    .Add(Restrictions.Disjunction()
                        .Add(Subqueries.WhereExists(consultaCoordenadores))
                        .Add(Subqueries.WhereExists(consultaParticipantes)
                        )
                    )
                )
                .SingleOrDefault();
        }

        public override int ContarTotalOficinas(Evento mEvento)
        {
            return mSessao
                .QueryOver<Oficina>()
                .Where(x => x.Evento == mEvento)
                .RowCount();
        }
    }
}
