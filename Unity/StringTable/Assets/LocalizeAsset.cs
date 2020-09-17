using System;
using System.Collections.Generic;
using UnityEngine;

namespace SAM.LOCALIZE
{
    public class LocalizeAsset : ScriptableObject
    {
        [Serializable]
        public class Data
        {
            public string key;
            public string value;
        }
        
        [HideInInspector]
        public string languageCode;
        [HideInInspector]
        public List<Data> table = new List<Data>();
    }
}