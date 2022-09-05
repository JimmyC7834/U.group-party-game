using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Dataset
{
    public interface IDataId<I> where I : Enum
    {
        I id { get; }
    }

    public class DataSetSO<I, V> : ScriptableObject where I : Enum where V : ScriptableObject, IDataId<I>
    {
        [SerializeField] private V[] _dataEntry;
        private Dictionary<I, V> _data;

        public V this[I id]
        {
            get
            {
                if (_data == null)
                    InitializeDataSet();
                return _data[id];
            }
        }

        private void InitializeDataSet()
        {
            _data = new Dictionary<I, V>();
            for (int i = 0; i < _dataEntry.Length; i++)
            {
                I id = _dataEntry[i].id;
                V value = _dataEntry[i];

                if (id == null || value == null)
                {
                    Debug.LogWarning($"Null id or value in data entry at index {i}: {id}, {value}");
                    continue;
                }
                
                if (_data.ContainsKey(id))
                {
                    Debug.LogWarning($"Value with Id {id}, {_data[id]} being overrided with {value}");
                    _data[id] = value;
                    continue;
                }
                
                _data.Add(id, value);
            }
        }
    }
}