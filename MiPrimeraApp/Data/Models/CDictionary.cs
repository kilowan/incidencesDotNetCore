using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraApp.Data.Models
{
    public class CDictionary<TKey, TKValue, TValue>
    {
        public ICollection<TKey> Keys;
        public ICollection<TValue> Values;
        public ICollection<TKValue> KValues;

        public int Count => Keys.Count;

        public void Add(TKey key, TKValue Kvalue, TValue value)
        {
            Keys.Add(key);
            KValues.Add(Kvalue);
            Values.Add(value);
        }

        public void Clear()
        {
            Keys.Clear();
            KValues.Clear();
            Values.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            return this.Keys.Contains(key);
        }
    }
}
