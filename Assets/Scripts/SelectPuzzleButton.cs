 
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectPuzzleButton : MonoBehaviour
{
    public GameData gameData;
    public GameLevelData levelData;
    public Text catagoryText;
    public Image progressBarFilling;

    private string gameSceneName = "GameScene";

    private bool _levelLocked;

    void Start()
    {
        _levelLocked = false;
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        UpdateButtonInformatýon();

        if (_levelLocked)
        {
            button.enabled = false;
        }
        else
        {
            button.enabled = true;
        } 
    } 
    private void InterstitialAdsClosed()
    {

    } 
    private void UpdateButtonInformatýon()
    {
        var currentIndex = -1;
        var totalBorads = 0;

        foreach (var data in levelData.data)
        {
            if (data.catagoryName == gameObject.name)
            {
                currentIndex = DataSaver.ReadCategoryCurrentIndexValues(gameObject.name);
                totalBorads = data.boardData.Count;

                if (levelData.data[0].catagoryName == gameObject.name && currentIndex < 0)
                {
                    DataSaver.SaveCategoryData(levelData.data[0].catagoryName, 0);
                    currentIndex = DataSaver.ReadCategoryCurrentIndexValues(gameObject.name);
                    totalBorads = data.boardData.Count;

                }
            }
        }

        if (currentIndex == -1)
        {
            _levelLocked = true;
        }


        catagoryText.text = _levelLocked ? string.Empty : (currentIndex.ToString() + "/" + totalBorads.ToString());
        progressBarFilling.fillAmount =
            (currentIndex > 0 && totalBorads > 0) ? ((float)currentIndex / (float)totalBorads) : 0f;
    }
    private void OnButtonClick()
    {
        gameData.selectedCategoryName = gameObject.name; 
        SceneManager.LoadScene(gameSceneName);
    }
}