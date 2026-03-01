using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class PessoaComumMapping: ClassMapping<PessoaComum>
    {
        public PessoaComumMapping()
        {
            this.Table("pessoas");

            this.Discriminator(x =>
            {
                x.Column("TIPO_PESSOA");
                x.Length(30);
                x.NotNullable(true);
            });


            this.Id(x => x.Id, m =>
            {
                m.Access(NHibernate.Mapping.ByCode.Accessor.NoSetter);
                m.Column("ID_PESSOA");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_PESSOAS" });
                });
            });

            this.Property(x => x.Nome, m =>
              {
                  m.Access(Accessor.NoSetter);
                  m.Column("NOME");
                  m.Length(150);
                  m.NotNullable(true);
              });

            this.Property(x => x.Celular, m =>
            {
                m.Access(Accessor.Property);
                m.Column("CELULAR");
                m.Length(15);
                m.NotNullable(false);
            });
            this.Property(x => x.Email, m =>
            {
                m.Access(Accessor.Property);
                m.Column("EMAIL");
                m.Length(100);
                m.NotNullable(false);
            });
        }
    }

    public class PessoaMapping : SubclassMapping<Pessoa>
    {
        public PessoaMapping()
        {
            this.DiscriminatorValue("evento");

            this.Property(x => x.AlergiaAlimentos, m =>
              {
                  m.Access(Accessor.Property);
                  m.Column("ALERGIA_ALIMENTOS");
                  m.Length(100);
                  m.NotNullable(false);
              });
            this.Property(x => x.DataNascimento, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("DATA_NASCIMENTO");
                m.NotNullable(false);
            });
            this.Property(x => x.EhDiabetico, m =>
            {
                m.Access(Accessor.Property);
                m.Column("EH_DIABETICO");
                m.NotNullable(false);
            });
            this.Property(x => x.EhVegetariano, m =>
            {
                m.Access(Accessor.Property);
                m.Column("EH_VEGETARIANO");
                m.NotNullable(false);
            });
            this.Component(x => x.Endereco, m =>
            {
                m.Access(Accessor.NoSetter);

                m.Property(x => x.Cidade, n =>
                  {
                      n.Access(Accessor.NoSetter);
                      n.Column("CIDADE");
                      n.Length(100);
                      n.NotNullable(false);
                  });
                m.Property(x => x.UF, n =>
                {
                    n.Access(Accessor.NoSetter);
                    n.Column("UF");
                    n.Length(2);
                    n.NotNullable(false);
                });
            });
            this.Property(x => x.Sexo, m =>
            {
                m.Access(Accessor.Property);
                m.Column("SEXO");
                m.NotNullable(false);
                m.Type<EnumGeneric<SexoPessoa>>();
            });
            this.Property(x => x.UsaAdocanteDiariamente, m =>
            {
                m.Access(Accessor.Property);
                m.Column("USA_ADOCANTE_DIAR");
                m.NotNullable(false);
            });
        }
    }
}
