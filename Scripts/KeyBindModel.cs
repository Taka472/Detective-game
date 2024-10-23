using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace AssemblyCSharp
{
    [Serializable]
    public class KeyBindModel
    {
        public List<KeyValue> keyDictionary = new();

        [Serializable]
        public class KeyValue
        {
            public string key;
            public string value;

            public KeyValue(string key, string value)
            {
                this.key = key;
                this.value = value;
            }
        }

        public static KeyBindModel GetModelFromJson(string response)
        {
            KeyBindModel model = JsonUtility.FromJson<KeyBindModel>(response);
            return model;
        } 

        public static string GetJsonFromModel(KeyBindModel model, bool pretty)
        {
            return JsonUtility.ToJson(model, pretty);
        }
    }
}
