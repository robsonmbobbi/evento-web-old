using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    class FaturamentoMapping: ClassMapping<Faturamento>
    {
        public FaturamentoMapping()
        {
            this.Table("faturamentos");

            Discriminator(d =>
            {
                d.Column("TIPO_FATURAMENTO");
                d.Length(30);
            });

            Abstract(true);

            this.Id(x => x.Id, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_FATURAMENTO");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_FATURAMENTO" });
                });
            });

            this.Property(x => x.Descricao, m =>
              {
                  m.Column("DESCRICAO");
                  m.NotNullable(true);
                  m.Length(500);
                  m.Access(Accessor.NoSetter);
              });

            this.ManyToOne(x=> x.Evento, m =>
            {
                m.Column("ID_EVENTO");
                m.NotNullable(true);
                m.Access(Accessor.Property);
                m.Class(typeof(Evento));
            });

            this.Property(x => x.Faturado, m =>
              {
                  m.Column("FATURADO");
                  m.NotNullable(true);
                  m.Access(Accessor.Property);
              });

            this.Property(x=>x.MotivoDesconto, m =>
            {
                m.Column("MOTIVO_DESCONTO");
                m.NotNullable(false);
                m.Length(500);
                m.Access(Accessor.Property);
            });

            this.Property(x => x.Tipo, m =>
              {
                  m.Column("TIPO");
                  m.NotNullable(true);
                  m.Access(Accessor.Property);
                  m.Type<EnumGeneric<EnumTipoTransacao>>();
              });

            this.Property(x => x.ValorBruto, m =>
              {
                  m.Column("VALOR_BRUTO");
                  m.NotNullable(true);
                  m.Access(Accessor.NoSetter);
              });

            this.Property(x=>x.ValorDesconto, m =>
            {
                m.Column("VALOR_DESCONTO");
                m.NotNullable(true);
                m.Access(Accessor.Property);
            });

            this.Property(x => x.Data, m =>
            {
                m.Column("DATA");
                m.NotNullable(true);
                m.Access(Accessor.Property);
            });
        }
    }

    class FaturamentoInscricaoMapping: SubclassMapping<FaturamentoInscricao>
    {
        public FaturamentoInscricaoMapping()
        {
            DiscriminatorValue("INSCRICAO");

            this.Bag(x => x.Inscricoes, m =>
            {
                m.Cascade(Cascade.None);
                m.Access(Accessor.NoSetter);
                m.Inverse(false);
                m.Lazy(CollectionLazy.Lazy);
                m.Key(k => k.Column("ID_FATURAMENTO"));
                m.Table("faturamento_inscricoes");
            }, c => c.ManyToMany(a => a.Column("ID_INSCRICAO")));
        }
    }

    class FaturamentoCompraMapping : SubclassMapping<FaturamentoCompra>
    {
        public FaturamentoCompraMapping()
        {
            DiscriminatorValue("COMPRA");
        }
    }

    class FaturamentoDoacaoMapping : SubclassMapping<FaturamentoDoacao>
    {
        public FaturamentoDoacaoMapping()
        {
            DiscriminatorValue("DOACAO");
        }
    }
}
