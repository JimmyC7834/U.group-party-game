using System;
using UnityEngine;

namespace Game.Dataset
{
    public class DataEntrySO<I> : ScriptableObject, IDataId<I> where I : Enum
    {
        [SerializeField] private string _displayName;
        [SerializeField] private I _id;

        public I id { get => _id; }
        public string displayName { get => (_displayName == "") ? _id.ToString() : _displayName; }
    }
}