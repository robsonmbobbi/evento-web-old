using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    class TransacaoMapping : ClassMapping<Transacao>
    {
        public TransacaoMapping()
        {
            this.Table("transacoes_financeiras");
            this.Id(x => x.Id, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_TRANSACAO_FINACEIRA");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_TRANSACAO_FINACEIRA" });
                });
            });

            this.Property(x => x.DataHora, m =>
            {
                m.Column("DATA_HORA");
                m.NotNullable(true);
                m.Access(Accessor.Property);
            });

            this.Property(x => x.OQue, m =>
            {
                m.Column("O_QUE");
                m.NotNullable(true);
                m.Access(Accessor.NoSetter);
                m.Length(500);
            });

            this.ManyToOne(x => x.QualConta, m =>
            {
                m.Column("ID_CONTA");
                m.NotNullable(true);
                m.Access(Accessor.Property);
                m.Class(typeof(Conta));
            });

            this.Property(x => x.Tipo, m =>
            {
                m.Column("TIPO");
                m.NotNullable(true);
                m.Access(Accessor.Property);
                m.Type<EnumGeneric<EnumTipoTransacao>>();
            });

            this.Property(x => x.Valor, m =>
            {
                m.Column("VALOR");
                m.NotNullable(true);
                m.Access(Accessor.NoSetter);
            });

            this.ManyToOne(x => x.QualEvento, m =>
            {
                m.Column("ID_EVENTO");
                m.NotNullable(true);
                m.Access(Accessor.Property);
                m.Class(typeof(Evento));
            });

            this.ManyToOne(x => x.Origem, m =>
            {
                m.Column("ID_ORIGEM");
                m.NotNullable(true);
                m.Access(Accessor.Property);
                m.Class(typeof(EntidadeFinanceira));
            });
        }
    }
}
