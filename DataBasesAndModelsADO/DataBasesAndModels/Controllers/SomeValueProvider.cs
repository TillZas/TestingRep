using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataBasesAndModels.Controllers
{
    public class SomeValueProvider : IValueProvider
    {
        public bool ContainsPrefix(string prefix)
        {
            return (string.Compare("someValue", prefix, true) == 0 || string.Compare("someValues", prefix, true) == 0);
        }

        private int[] valueSet(string key)
        {
            Random rnd = new Random();
            if (string.Compare("someValue", key, true) == 0)
            {
                return new int[] { rnd.Next(10) };
            }
            else if (string.Compare("someValues", key, true) == 0)
            {
                int lng = rnd.Next(10);
                int[] res = new int[lng];
                for(int i = 0; i < lng; i++)
                {
                    if (i < 2) res[i] = rnd.Next(1000);
                    else res[i] = (int)Math.Abs(res[i - 2] - res[i - 1]);
                }
                return res;
            }
            else return null;
        }

        public ValueProviderResult GetValue(string key)
        {
            if (ContainsPrefix(key))
            {
                string res = "";
                int[] lst = valueSet(key);
                res += lst.Length == 1 ? "Ваше значение: " : "Ваши значения: ";
                for(int i = 0; i < lst.Length; i++)
                {
                    if (i == 0) res += lst[i];
                    else res += ", " + lst[i];
                }
                res += ".";
                return new ValueProviderResult(res,null,CultureInfo.InvariantCulture);
            }
            return null;
        }
    }

    public class SomeValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            return new SomeValueProvider();
        }
    }
}