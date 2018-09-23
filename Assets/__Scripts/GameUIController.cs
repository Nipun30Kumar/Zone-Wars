using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIController : MonoBehaviour {
    public static GameUIController Instance;
    
    [SerializeField] GameObject gamehelpPanel;
    [SerializeField] GameObject pointHelpPanel;
    [SerializeField] GameObject gameoverPanel;
    
    [SerializeField] TextMeshProUGUI[] scoreTexts;
    [SerializeField] TextMeshProUGUI[] moveTexts;
    
    [SerializeField] TextMeshProUGUI[] gameoverScoreTexts;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

    }

    public void Initialize(int player1TurnCount, int player2TurnCount) {

        scoreTexts[0].text = "MOVES LEFT : " + player1TurnCount;
        scoreTexts[1].text = "MOVES LEFT : " + player2TurnCount;
        scoreTexts[0].text = "SCORE : 0";
        scoreTexts[1].text = "SCORE : 0";
    }

    public void ShowHelpPanel() {

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
