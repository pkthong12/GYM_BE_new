using System.Data;
using System.Reflection;

namespace GYM_BE.Core.DataContract
{
    public static class MsSQLStaticHelper
    {
        public static List<T> ToList<T>(this DataTable dt)
        {
            List<T> list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                list.Add(item);
            }

            return list;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type typeFromHandle = typeof(T);
            T val = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                PropertyInfo[] properties = typeFromHandle.GetProperties();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    if (propertyInfo.Name == column.ColumnName)
                    {
                        propertyInfo.SetValue(val, (dr[column.ColumnName] == DBNull.Value) ? null : dr[column.ColumnName], null);
                    }
                }
            }

            return val;
        }
    }
}
