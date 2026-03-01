using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class CodigoAcessoInscricaoMapping : ClassMapping<CodigoAcessoInscricao>
    {
        public CodigoAcessoInscricaoMapping()
        {
            this.Table("codigos_acesso_inscricao");
            this.Id(x => x.Id, m =>
            {
                m.Access(NHibernate.Mapping.ByCode.Accessor.NoSetter);
                m.Column("ID_CODIGO_ACESSO_INS");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_CODIGOS_ACESSO_INSCRICAO" });
                });
            });

            this.Property(x => x.Codigo, m =>
              {
                  m.Access(Accessor.Property);
                  m.NotNullable(true);
                  m.Column("CODIGO");
                  m.Length(100);
                  m.Unique(true);
              });

            this.Property(x => x.DataHoraValidade, m =>
            {
                m.Access(Accessor.Property);
                m.NotNullable(true);
                m.Column("DATA_HORA_VALIDADE");
            });

            this.ManyToOne(x => x.Inscricao, m =>
            {
                m.Access(Accessor.Property);
                m.NotNullable(false);
                m.Column("ID_INSCRICAO");
            });

            this.Property(x => x.Identificacao, m =>
             {
                 m.Access(Accessor.Property);
                 m.NotNullable(false);
                 m.Column("IDENTIFICACAO");
                 m.Length(100);
             });
        }
    }
}
