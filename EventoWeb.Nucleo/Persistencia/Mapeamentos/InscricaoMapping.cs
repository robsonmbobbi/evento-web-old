using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class InscricaoMapping: ClassMapping<Inscricao>
    {
        public InscricaoMapping()
        {
            Table("inscricoes");
            this.Lazy(false);

            Discriminator(d =>
            {
                d.Column("TIPO_INSCRICAO");
                d.Length(30);
            });

            Abstract(true);

            Id(x => x.Id, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_INSCRICAO");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_INSCRICAO" });
                });
            });

            Property(x => x.ConfirmadoNoEvento, m =>
              {
                  m.Access(Accessor.Property);
                  m.Column("CONFIRMADO");
                  m.NotNullable(true);
              });

            Property(x => x.DataRecebimento, m =>
              {
                  m.Access(Accessor.Property);
                  m.Column("DATA_RECEBIMENTO");
                  m.NotNullable(true);
              });

            Property(x => x.DormeEvento, m =>
            {
                m.Access(Accessor.Property);
                m.Column("DORMIRA");
                m.NotNullable(true);
            });
            ManyToOne(x => x.Evento, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_EVENTO");
                m.NotNullable(true);
            });

            Property(x => x.NomeCracha, m =>
            {
                m.Access(Accessor.Property);
                m.Column("NOME_CRACHA");
                m.Length(150);
                m.NotNullable(false);
            });
            ManyToOne(x => x.Pessoa, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_PESSOA");
                m.NotNullable(true);
                m.Cascade(Cascade.All | Cascade.DeleteOrphans);
            });
            Property(x => x.Situacao, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("SITUACAO");
                m.NotNullable(true);
                m.Type<EnumGeneric<EnumSituacaoInscricao>>();
            });

            Property(x => x.Observacoes, m =>
            {
                m.Access(Accessor.Property);
                m.Column("OBSERVACOES");
                m.NotNullable(false);
                m.Type(NHibernateUtil.StringClob);
            });

            Component(x => x.Pagamento, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Parent(x => x.Inscricao, n =>
                {
                    n.Access(Accessor.NoSetter);
                });

                m.Bag(y => y.Comprovantes, n =>
                {
                    n.Cascade(Cascade.All | Cascade.DeleteOrphans);
                    n.Inverse(true);
                    n.Lazy(CollectionLazy.Lazy);
                    n.Access(Accessor.NoSetter);
                    n.Key(k => k.Column("ID_INSCRICAO"));
                }, c => c.OneToMany(o => o.Class(typeof(ComprovantePagamento))));

                m.Property(y => y.Forma, n =>
                {
                    n.Column("FORMA_PAGAMENTO");
                    n.Access(Accessor.NoSetter);
                    n.NotNullable(false);
                    n.Type<EnumGeneric<EnumPagamento>>();
                });
                m.Property(y => y.Observacao, n =>
                {
                    n.Column("OBS_PAGAMENTO");
                    n.Access(Accessor.Property);
                    n.NotNullable(false);
                    n.Type(NHibernateUtil.StringClob);
                });
            });
        }
    }

    public class InscricaoInfantilMapping: SubclassMapping<InscricaoInfantil>
    {
        public InscricaoInfantilMapping()
        {
            DiscriminatorValue("INFANTIL");
            
            ManyToOne(x => x.InscricaoResponsavel1, m =>
            {
                m.Column("ID_INSC_RESPONSAVEL_1");
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
            });

            ManyToOne(x => x.InscricaoResponsavel2, m =>
            {
                m.Column("ID_INSC_RESPONSAVEL_2");
                m.Access(Accessor.NoSetter);
                m.NotNullable(false);
            });
        }
    }

    public class InscricaoParticipanteTrabalhadorMapping: SubclassMapping<InscricaoParticipante>
    {
        public InscricaoParticipanteTrabalhadorMapping()
        {
            DiscriminatorValue("PART_TRAB");

            Bag(x => x.Atividades, m =>
              {
                  m.Cascade(Cascade.All | Cascade.DeleteOrphans);
                  m.Inverse(true);
                  m.Lazy(CollectionLazy.Lazy);
                  m.Access(Accessor.NoSetter);
                  m.Key(k => k.Column("ID_INSCRICAO"));
              }, c => c.OneToMany(o => o.Class(typeof(AAtividadeInscricao))));

            Property(x => x.Tipo, m =>
            {
                m.Column("TIPO");
                m.Access(Accessor.Property);
                m.NotNullable(false);
                m.Type<EnumGeneric<EnumTipoParticipante>>();
            });
            Property(x => x.InstituicoesEspiritasFrequenta, m =>
            {
                m.Access(Accessor.Property);
                m.Column("INSTITUICOES_ESPIRITAS_FREQ");
                m.NotNullable(false);
            });
        }
    }

    public class ComprovantePagamentoMapping: ClassMapping<ComprovantePagamento>
    {
        public ComprovantePagamentoMapping()
        {
            Table("pagamento_inscricao_comprovantes");            

            Id(x => x.Id, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_PAG_INSC_COMPROVANTES");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_PAG_INSC_COMPROVANTES" });
                });
            });

            ManyToOne(x => x.ArquivoComprovante, m =>
              {
                  m.Column("ID_ARQUIVO");
                  m.Access(Accessor.NoSetter);
                  m.NotNullable(true);
                  m.Cascade(Cascade.All | Cascade.DeleteOrphans);
              });
            ManyToOne(x => x.Inscricao, m =>
            {
                m.Column("ID_INSCRICAO");
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
            });
        }
    }
}
