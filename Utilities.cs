/* In order to avoid having a table in the database for a shopping cart,
we neeed to serialize and de-serialize the items as objects and store
then into a user-specific session */

using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace RobotJester.Utilities 
{
    public static class Extensions 
    {
        //ADD OBJECT TO SESSION
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            //CONVERT OBJ TO STRING
            string objAsString = JsonConvert.SerializeObject(value);
            session.SetString(key, objAsString);
        }
        //GET OBJECT FROM SESSION
        //HttpContext.Session.GetObjectFromJson<List<Products>("cart")>;
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            string strVal = session.GetString(key);
            var obj = JsonConvert.DeserializeObject<T>(strVal);
            if (obj == null)
                return default(T);
            return obj;
        }
    }
}