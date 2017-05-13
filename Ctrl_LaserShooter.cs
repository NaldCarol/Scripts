using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl_LaserShooter : MonoBehaviour {
	#region 此區為雷射基本資訊，供分流、辨識用，及通用的發射基準

	[Header ("↓此區為雷射基本資訊，供分流、辨識用，及通用的發射基準↓")]
	[Header ("1.雷射模式為向下(0)/隔一格向下(1)/斜角向下(2)哪一個？")]
	public int myLaseMod;
	[Header ("2.此雷射之父物件")]
	public GameObject myFather;
	[Header ("3.此雷射之發射點")]
	public Transform myFirePoint;
	[Header ("4.此雷射為前(0)/後(1)/左(2)/右(3)哪一個？")]
	public int myFWDID;
	#endregion

	#region 此區為0號CanMove雷射基本資訊，使雷射正確產生
	[Header ("↓↓↓此區為0號CanMove雷射基本資訊，使雷射正確產生↓")]
	[Header ("1.此雷射發射長度")]
	public float canMoveRay_Length;
	[Header ("2.此雷射擊中之物件")]
	public RaycastHit canMoveRay_Hit;
	[Header ("3.此雷射之發射點與擊中之物件之距離")]
	public float canMoveRay_Dis;
	#endregion

	#region 此區為1號CanPush雷射基本資訊，使雷射正確產生
	[Header ("↓↓↓此區為1號CanPark雷射基本資訊，使雷射正確產生↓")]
	[Header ("1.此雷射發射長度")]
	public float canParkRay_Length;
	[Header ("2.此雷射擊中之物件")]
	public RaycastHit canParkRay_Hit;
	[Header ("3.此雷射之發射點與擊中之物件之距離")]
	public float canParkRay_Dis;
	#endregion

	#region 此區為2號CanThrow雷射基本資訊，使雷射正確產生
	[Header ("↓↓↓此區為2號CanThrow雷射基本資訊，使雷射正確產生↓")]
	[Header ("1.此雷射發射長度")]
	public float canThrowRay_Length;
	[Header ("2.此雷射擊中之物件")]
	public RaycastHit canThrowRay_Hit;
	[Header ("3.此雷射之發射點與擊中之物件之距離")]
	public float canThrowRay_Dis;
	#endregion




	// Use this for initialization
	void Start () {
		myFather = gameObject.transform.parent.gameObject;
		myFirePoint = gameObject.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!GameObject.Find ("myPlayer").GetComponent<Ctrl_OnPlayer> ().isMoving) {
			switch (myLaseMod) {
			case 0:
				Mod_0_CanMove ();
				break;
			case 1:
				Mod_1_CanPush ();
				break;
			case 2:
				Mod_2_CanThrow (myFWDID);
				break;
			}
		}
	}

	public void Mod_0_CanMove () {
		if (Physics.Raycast (myFirePoint.position, -myFirePoint.up, out canMoveRay_Hit, canMoveRay_Length)) {
			Mod_0_HitFloorSwitcher ();
		} else {
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroGoFoward [myFWDID] = false;
		}
		Debug.DrawRay (myFirePoint.position, -myFirePoint.up, Color.red);
	}

	public void Mod_0_HitFloorSwitcher () {
		canMoveRay_Dis = Vector3.Distance (canMoveRay_Hit.transform.position, myFirePoint.transform.position);
		switch (canMoveRay_Hit.transform.tag) {
		case "Plane":
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroGoFoward [myFWDID] = false;
			break;
		case "CanMoveCube":
			SwitcherForCanMoveCube (myFWDID, canMoveRay_Dis);
			break;
		case"NPC":
			SwitcherForNPC (myFWDID, canMoveRay_Hit);
			break;
		case "Floor":
		default :
			SwitcherForDefault (myFWDID, canMoveRay_Dis);
			break;
		}
	}

	public void SwitcherForCanMoveCube (int fwdID_Temp, float dis_Temp) {
		if (dis_Temp < 7.5f) {//8以下
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroGoFoward [fwdID_Temp] = false;
			myFather.GetComponent<Ctrl_OnPlayer> ().isHeroFowardHasItem [fwdID_Temp] = true;

		}else if(dis_Temp >= 7.5f && dis_Temp < 8.5f){
			if(canMoveRay_Hit.transform.GetComponent<Ctrl_OnCube>().cubeModeID==3){
				myFather.GetComponent<Ctrl_OnPlayer> ().canHeroGoFoward [fwdID_Temp] = false;
				myFather.GetComponent<Ctrl_OnPlayer> ().isHeroFowardHasItem [fwdID_Temp] = true;
				if (canMoveRay_Hit.transform.GetComponent<Ctrl_OnCube> ().isFalling) {
					myFather.GetComponent<Ctrl_OnPlayer> ().myReadyMoveCube = null;
				} else {
					#region 字元轉換成數字
					char fatherFaceID = myFather.GetComponent<Ctrl_OnPlayer> ().heroForwardID;
					int fatherFaceID_int;
					if (fatherFaceID == 'W') {
						fatherFaceID_int = 0;
					} else if (fatherFaceID == 'S') {
						fatherFaceID_int = 1;
					} else if (fatherFaceID == 'A') {
						fatherFaceID_int = 2;
					} else if (fatherFaceID == 'D') {
						fatherFaceID_int = 3;
					} else {
						fatherFaceID_int = -1;
					}
					#endregion
					if (fatherFaceID_int == myFWDID) {
						myFather.GetComponent<Ctrl_OnPlayer> ().myReadyMoveCube = canMoveRay_Hit.transform.gameObject;
					} 
				}

			}		
		} else if (dis_Temp >= 8.5f && dis_Temp < 9.5f) {//9
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroGoFoward [fwdID_Temp] = true;
			myFather.GetComponent<Ctrl_OnPlayer> ().isHeroFowardHasItem [fwdID_Temp] = true;

			if (canMoveRay_Hit.transform.GetComponent<Ctrl_OnCube> ().isFalling) {
				myFather.GetComponent<Ctrl_OnPlayer> ().myReadyMoveCube = null;
			} else {
				#region 字元轉換成數字
				char fatherFaceID = myFather.GetComponent<Ctrl_OnPlayer> ().heroForwardID;
				int fatherFaceID_int;
				if (fatherFaceID == 'W') {
					fatherFaceID_int = 0;
				} else if (fatherFaceID == 'S') {
					fatherFaceID_int = 1;
				} else if (fatherFaceID == 'A') {
					fatherFaceID_int = 2;
				} else if (fatherFaceID == 'D') {
					fatherFaceID_int = 3;
				} else {
					fatherFaceID_int = -1;
				}
				#endregion
				if (fatherFaceID_int == myFWDID) {
					myFather.GetComponent<Ctrl_OnPlayer> ().myReadyMoveCube = canMoveRay_Hit.transform.gameObject;
				} 
			}

		} else if (dis_Temp >= 9.5f && dis_Temp < 11.5f) {//10&11
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroGoFoward [fwdID_Temp] = true;
			myFather.GetComponent<Ctrl_OnPlayer> ().isHeroFowardHasItem [fwdID_Temp] = false;
		} else {
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroGoFoward [fwdID_Temp] = false;
			myFather.GetComponent<Ctrl_OnPlayer> ().isHeroFowardHasItem [fwdID_Temp] = false;
		}
		myFather.GetComponent<Ctrl_OnPlayer> ().heroWASDVector3 [fwdID_Temp] = canMoveRay_Hit.transform.position;
	}

	public void SwitcherForNPC (int fwdID_Temp, RaycastHit hit_Temp){
		if (hit_Temp.transform.parent == null) {
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroGoFoward [fwdID_Temp] = false;
			GameObject npc_Temp = GameObject.Find (hit_Temp.transform.name);
			//npc_Temp.GetComponent<TestFollow> ().talkToNPCSwitch ();
		} else {
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroGoFoward [fwdID_Temp] = true;
			GameObject npc_Temp = GameObject.Find (hit_Temp.transform.name);
			//npc_Temp.GetComponent<TestFollow> ().talkToNPCSwitch ();
		}
		myFather.GetComponent<Ctrl_OnPlayer> ().heroWASDVector3 [fwdID_Temp] = canMoveRay_Hit.transform.position;

	}

	public void SwitcherForDefault (int fwdID_Temp, float dis_Temp) {
		if (dis_Temp < 8.5f) {
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroGoFoward [fwdID_Temp] = false;
			myFather.GetComponent<Ctrl_OnPlayer> ().isHeroFowardHasItem [fwdID_Temp] = true;
		} else if (dis_Temp >= 8.5f && dis_Temp < 9.5f) {
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroGoFoward [fwdID_Temp] = true;
			myFather.GetComponent<Ctrl_OnPlayer> ().isHeroFowardHasItem [fwdID_Temp] = true;
		} else if (dis_Temp >= 9.5f && dis_Temp < 11.5f) {
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroGoFoward [fwdID_Temp] = true;
			myFather.GetComponent<Ctrl_OnPlayer> ().isHeroFowardHasItem [fwdID_Temp] = false;
		} else {
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroGoFoward [fwdID_Temp] = false;
			myFather.GetComponent<Ctrl_OnPlayer> ().isHeroFowardHasItem [fwdID_Temp] = false;
		}
		myFather.GetComponent<Ctrl_OnPlayer> ().heroWASDVector3 [fwdID_Temp] = canMoveRay_Hit.transform.position;
	}

	public void Mod_1_CanPush () {
		if (Physics.Raycast (myFirePoint.position, -myFirePoint.up, out canParkRay_Hit, canParkRay_Length)) {
			Mod_1_HitFloorSwitcher (myFWDID);
		}else {
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroPushCube [myFWDID] = false;
		}
		Debug.DrawRay (myFirePoint.position, -myFirePoint.up, Color.blue);
	}

	public void Mod_1_HitFloorSwitcher (int ID) {
		canParkRay_Dis = Vector3.Distance (canParkRay_Hit.transform.position, myFirePoint.transform.position);
		switch (canParkRay_Hit.transform.tag) {
		case "Plane":
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroPushCube [ID] = true;
			break;
		case "CanMoveCube":
			if (canParkRay_Dis > 9.5 ) {
				myFather.GetComponent<Ctrl_OnPlayer> ().canHeroPushCube [ID] = true;
			} else {
				myFather.GetComponent<Ctrl_OnPlayer> ().canHeroPushCube [ID] = false;
			}
			break;
		case "NPC":
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroPushCube [ID] = false;
			break;
		case "Floor":
		default :
			if (canParkRay_Dis < 9.5f) {
				myFather.GetComponent<Ctrl_OnPlayer> ().canHeroPushCube [ID] = false;
			} else {
				myFather.GetComponent<Ctrl_OnPlayer> ().canHeroPushCube [ID] = true;
			}
			break;
		}
	}

	public void Mod_2_CanThrow (int ID) {
		if (Physics.Raycast (myFirePoint.position, -myFirePoint.up, out canThrowRay_Hit, canThrowRay_Length)) {
			if (canThrowRay_Hit.transform.tag == "NPC") {
				myFather.GetComponent<Ctrl_OnPlayer> ().canHeroThrowCube [ID] = false;
			} else {
				myFather.GetComponent<Ctrl_OnPlayer> ().canHeroThrowCube [ID] = true;
				}
			canThrowRay_Dis = Vector3.Distance (canThrowRay_Hit.transform.position, myFirePoint.transform.position);
		} else {
			myFather.GetComponent<Ctrl_OnPlayer> ().canHeroThrowCube [myFWDID] = false;
		}
		Debug.DrawRay (myFirePoint.position, -myFirePoint.up, Color.magenta);
	}
}
