using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MiPrimeraApp.Data.Models
{
    public class CDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public ICollection<TKey> Keys;
        public ICollection<TKey> Comparers;
        public ICollection<TValue> Values;
        private ICollection<KeyValuePair<TKey, TValue>> KeysValues;
        private ICollection<ColumnKeyValue<TKey, TValue>> list;
        private bool readOnly = false;

        public CDictionary()
        {
            this.Keys = new List<TKey>();
            this.Comparers = new List<TKey>();
            this.Values = new List<TValue>();
            this.KeysValues = new List<KeyValuePair<TKey, TValue>>();
            this.list = new List<ColumnKeyValue<TKey, TValue>>();
        }
        public TValue this[TKey key]
        {
            get => KeysValues
                .Where(a => a.Key.Equals(key))
                .Select(a => a.Value)
                .Single();
            set => Add(key, value);
        }

        public int Count => Keys.Count;

        public bool IsReadOnly => readOnly;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        public void Add(TKey key, TKey comparer, TValue value)
        {
            Keys.Add(key);
            Comparers.Add(comparer);
            Values.Add(value);
            KeysValues.Add(new KeyValuePair<TKey, TValue>(key, value));
            list.Add(new ColumnKeyValue<TKey, TValue>(key, comparer, value));
        }

        public void Add(TKey key, TValue value)
        {
            Keys.Add(key);
            Values.Add(value);
            KeysValues.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Keys.Add(item.Key);
            Values.Add(item.Value);
            KeysValues.Add(item);
        }

        public void Clear()
        {
            Keys.Clear();
            Comparers.Clear();
            Values.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return KeysValues.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return Keys.Contains(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < KeysValues.Count; i++)
            {
                _ = array.Append(KeysValues.ToArray()[i]);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return KeysValues.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            bool contained = ContainsKey(key);
            if (contained)
            {
                KeyValuePair<TKey, TValue> keyValue = KeysValues
                    .Where(a => a.Key.Equals(key))
                    .Single();

                Values.Remove(keyValue.Value);
                Keys.Remove(key);
                KeysValues.Remove(keyValue);
            }

            return contained;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool contained = Contains(item);
            if (contained)
            {
                Values.Remove(item.Value);
                Keys.Remove(item.Key);
                KeysValues.Remove(item);
            }

            return contained;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            bool contained = ContainsKey(key);
            value = default;
            if (contained)
            {
                value = KeysValues
                    .Where(a => a.Key.Equals(key))
                    .Select(a => a.Value)
                    .Single();
            }

            return contained;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Keys.GetEnumerator();
        }

        public ICollection<ColumnKeyValue<TKey, TValue>> Get()
        {
            return list;
        }
    }
}
