namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface IPersistencia<T>
    {
        void Incluir(T objeto);
        void Excluir(T objeto);
        void Atualizar(T objeto);
    }

    public abstract class ARepositorio<T>
    {
        private IPersistencia<T> mPersistencia;

        public ARepositorio(IPersistencia<T> persistencia)
        {
            mPersistencia = persistencia;
        }

        public virtual void Incluir(T objeto)
        {
            mPersistencia.Incluir(objeto);
        }

        public virtual void Excluir(T objeto)
        {
            mPersistencia.Excluir(objeto);
        }

        public virtual void Atualizar(T objeto)
        {
            mPersistencia.Atualizar(objeto);
        }
    }
}
