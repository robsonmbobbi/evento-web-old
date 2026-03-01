using EventoWeb.Nucleo.Negocio.Entidades;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class MensagemEmailPadraoMapping : ClassMapping<MensagemEmailPadrao>
    {
        public MensagemEmailPadraoMapping()
        {
            this.Table("mensagens_email_padrao");
            this.Id(x => x.Id, m =>
            {
                m.Access(NHibernate.Mapping.ByCode.Accessor.NoSetter);
                m.Column("ID_MENSAGEM_EMAIL_PADRAO");
                m.Generator(Generators.Native, g =>
                {
                    g.Params(new { sequence = "GEN_MENSAGEM_EMAIL_PADRAO" });
                });
            });

            this.Component(x => x.MensagemInscricaoConfirmada, c =>
            {
                c.Access(Accessor.NoSetter);
                c.Property(o => o.Assunto, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("ASSUNTO_INSC_CONFIRMADA");
                    m.Length(150);
                });
                c.Property(O => O.Mensagem, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("MENSAGEM_INSC_CONFIRMADA");
                    m.Type(NHibernateUtil.StringClob);
                });
            });

            this.Component(x => x.MensagemInscricaoRegistradaAdulto, c =>
            {
                c.Access(Accessor.NoSetter);
                c.Property(o => o.Assunto, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("ASSUNTO_INSC_REGISTRADA");
                    m.Length(150);
                });
                c.Property(O => O.Mensagem, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("MENSAGEM_INSC_REGISTRADA");
                    m.Type(NHibernateUtil.StringClob);
                });
            });

            this.Component(x => x.MensagemInscricaoRegistradaInfantil, c =>
            {
                c.Access(Accessor.NoSetter);
                c.Property(o => o.Assunto, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("ASSUNTO_INSC_REGISTRADA_INFANTIL");
                    m.Length(150);
                });
                c.Property(O => O.Mensagem, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("MENSAGEM_INSC_REGISTRADA_INFANTIL");
                    m.Type(NHibernateUtil.StringClob);
                });
            });

            this.Component(x => x.MensagemInscricaoCodigoAcessoAcompanhamento, c =>
            {
                c.Access(Accessor.NoSetter);
                c.Property(o => o.Assunto, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("ASSUNTO_INSC_COD_ACESSO_ACOMP");
                    m.Length(150);
                });
                c.Property(O => O.Mensagem, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("MENSAGEM_INSC_COD_ACESSO_ACOMP");
                    m.Type(NHibernateUtil.StringClob);
                });
            });

            this.Component(x => x.MensagemInscricaoCodigoAcessoCriacao, c =>
            {
                c.Access(Accessor.NoSetter);
                c.Property(o => o.Assunto, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("ASSUNTO_INSC_COD_ACESSO_CRI");
                    m.Length(150);
                });
                c.Property(O => O.Mensagem, m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.NotNullable(false);
                    m.Column("MENSAGEM_INSC_COD_ACESSO_CRI");
                    m.Type(NHibernateUtil.StringClob);
                });
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
