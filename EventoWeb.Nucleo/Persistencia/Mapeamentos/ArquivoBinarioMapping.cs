using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class ArquivoBinarioMapping : ClassMapping<ArquivoBinario>
    {
        public ArquivoBinarioMapping()
        {
            Table("arquivos_binarios");
            Id(x => x.Id, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Column("ID_ARQUIVO");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_ARQUIVO_BINARIO" });
                });
            });

            Property(x => x.Arquivo, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("ARQUIVO");
                m.Type(NHibernateUtil.BinaryBlob);
            });

            Property(x => x.Tipo, m =>
            {
                m.Access(Accessor.NoSetter);
                m.NotNullable(true);
                m.Column("TIPO_ARQUIVO");
                m.Type<EnumGeneric<EnumTipoArquivoBinario>>();
            });
        }
    }
}
