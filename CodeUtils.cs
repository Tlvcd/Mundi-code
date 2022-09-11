using UnityEngine;
using System.Collections.Generic;
using System.Linq;


namespace Axis.Utilities
{
    public static class CodeUtils
    {
        

        public static Color ModifyAlpha(this Color col, float alpha)
        {
            return new Color(col.r, col.g, col.b, alpha);
        }

        public static Color32 ModifyAlpha(this Color32 col, float alpha)
        {
            return new Color(col.r, col.g, col.b, alpha);
        }

        public static Dictionary<Tkey, float> AddDictionaries<Tkey>(Dictionary<Tkey, float> org, Dictionary<Tkey, float> outside)
            {
            var AllKeys = org.Keys.Union(outside.Keys);

            var res3 = AllKeys.ToDictionary(key => key,
                key => (org.Keys.Contains(key) ? org[key] : 0) +
                (outside.Keys.Contains(key) ? outside[key] : 0));
            return res3;
        }

        public static Dictionary<Tkey, float> SubtractDictionaries<Tkey>(Dictionary<Tkey, float> org, Dictionary<Tkey, float> outside)
        {
            var AllKeys = org.Keys.Union(outside.Keys);

            var res3 = AllKeys.ToDictionary(key => key,
                key => (org.Keys.Contains(key) ? org[key] : 0) -
                (outside.Keys.Contains(key) ? outside[key] : 0));
            return res3;
        }


    }

}
