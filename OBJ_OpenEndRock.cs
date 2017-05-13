using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_OpenEndRock : MonoBehaviour {
	public int myRockCounter;
	public GameObject mySaveRock;

	void Start () {
		mySaveRock = GameObject.Find ("save");
		mySaveRock.SetActive (false);
	}
	// Update is called once per frame
	void Update () {
		if (myRockCounter >= 4) {
			mySaveRock.SetActive (true);
		} else {
			mySaveRock.SetActive (false);
		}
	}
}
