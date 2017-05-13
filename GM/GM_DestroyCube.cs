using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_DestroyCube : MonoBehaviour {

	void OnTriggerEnter(Collider deadCube_temp){
		if (deadCube_temp.name == "DeadCube") {
			if (deadCube_temp.GetComponent<Ctrl_OnCube> ().cubeModeID == 1 || deadCube_temp.GetComponent<Ctrl_OnCube> ().cubeModeID == 3) {
				//Debug.Log ("Enter");
				Destroy (deadCube_temp.gameObject);
			}
		}
	}
}
