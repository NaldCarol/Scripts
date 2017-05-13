using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_TogLog : MonoBehaviour {


	 
	void OnTriggerStay(Collider item){
		if (item.gameObject.GetComponent<Ctrl_OnCube> ()) {
			if (item.gameObject.GetComponent<Ctrl_OnCube> ().cubeModeID == 1 || item.gameObject.GetComponent<Ctrl_OnCube> ().cubeModeID == 2) {
				if (Input.GetKey (KeyCode.CapsLock)) {
					CloseLogPan ();
				} else {
					if (!GameObject.Find ("HeadPanel_Stone")) {
						GM_UIManager.Instance.ShowPanel ("HeadPanel_Stone");
					}
				}
			}  
			if (item.gameObject.GetComponent<Ctrl_OnCube> ().cubeModeID == 3) {
				if (Input.GetKey (KeyCode.CapsLock) || Input.GetKey (KeyCode.LeftShift)) {
					CloseLogPan ();
				} else {
					if (!GameObject.Find ("HeadPanel_Mys")) {
						GM_UIManager.Instance.ShowPanel ("HeadPanel_Mys");
					}
				}
			}
		} 
	}

	void OnTriggerExit(){
		CloseLogPan ();
	}

	public void CloseLogPan(){
		if (GameObject.Find ("HeadPanel_Mys")) {
			GM_UIManager.Instance.ClosePanel ("HeadPanel_Mys");
		}
		if (GameObject.Find ("HeadPanel_Stone")) {
			GM_UIManager.Instance.ClosePanel ("HeadPanel_Stone");
		}
	}
}
