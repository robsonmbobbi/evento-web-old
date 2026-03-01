using EventoWeb.Nucleo.Negocio.Excecoes;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum EnumTipoArquivoBinario { PDF, ImagemPNG, ImagemJPEG }

    public class ArquivoBinario : Entidade
    {
        private byte[] m_Arquivo;
        private EnumTipoArquivoBinario m_Tipo;

        public ArquivoBinario(byte[] arquivo, EnumTipoArquivoBinario tipo)
        {
            Arquivo = arquivo;
            Tipo = tipo;
        }

        protected ArquivoBinario() { }

        public virtual byte[] Arquivo
        {
            get { return m_Arquivo; }
            set
            {
                if (value == null || value.Length == 0)
                    throw new ExcecaoNegocioAtributo("ArquivoBinario", "Arquivo", "Arquivo vazio");
                m_Arquivo = value;
            }
        }

        public virtual EnumTipoArquivoBinario Tipo
        {
            get { return m_Tipo; }
            set => m_Tipo = value;
        }
    }
}
