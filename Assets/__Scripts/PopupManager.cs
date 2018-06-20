using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour {

    GameplayController gameplayController;

    public void Start() {
        gameplayController = FindObjectOfType<GameplayController>();
    }

    public void OnClickNext()
    {
        LeanTween.move(gameplayController.gameHelpPanel, gameplayController.panelFinalTweenPosition, 0.5f);

        LeanTween.delayedCall(gameplayController.pointsPanel, 0.5f, () =>
                              LeanTween.move(gameplayController.pointsPanel, gameplayController.helpPanelPos, 0.5f)
        );
    }

    public void OnClickOk() {
        LeanTween.move(gameplayController.pointsPanel, gameplayController.panelFinalTweenPosition, 0.5f);

        LeanTween.alpha(gameplayController.blackBG, 0, 0.5f);

        LeanTween.delayedCall(gameplayController.blackBG, 0.5f, () =>
                              {
                                  gameplayController.blackBG.SetActive(false);
                                  gameplayController.helpRead = true;
                                  gameplayController.StartGameCoroutine();
                              });
        
    }
}
