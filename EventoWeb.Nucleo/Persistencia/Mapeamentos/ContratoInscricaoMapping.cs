using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class ContratoInscricaoMapping : ClassMapping<ContratoInscricao>
    {
        public ContratoInscricaoMapping()
        {
            this.Table("contratos_inscricao");
            this.Id(x => x.Id, m =>
              {
                  m.Access(Accessor.NoSetter);
                  m.Column("ID_CONTRATO_INSCRICAO");
                  m.Generator(Generators.Native, g =>
                  {
                      g.Params(new { sequence = "GEN_CONTRATOS_INSCRICAO" });
                  });
              });

            this.Property(x => x.InstrucoesPagamento, m =>
              {
                  m.Column("INSTRUCOES_PAGAMENTO");
                  m.NotNullable(false);
                  m.Type(NHibernateUtil.StringClob);
              });
            this.Property(x => x.PassoAPassoInscricao, m =>
            {
                m.Column("PASSA_A_PASSO_INSCRICAO");
                m.NotNullable(false);
                m.Type(NHibernateUtil.StringClob);
            });
            this.Property(x => x.Regulamento, m =>
            {
                m.Column("REGULAMENTO");
                m.NotNullable(false);
                m.Type(NHibernateUtil.StringClob);
            });

            this.ManyToOne(x => x.Evento, m =>
              {
                  m.Column("ID_EVENTO");
                  m.NotNullable(true);
                  m.Access(Accessor.NoSetter);
              });
        }
    }
}
