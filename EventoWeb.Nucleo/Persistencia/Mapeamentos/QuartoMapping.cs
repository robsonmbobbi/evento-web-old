using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class QuartoMapping: ClassMapping<Quarto>
    {
        public QuartoMapping()
        {
            this.Table("quartos");
            this.Id(x => x.Id, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_QUARTO");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_QUARTO" });
                });
            });

            this.Property(x => x.Capacidade, m =>
              {
                  m.Access(Accessor.NoSetter);
                  m.NotNullable(false);
                  m.Column("CAPACIDADE");
              });

            this.Property(x => x.EhFamilia, m =>
            {
                m.Access(Accessor.Property);
                m.NotNullable(true);
                m.Column("EH_FAMILIA");
            });

            this.ManyToOne(x => x.Evento, m =>
            {
                m.Column("ID_EVENTO");
                m.NotNullable(false);
                m.Access(Accessor.NoSetter);
                m.Class(typeof(Evento));
            });

            this.Bag(x => x.Inscritos, m =>
            {
                m.Cascade(Cascade.All | Cascade.DeleteOrphans);
                m.Access(Accessor.NoSetter);
                m.Inverse(true);
                m.Lazy(CollectionLazy.Lazy);
                m.Key(k => k.Column("ID_QUARTO"));
            }, c => c.OneToMany(a => a.Class(typeof(QuartoInscrito))));

            this.Property(x => x.Nome, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("NOME");
                m.Length(100);
            });

            this.Property(x => x.Sexo, m =>
            {
                m.Access(Accessor.Property);
                m.NotNullable(true);
                m.Column("SEXO");
                m.Type<EnumGeneric<EnumSexoQuarto>>();
            });
        }
    }
}
