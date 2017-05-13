using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl_MysTrigger : MonoBehaviour {

	private void OnTriggerEnter (Collider other) {
		GameObject.Find ("GameManager").GetComponent<OBJ_OpenEndRock> ().myRockCounter++;
	}

	private void OnTriggerExit (Collider other) {
		GameObject.Find ("GameManager").GetComponent<OBJ_OpenEndRock> ().myRockCounter--;
	}
}
