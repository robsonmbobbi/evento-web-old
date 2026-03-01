using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    class TituloMapping: ClassMapping<Titulo>
    {
        public TituloMapping()
        {
            this.Table("titulos_financeiros");
            this.Id(x => x.Id, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_TITULO_FINANCEIRO");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_TITULO_FINANCEIRO" });
                });
            });

            this.Property(x => x.DataCriado, m =>
            {
                m.Column("DATA_CRIACAO");
                m.NotNullable(true);
                m.Access(Accessor.Property);
            });

            this.Property(x => x.DataVencimento, m =>
            {
                m.Column("DATA_VENCIMENTO");
                m.NotNullable(true);
                m.Access(Accessor.Property);
            });

            this.Property(x => x.Descricao, m =>
            {
                m.Column("DESCRICAO");
                m.NotNullable(false);
                m.Access(Accessor.Property);
                m.Length(300);
            });

            this.Property(x => x.Tipo, m =>
            {
                m.Column("TIPO");
                m.NotNullable(true);
                m.Access(Accessor.Property);
                m.Type<EnumGeneric<EnumTipoTransacao>>();
            });

            this.Property(x => x.Liquidado, m =>
            {
                m.Column("LIQUIDADO");
                m.NotNullable(true);
                m.Access(Accessor.Property);
            });

            this.Property(x => x.Valor, m =>
            {
                m.Column("VALOR");
                m.NotNullable(true);
                m.Access(Accessor.Property);
            });

            this.ManyToOne(x => x.Evento, m =>
            {
                m.Column("ID_EVENTO");
                m.NotNullable(true);
                m.Access(Accessor.Property);
                m.Class(typeof(Evento));
            });

            this.ManyToOne(x => x.Origem, m =>
            {
                m.Column("ID_FATURAMENTO");
                m.NotNullable(true);
                m.Access(Accessor.Property);
                m.Class(typeof(Faturamento));
            });
        }
    }
}
