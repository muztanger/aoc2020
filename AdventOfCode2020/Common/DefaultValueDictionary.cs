﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    public class DefaultValueDictionary<TKey, TValue>
        where TKey : notnull
    {
        readonly Dictionary<TKey, TValue> mDict;
        private readonly Func<TValue> mDefaultValueFactory;

        public DefaultValueDictionary(Func<TValue> defaultValueFactory)
        {
            mDict = new Dictionary<TKey, TValue>();
            mDefaultValueFactory = defaultValueFactory;
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
            if (mDict.ContainsKey(key))
            {
                return mDict[key];
            }
            var result = mDefaultValueFactory();
            mDict[key] = result;
            return result;
        }
    }
}