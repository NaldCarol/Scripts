using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl_OnCube : MonoBehaviour {
	#region 此區為方塊及雷射基本資訊，供辨識用，及通用的發射基準
	[Header("1.方塊為牆(0)/石頭(1)/浮石(2)/秘石(3)中的哪一種？")]
	public int cubeModeID;
	[Header ("2.方塊發出雷射之發射點")]
	public Transform myFirePoint;
	[Header ("3.此雷射發射長度")]
	public float cubeRay_Length;
	[Header ("4.此雷射擊中之物件")]
	public RaycastHit cubeRay_Hit;
	private Ray cubeRay;
	#endregion

	#region 此區布林值供判斷行為模式用
	[Header ("↓此區布林值供判斷行為模式用↓")]
	[Header ("1.方塊正在移動嗎？")]
	public bool isMoving;
	[Header ("2.方塊正在落下嗎？")]
	public bool isFalling;
	[Header ("3.方塊需要加上鋼體嗎？")]
	public bool isAddRigidbody;
	[Header ("4.主角需要將isHeroFowardHasItem[]清空嗎？")]
	public bool cleanFowardHasItem;
	#endregion

	// Use this for initialization
	void Start () {
			myFirePoint = gameObject.transform;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (cubeModeID != 0) {
			MyRaycastHit ();
			if (isAddRigidbody) {
				if (!cleanFowardHasItem) {
					for (int i = 0; i < GameObject.Find ("myPlayer").GetComponent<Ctrl_OnPlayer> ().isHeroFowardHasItem.Length; i++) {
						GameObject.Find ("myPlayer").GetComponent<Ctrl_OnPlayer> ().isHeroFowardHasItem [i] = false;
					}
					cleanFowardHasItem = true;
				}
			}
		}
	}

	public void MyRaycastHit(){
		if (Physics.Raycast (myFirePoint.position, -myFirePoint.up, out cubeRay_Hit, cubeRay_Length)) {
			if (cubeRay_Hit.transform.name == "Plane") {
				if (!GameObject.Find ("myPlayer").GetComponent<Ctrl_OnPlayer> ().isMoving) {
					isFalling = true;
				}
				if (isFalling) {
					if (!isAddRigidbody) {
						this.gameObject.AddComponent<Rigidbody> ();
						gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
						isAddRigidbody = true;
						this.gameObject.name = "DeadCube";
					}
				}
			}
		}
		Debug.DrawRay (myFirePoint.position, -myFirePoint.up, Color.green);
	}

	public void ToFloor(){
		if (Physics.Raycast (myFirePoint.position, -myFirePoint.up, out cubeRay_Hit, cubeRay_Length)) {
			if (cubeRay_Hit.transform.name != "Plane") {
				this.transform.position=cubeRay_Hit.transform.position+new Vector3(0,1,0);		
			}
		}
	}
}
