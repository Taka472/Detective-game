using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace AssemblyCSharp
{
    public class PlayerModel
    {
        public Vector3 playerPosition = new();
        public List<int> evidenceIDList = new();
        public List<string> locationList = new();
        public int currentScene;
        public List<int> firstTimeInteractionList = new();

        public static PlayerModel GetModelFromJson(string response)
        {
            PlayerModel model = JsonUtility.FromJson<PlayerModel>(response);
            return model;
        }

        public static string GetJsonFromModel(PlayerModel mode, bool pretty)
        {
            return JsonUtility.ToJson(mode, pretty);
        }
    }
}
