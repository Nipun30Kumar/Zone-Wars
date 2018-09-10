using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public GameObject movesCountPanel;
    public GameObject inventoryPanel;
    public TMP_InputField movesCount;     

    void start()
    {        
    }

    public void OnClickPlay()
    {
        // On clicking the play button on the main menu

        movesCountPanel.SetActive(true);
        GameplayController.modeAI = false;
        LeanTween.scale(movesCountPanel, Vector3.one, 0.2f);
    }

    public void OnClickHelp()
    {
        // On clicking the help button on the main menu


    }

    public void OnClickExit()
    {
        // On clicking the exit button on the main menu
        Application.Quit();
    }

    public void OnClickGo()
    {
        // On pressing GO in the moves panel        

        if(movesCount != null && int.Parse(movesCount.text) >= 2)
        {
            int moves = int.Parse(movesCount.text);
            GameplayController.turnCount = moves * 2;
        }
        LoaadInventoryPanel();
        //SceneManager.LoadSceneAsync("Gameplay");
    }

    public void OnClickAI()
    {
        // On clicking the single player mode on the main menu.
        movesCountPanel.SetActive(true);
        GameplayController.modeAI = true;
        LeanTween.scale(movesCountPanel, Vector3.one, 0.2f);
    }

    public void OnClickClose() {
        // On clicking on the close button in the popup
        LeanTween.scale(movesCountPanel, Vector3.zero, 0.2f);
        LeanTween.delayedCall(movesCountPanel, 0.3f, () => movesCountPanel.SetActive(false));
        
    }

    public void TestMode()
    {
        // LOAD TEST CONFIGURATION : 5 MOVES EACH PLAYER.
        GameplayController.turnCount = 10;
        GameplayController.modeAI = false;
        SceneManager.LoadSceneAsync("Gameplay");
    }

    public void LoaadInventoryPanel()
    {
        inventoryPanel.SetActive(true);
        LeanTween.scale(inventoryPanel, Vector3.one, 0.2f);
    }
}
