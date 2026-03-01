using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class QuartoInscritoMapping: ClassMapping<QuartoInscrito>
    {
        public QuartoInscritoMapping()
        {
            this.Table("quartos_inscritos");
            this.Id(x => x.Id, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_QUARTO_INSCRITO");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_QUARTO_INSCRITO" });
                });
            });

            this.Property(x => x.EhCoordenador, m =>
              {
                  m.Access(Accessor.Property);
                  m.NotNullable(true);
                  m.Column("EH_COORDENADOR");
              });

            this.ManyToOne(x => x.Inscricao, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("ID_INSCRICAO");
            });

            this.ManyToOne(x => x.Quarto, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("ID_QUARTO");
            });
        }
    }
}
