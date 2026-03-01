using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class ConfiguracaoEmailMapping : ClassMapping<ConfiguracaoEmail>
    {
        public ConfiguracaoEmailMapping()
        {
            this.Table("configuracoes_email");
            this.Id(x => x.Id, m =>
            {
                m.Access(NHibernate.Mapping.ByCode.Accessor.NoSetter);
                m.Column("ID_CONFIGURACAO_EMAIL");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_CONFIGURACOES_EMAIL" });
                });
            });

            this.Property(x => x.EnderecoEmail, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("ENDERECO_EMAIL");
                m.Length(150);
            });
            this.Property(x => x.UsuarioEmail, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("USUARIO_EMAIL");
                m.Length(100);
            });
            this.Property(x => x.SenhaEmail, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("SENHA_EMAIL");
                m.Length(100);
            });
            this.Property(x => x.ServidorEmail, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("SERVIDOR_EMAIL");
                m.Length(50);
            });
            this.Property(x => x.PortaServidor, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("PORTA_SERVIDOR");
            });
            this.Property(x => x.TipoSeguranca, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("TIPO_SEGURANCA");
                m.Type<EnumGeneric<TipoSegurancaEmail>>();
            });            

            this.ManyToOne(x => x.Evento, m =>
              {
                  m.Access(Accessor.NoSetter);
                  m.Column("ID_EVENTO");
                  m.NotNullable(true);
              });
        }
    }
}
