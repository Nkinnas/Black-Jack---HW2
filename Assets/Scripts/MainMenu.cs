using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    public GameObject RulesPanel;
    public GameObject CardBackPanel; 
    private string selectedCardColor = "Red";

    public void StartGame()
    {

        PlayerPrefs.SetString("SelectedCardColor", selectedCardColor);
        SceneManager.LoadScene("GAMESCENE"); 
    }

    public void ShowRules()
    {
        RulesPanel.SetActive(true);

    }

    public void HideRules()
    {
        RulesPanel.SetActive(false);
    }
    public void ShowCardBackPanel()
    {
        CardBackPanel.SetActive(true);
    }


    public void HideCardBackPanel()
    {
        CardBackPanel.SetActive(false);
    }


    public void SelectRedCardBack()
    {
        selectedCardColor = "Red";
    }
    public void SelectBlueCardBack()
    {
        selectedCardColor = "Blue";
    }
    public void SelectGrayCardBack()
    {
        
        selectedCardColor = "Gray";
    }
}
