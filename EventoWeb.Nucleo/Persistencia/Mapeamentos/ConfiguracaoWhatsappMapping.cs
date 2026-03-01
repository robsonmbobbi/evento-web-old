using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class ConfiguracaoWhatsappMapping : ClassMapping<ConfiguracaoWhatsapp>
    {
        public ConfiguracaoWhatsappMapping()
        {
            this.Table("configuracoes_whatsapp");
            this.Id(x => x.Id, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_CONFIGURACOES_WHATSAPP" });
                });
            });

            this.Property(x => x.Instancia, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("INSTANCIA");
                m.Length(400);
            });

            this.Property(x => x.HostApi, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("HOST_API");
                m.Length(400);
            });

            this.Property(x => x.ChaveApi, m => {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("CHAVE_API");
                m.Length(400);
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
