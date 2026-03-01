using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class DepartamentoMapping: ClassMapping<Departamento>
    {
        public DepartamentoMapping()
        {
            this.Table("departamentos");
            this.Id(x => x.Id, m =>
            {
                m.Access(NHibernate.Mapping.ByCode.Accessor.NoSetter);
                m.Column("ID_DEPARTAMENTO");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_DEPARTAMENTOS" });
                });
            });

            this.ManyToOne(x => x.Evento, m =>
            {
                m.NotNullable(true);
                m.Access(Accessor.NoSetter);
                m.Column("ID_EVENTO");
            });

            this.Property(x => x.Nome, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("NOME");
                m.Length(250);
            });
        }
    }
}
