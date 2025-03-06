using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class EventoMapping: ClassMapping<Evento>
    {
        public EventoMapping()
        {
            this.Table("eventos");
            this.Id(x => x.Id, m =>
              {
                  m.Access(NHibernate.Mapping.ByCode.Accessor.NoSetter);
                  m.Column("ID_EVENTO");
                  m.Generator(Generators.Native, g =>
                  {
                      g.Params(new { sequence = "GEN_EVENTOS" });
                  });
              });

            this.Property(x => x.Nome, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Length(250);
                m.Column("NOME");
            });

            this.Component(x => x.PeriodoInscricaoOnLine, c =>
             {
                 c.Access(Accessor.NoSetter);
                 c.Property(o => o.DataInicial, m => {
                     m.Access(Accessor.Property);
                     m.NotNullable(true);
                     m.Column("DATA_INICIO_INSC");
                 });
                 c.Property(o => o.DataFinal, m => {
                     m.Access(Accessor.Property);
                     m.NotNullable(true);
                     m.Column("DATA_FIM_INSC");
                 });
             });

            this.Component(x => x.PeriodoRealizacaoEvento, c =>
            {
                c.Access(Accessor.NoSetter);
                c.Property(o => o.DataInicial, m => {
                    m.Access(Accessor.Property);
                    m.NotNullable(true);
                    m.Column("DATA_INICIO_EVENTO");
                });
                c.Property(o => o.DataFinal, m => {
                    m.Access(Accessor.Property);
                    m.NotNullable(true);
                    m.Column("DATA_FIM_EVENTO");
                });
            });

            this.Property(x => x.DataRegistro, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("DATA_REGISTRO");
            });
            this.ManyToOne(x => x.Logotipo, m => {
                m.Access(Accessor.Property);
                m.Cascade(Cascade.All | Cascade.DeleteOrphans);
                m.NotNullable(false);
                m.Column("ID_ARQUIVO_LOGOTIPO");
            });
            this.Property(x => x.TemDepartamentalizacao, m => {
                m.Access(Accessor.Property);
                m.NotNullable(true);
                m.Column("TEM_DEPARTAMENTALIZACAO");
            });

            this.Property(x => x.ConfiguracaoSalaEstudo, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Access(Accessor.NoSetter);
                m.NotNullable(false);
                m.Column("MODELO_DIV_SL_ESTUDO");
            });

            this.Property(x => x.TemDormitorios, m => {
                m.Access(Accessor.Property);
                m.NotNullable(true);
                m.Column("TEM_DORMITORIOS");
            });

            this.Property(x => x.ConfiguracaoEvangelizacao, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(false);
                m.Column("PUBLICO_EVANGELIZACAO");
            });

            this.Property(x => x.ConfiguracaoTempoSarauMin, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(false);
                m.Column("TEMPO_SARAU_MIN");
            });

            this.Property(x => x.IdadeMinimaInscricaoAdulto, m =>
             {
                 m.Access(Accessor.NoSetter);
                 m.NotNullable(true);
                 m.Column("IDADE_MINIMA_INSC_ADULTO");
             });

            this.Property(x => x.ValorInscricaoAdulto, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("VALOR_INSC_ADULTO");
            });

            this.Property(x => x.ValorInscricaoCrianca, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("VALOR_INSC_CRIANCA");
            });

            this.Property(x => x.ConfiguracaoOficinas, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(false);
                m.Column("MODELO_DIV_OFICINAS");
            });

            this.Property(x => x.PermiteEscolhaDormirEvento, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(false);
                m.Column("PERMITE_ESCOLHA_DORMIR_EVENTO");
            });
        }
    }
}
