using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Caching
{
 
    static class CacheKeySupport
    {
        public static readonly Type[] List;

        static CacheKeySupport()
        {
            var types = new[]
                          {
                              typeof(Guid),                              
                              typeof (String),
                              typeof (UInt32),
                              typeof (UInt64),
                              typeof (Char),
                              typeof (Boolean),                           
                              typeof (Int16),
                              typeof (Int32),
                              typeof (Int64),
                              typeof (Single),
                              typeof (Double),
                              typeof (Decimal),                             
                              typeof (UInt16),                           
                              typeof (Enum)
                          };


            var nullTypes = from t in types
                            where t.IsValueType
                            select typeof(Nullable<>).MakeGenericType(t);

            List = types.Concat(nullTypes).ToArray();
        }

        public static bool IsSupported(Type type)
        {
            return (List.Any(x => x.IsAssignableFrom(type)));         
        }
    }
}
