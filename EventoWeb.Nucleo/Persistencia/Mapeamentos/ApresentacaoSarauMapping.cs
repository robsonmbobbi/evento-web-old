using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class ApresentacaoSarauMapping : ClassMapping<ApresentacaoSarau>
    {
        public ApresentacaoSarauMapping()
        {
            this.Table("apresentacoes_sarau");
            this.Id(x => x.Id, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_APRESENTACAO_SARAU");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_APRESENTACAO_SARAU" });
                });
            });

            this.ManyToOne(x => x.Evento, m =>
            {
                m.Column("ID_EVENTO");
                m.NotNullable(false);
                m.Access(Accessor.NoSetter);
                m.Class(typeof(Evento));
            });

            this.Property(x => x.DuracaoMin, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("DURACAO_MIN");
            });

            this.Property(x => x.Tipo, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("TIPO");
                m.Length(200);
            });

            this.Bag(x => x.Inscritos, m =>
            {
                m.Cascade(Cascade.None);
                m.Inverse(false);
                m.Lazy(CollectionLazy.NoLazy);
                m.Access(Accessor.NoSetter);
                m.Table("apresentacoes_sarau_inscritos");
                m.Key(k => k.Column("ID_APRESENTACAO_SARAU"));
            }, c => c.ManyToMany(a => a.Column("ID_INSCRICAO")));            
        }
    }
}
