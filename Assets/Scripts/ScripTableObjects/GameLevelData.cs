using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu]
public class GameLevelData : ScriptableObject
{
    [System .Serializable]
    public struct CategoryRecord
    {
        public string catagoryName;
        public List <BoardData> boardData;

    }
    public List<CategoryRecord> data;
}
