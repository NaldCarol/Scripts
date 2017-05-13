using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LTEGT_onFinishTrigger : MonoBehaviour {
    public GameObject gameManager;
	public GameObject scorePanel;
	public int npcNumInt;
	public int totalTimeInt;
	public char rankChar; 

	void Start(){
		gameManager = GameObject.Find ("GameManager");
	}
    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.name == "myPlayer") {
			gameManager.GetComponent<GM_CanvasManager> ().MisCom ();
			npcNumInt=gameManager.GetComponent<GM_GameManager> ().SaveScore ();
			totalTimeInt=gameManager.GetComponent<GM_GameManager> ().ReadTime ();
			scorePanel = GameObject.Find ("ScorePanel");
			StartCoroutine (scorePanel.GetComponent<GM_ScoreCounting> ().DisplayNPCNum (npcNumInt,totalTimeInt));
			rankChar=gameManager.GetComponent<GM_GameManager> ().RankCounter(npcNumInt,totalTimeInt);
			StartCoroutine (scorePanel.GetComponent<GM_ScoreCounting> ().DisplayRank (rankChar));
		}
    }
}
