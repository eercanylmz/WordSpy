using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataSelector : MonoBehaviour
{
    public GameData currentGameData;
    public GameLevelData levelData;

    void Awake()
    {
        SelectSequentalBoardData();
    }

    private void SelectSequentalBoardData()
    {
        foreach (var data in levelData.data)
        {
            if (data.catagoryName == currentGameData.selectedCategoryName)
            {
                var boardIndex = DataSaver.ReadCategoryCurrentIndexValues(currentGameData.selectedCategoryName);

                if (boardIndex < data.boardData.Count)
                {
                    currentGameData.SelectedBoardData = data.boardData[boardIndex];
                }
                else
                {
                    var randomIndex = Random.Range(0, data.boardData.Count);
                    currentGameData.SelectedBoardData = data.boardData[randomIndex];

                }
            }
        }
    }
}