using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    public class DefaultDictionary<TKey, TValue> where TKey : notnull
    {
        readonly Dictionary<TKey, TValue> mDict;

        public Dictionary<TKey, TValue> Value => mDict;

        public DefaultDictionary()
        {
            mDict = new Dictionary<TKey, TValue>();
        }

        public TValue this[TKey key] 
        { 
            get => GetValue(key);
            set => SetValue(key, value);
        }

        private void SetValue(TKey key, TValue value)
        {
            mDict[key] = value;
        }

        private TValue GetValue(TKey key)
        {
            mDict.TryGetValue(key, out var x);
            return x;
        }
    }
}
