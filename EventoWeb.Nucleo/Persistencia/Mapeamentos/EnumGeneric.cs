using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Mapeamentos
{
    public class EnumGeneric<TipoEnum> : IUserType
    {
        public Boolean IsMutable { get { return false; } }
        public Type ReturnedType { get { return typeof(TipoEnum); } }
        public SqlType[] SqlTypes { get { return new[] { SqlTypeFactory.Int16 }; } }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public new bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x == null ? typeof(TipoEnum).GetHashCode() : x.GetHashCode();
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var tmp = NHibernateUtil.Int32.NullSafeGet(rs, names[0], session);

            if (tmp == null)
                return null;
            else
                return Enum.Parse(typeof(TipoEnum), tmp.ToString());
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            if (value == null)
            {
                ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                ((IDataParameter)cmd.Parameters[index]).Value = (Int32)value;
            }
        }
    }
}
