using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Repositorios;
using EventoWeb.Nucleo.Persistencia.Infra;
using EventoWeb.Nucleo.Persistencia.Relatorios;
using EventoWeb.Nucleo.Persistencia.Repositorios;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using System.Reflection;

namespace EventoWeb.Nucleo.Persistencia
{
    public class ConfiguracaoNHibernate {

        public ISessionFactory GerarFabricaSessao()
        {
            NHibernate.Cfg.Environment.UseReflectionOptimizer = false;
            var configuration = new Configuration();
            configuration.Configure();
            var mapper = new ModelMapper();

            configuration.AddAssembly("EventoWeb.Nucleo");
            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());

            configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            return configuration.BuildSessionFactory();
        }
    }

    public class Contexto: IContexto
    {
        private readonly ISession m_Sessao;

        public Contexto(ISession sessao)
        {
            m_Sessao = sessao;
        }

        public void IniciarTransacao()
        {
            m_Sessao.BeginTransaction();
        }

        public void SalvarTransacao()
        {
            var session = m_Sessao;
            if (session.GetCurrentTransaction() != null)
                session.GetCurrentTransaction().Commit();
        }

        public void CancelarTransacao()
        {
            var session = m_Sessao;
            if (session.GetCurrentTransaction() != null)
            {
                session.GetCurrentTransaction().Rollback();
                session.Clear();
            }
        }

        public void Dispose()
        {
            m_Sessao.Close();
            m_Sessao.Dispose();
        }

        public AEventos RepositorioEventos => new RepositorioEventosNH(m_Sessao);
        public AUsuarios RepositorioUsuarios => new RepositorioUsuariosNH(m_Sessao);
        public ADepartamentos RepositorioDepartamentos => new RepositorioDepartamentosNH(m_Sessao);
        public ASalasEstudo RepositorioSalasEstudo => new RepositorioSalasEstudoNH(m_Sessao);
        public AOficinas RepositorioOficinas => new RepositorioOficinasNH(m_Sessao);
        public AConfiguracoesEmail RepositorioConfiguracoesEmail => new RepositorioConfiguracoesEmailNH(m_Sessao);
        public AInscricoes RepositorioInscricoes => new InscricoesNH(m_Sessao);
        public ACodigosAcessoInscricao RepositorioCodigosAcessoInscricao => new RepositorioCodigosAcessoInscricaoNH(m_Sessao);        
        public AMensagensEmailPadrao RepositorioMensagensEmailPadrao => new RepositorioMensagensEmailPadrao(m_Sessao);
        public AApresentacoesSarau RepositorioApresentacoesSarau => new RepositorioApresentacoesSarauNH(m_Sessao);
        public AArquivosBinarios RepositorioArquivosBinarios => new RepositorioArquivosBinariosNH(m_Sessao);
        public AQuartos RepositorioQuartos => new RepositorioQuartosNH(m_Sessao);

        public IServicoGeradorCodigoSeguro ServicoGeradorCodigoSeguro => new ServicoGeradorCodigoSeguro();

        public ATitulos RepositorioTitulosFinanceiros => new RepositorioTitulosFinanceirosNH(m_Sessao);
        public ATransacoes RepositorioTransacoesFinanceiras => new RepositorioTransacoesFinanceirasNH(m_Sessao);
        public AContas RepositorioContasBancarias => new RepositorioContasBancariasNH(m_Sessao);
        public AFaturamentos RepositorioFaturamentos => new RepositorioFaturamentosNH(m_Sessao);
        
        public IRelatorioDivisaoSalasEstudo RelatorioDivisaoSalasEstudo => new RelatorioDivisaoSalasEstudo();
        public IRelatorioDivisaoOficinas RelatorioDivisaoOficinas => new RelatorioDivisaoOficinas();
        public IRelatorioDivisaoQuartos RelatorioDivisaoQuartos => new RelatorioDivisaoQuartos();

        public AContratosInscricao RepositorioContratosInscricao => new RepositorioContratosInscricao(m_Sessao);

        public IRelatorioInscritosDepartamentos RelatorioInscritosDepartamentos => new RelatorioInscritosDepartamentos();

        public IRelatorioSarau RelatorioSarau => new RelatorioSarau();

        public AConfiguracoesWhatsapp RepositorioConfiguracoesWhatsapp => new RepositorioConfiguracoesWhatsappNH(m_Sessao);

        public AMensagensWhatsappPadrao RepositorioMensagensWhatsappPadrao => new RepositorioMensagensWhatsappPadrao(m_Sessao);
    }
}
