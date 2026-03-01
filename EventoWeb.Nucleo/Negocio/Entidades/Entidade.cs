using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Entidade
    {
        private int m_Id = 0;

        public virtual int Id
        {
            get { return m_Id; }
        }

        public override bool Equals(object obj)
        {
            Entidade other = obj as Entidade;

            if (other == null)
                return false;

            if (Id == 0 && other.Id == 0)
                return (object)this == other;
            else
                return Id == other.Id;
        }

        public override int GetHashCode()
        {
            if (Id == 0)
                return base.GetHashCode();

            string stringRepresentation =
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName
                + "#" + Id.ToString();

            return stringRepresentation.GetHashCode();
        }

        public virtual Type GetTypeUnproxied()
        {
            return this.GetType();
        }
    }
}
