using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class AtividadeInscricaoMapping : ClassMapping<AAtividadeInscricao>
    {
        public AtividadeInscricaoMapping()
        {
            this.Table("atividades_inscricao");

            this.Discriminator(x =>
            {
                x.Column("TIPO_ATIVIDADE");
                x.Length(30);
                x.NotNullable(true);
            });

            this.Abstract(true);

            this.Id(x => x.Id, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_ATIVIDADE_INSCRICAO");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_ATIVIDADE_INSCRICAO" });
                });
            });

            this.ManyToOne(x => x.Inscrito, m =>
            {
                m.Column("ID_INSCRICAO");
                m.NotNullable(true);
                m.Access(Accessor.NoSetter);
                m.Class(typeof(InscricaoParticipante));
            });
        }
    }

    public class AtividadeInscricaoDepartamentoMapping : SubclassMapping<AtividadeInscricaoDepartamento>
    {
        public AtividadeInscricaoDepartamentoMapping()
        {
            this.DiscriminatorValue("departamentos");

            this.ManyToOne(x => x.DepartamentoEscolhido, m =>
            {
                m.Column("ID_DEPARTAMENTO");
                m.Access(Accessor.NoSetter);
                m.NotNullable(false);
                m.Lazy(LazyRelation.NoLazy);
                m.Class(typeof(Departamento));
            });

            this.Property(x => x.EhCoordenacao, m =>
              {
                  m.Column("EH_COORD_DEP");
                  m.Access(Accessor.Property);
                  m.NotNullable(false);
              });
        }
    }

    public class AtividadeInscricaoOficinasMapping : SubclassMapping<AtividadeInscricaoOficinas>
    {
        public AtividadeInscricaoOficinasMapping()
        {
            this.DiscriminatorValue("oficinas");

            this.List(x => x.Oficinas, m =>
            {
                m.Cascade(Cascade.None);
                m.Access(Accessor.NoSetter);
                m.Inverse(false);
                m.Lazy(CollectionLazy.NoLazy);
                m.Index(x => x.Column("POSICAO"));
                m.Key(k => k.Column("ID_ATIVIDADE_INSCRICAO"));
                m.Table("oficinas_escolhidas");
            }, c => c.ManyToMany(a => a.Column("ID_OFICINA")));
        }
    }

    public class AtividadeInscricaoOficinaSemEscolhaMapping : SubclassMapping<AtividadeInscricaoOficinaSemEscolha>
    {
        public AtividadeInscricaoOficinaSemEscolhaMapping()
        {
            this.DiscriminatorValue("oficinas_sem_escolha");
        }
    }

    public class AtividadeInscricaoOficinasCoordenacaoMapping : SubclassMapping<AtividadeInscricaoOficinasCoordenacao>
    {
        public AtividadeInscricaoOficinasCoordenacaoMapping()
        {
            this.DiscriminatorValue("oficinasCoord");

            this.ManyToOne(x => x.OficinaEscolhida, m =>
            {
                m.Column("ID_OFICINA");
                m.Access(Accessor.NoSetter);
                m.NotNullable(false);
                m.Lazy(LazyRelation.NoLazy);
                m.Class(typeof(Oficina));
            });
        }
    }

    public class AtividadeInscricaoSalasMapping : SubclassMapping<AtividadeInscricaoSalaEstudo>
    {
        public AtividadeInscricaoSalasMapping()
        {
            this.DiscriminatorValue("salas");
        }
    }

    public class AtividadeInscricaoSalasCoordenacaoMapping : SubclassMapping<AtividadeInscricaoSalaEstudoCoordenacao>
    {
        public AtividadeInscricaoSalasCoordenacaoMapping()
        {
            this.DiscriminatorValue("salasCoord");

            this.ManyToOne(x => x.SalaEscolhida, m =>
            {
                m.Column("ID_SALA_ESTUDO");
                m.Access(Accessor.NoSetter);
                m.NotNullable(false);
                m.Lazy(LazyRelation.NoLazy);
                m.Class(typeof(SalaEstudo));
            });
        }
    }

    public class AtividadeInscricaoSalasOrdemEscolhaMapping : SubclassMapping<AtividadeInscricaoSalaEstudoOrdemEscolha>
    {
        public AtividadeInscricaoSalasOrdemEscolhaMapping()
        {
            this.DiscriminatorValue("salasEscolhidas");

            this.List(x => x.Salas, m =>
            {
                m.Cascade(Cascade.None);
                m.Access(Accessor.NoSetter);
                m.Inverse(false);
                m.Lazy(CollectionLazy.NoLazy);
                m.Index(x => x.Column("POSICAO"));
                m.Key(k => k.Column("ID_ATIVIDADE_INSCRICAO"));
                m.Table("salas_estudo_escolhidas");
            }, c => c.ManyToMany(a => a.Column("ID_SALA_ESTUDO")));
        }
    }
}
