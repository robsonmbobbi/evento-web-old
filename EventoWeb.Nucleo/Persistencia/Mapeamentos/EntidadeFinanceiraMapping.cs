using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    class EntidadeFinanceiraMapping: ClassMapping<EntidadeFinanceira>
    {
        public EntidadeFinanceiraMapping()
        {
            this.Table("entidades_financeiras");

            Discriminator(d =>
            {
                d.Column("TIPO");
                d.Length(30);
            });

            Abstract(true);

            this.Id(x => x.Id, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_ENTIDADE_FINANCEIRA");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_ENTIDADE_FINANCEIRA" });
                });
            });

            this.Property(x => x.Tipo, m =>
            {
                m.Column("TIPO");
                m.NotNullable(true);
                m.Access(Accessor.Property);
                m.Type<EnumGeneric<EnumTipoTransacao>>();
            });

            this.ManyToOne(x => x.Evento, m =>
            {
                m.Column("ID_EVENTO");
                m.NotNullable(true);
                m.Access(Accessor.Property);
                m.Class(typeof(Evento));
            });
        }

    }
}
