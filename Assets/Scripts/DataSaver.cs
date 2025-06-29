using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    public static int ReadCategoryCurrentIndexValues(string name)
    {
        var value = -1;
        if (PlayerPrefs.HasKey(name))
            value = PlayerPrefs.GetInt(name);

        return value;

    }
    public static void SaveCategoryData(string categoryName, int currentIndex)
    {
        PlayerPrefs.SetInt(categoryName, currentIndex);
        PlayerPrefs.Save();

    }
    public static void clearGameData(GameLevelData levelData)
    {
        foreach (var data in levelData.data)
        {
            PlayerPrefs.SetInt(data.catagoryName, -1);
        }
        PlayerPrefs.SetInt(levelData.data[0].catagoryName, 0);
        PlayerPrefs.Save();
    }
}