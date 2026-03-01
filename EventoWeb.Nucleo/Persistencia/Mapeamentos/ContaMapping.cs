using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    class ContaMapping: ClassMapping<Conta>
    {
        public ContaMapping()
        {
            this.Table("contas");
            this.Id(x => x.Id, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_CONTA");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_CONTA" });
                });
            });

            this.Property(x => x.Descricao, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("DESCRICAO");
                m.Length(100);
            });

            this.Property(x => x.SaldoInicial, m =>
            {
                m.Access(Accessor.Property);
                m.NotNullable(true);
                m.Column("SALDO_INICIAL");
            });

            this.ManyToOne(x => x.QualEvento, m =>
            {
                m.Column("ID_EVENTO");
                m.NotNullable(true);
                m.Access(Accessor.Property);
                m.Class(typeof(Evento));
            });
        }
    }
}
