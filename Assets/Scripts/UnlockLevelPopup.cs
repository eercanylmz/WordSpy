using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevelPopup : MonoBehaviour
{
    [System.Serializable]

    public struct CatagoryName
    {
        public string name;
        public Sprite sprite;

    };

    public GameData currentGameData;
    public List<CatagoryName> catagoryNames;
    public GameObject winPopup;
    public Image catagoryNameImage;
    void Start()
    {
        winPopup.SetActive(false);

        GameEvents.OnUnlockNextCategory += OnUnlockNextCategory;

    }
    private void OnDisable()
    {
        GameEvents.OnUnlockNextCategory -= OnUnlockNextCategory;
    }
    private void OnUnlockNextCategory()
    {
        bool captureNext=false;
        foreach (var writing in catagoryNames)
        {
            if(captureNext)
            { 
            catagoryNameImage.sprite=writing.sprite;
            captureNext =false;
                break ;
            }

            if(writing.name == currentGameData .selectedCategoryName )
            {
                captureNext = true;
            }
        }
        winPopup .SetActive(true);                          
    }
}
