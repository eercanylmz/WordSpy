using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverPopup : MonoBehaviour
{
    public GameObject gameOverPopup;
    public GameObject continueAfterAdsButton;

    void Start()
    {
        continueAfterAdsButton.GetComponent<Button>().interactable = false;
        gameOverPopup.SetActive(false);

        GameEvents.OnGameOver += ShowGameOverPopup;
    }
    private void OnDisable()
    {
        GameEvents.OnGameOver -= ShowGameOverPopup;
    }
    private void ShowGameOverPopup()
    {

        gameOverPopup.SetActive(true);
        continueAfterAdsButton .GetComponent<Button>().interactable = false;
            
    }
}
