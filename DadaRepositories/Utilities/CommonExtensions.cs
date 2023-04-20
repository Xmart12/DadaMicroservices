using DadaRepositories.Interfaces;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace DadaRepositories.Utilities
{
    public static class CommonExtensions
    {
        public static T ConvertTo<T>(this ExpandoObject expando) where T : class, IBaseFirestoreData
        {
            if (expando == null) return default;

            //Convert Timestamp to Datetime
            expando.Where(w => w.Value is Timestamp).Select(s => s.Key).ToList().ForEach(prop =>
            {
                if (expando is IDictionary<string, object> expandoDict && expandoDict.ContainsKey(prop))
                {
                    expandoDict[prop] = ((Timestamp)expandoDict[prop]).ToDateTime();
                }
            });

            T entity = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(expando));
            
            return entity;
        }


        public static ExpandoObject ConvertToExpandoObject<T>(this T entity) where T : class, IBaseFirestoreData
        {
            var properties = typeof(T).GetProperties();

            //Convert Datetime to Datetime Timestamp
            properties.Where(w => w.PropertyType == typeof(DateTime)).ToList().ForEach(prop =>
            {
                prop.SetValue(entity, DateTime.SpecifyKind((DateTime)prop.GetValue(entity), DateTimeKind.Utc));
            });

            ExpandoObject expando = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(entity));

            return expando;
        }
    }
}
