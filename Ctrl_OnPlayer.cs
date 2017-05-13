using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl_OnPlayer : MonoBehaviour {
	#region 此區供主角決定實際移動方向及步數用

	[Header ("↓此區供主角決定實際移動方向及步數用↓")]
	[Header ("主角面向方位ID")]
	public char heroForwardID;
	//WSAD
	[Header ("主角前後左右之座標")]
	public Vector3[] heroWASDVector3 = new Vector3[4];
	[Header ("主角前次座標")]
	public Vector3 heroLastPos;
	[Header ("主角最終決定移動座標")]
	public Vector3 heroMoveWayTarget;
	[Header ("主角基本移動速度")]
	public float moveSpeed_Basic;
	[Header ("主角實際移動速度")]
	public float moveSpeed_Recent;
	[Header ("↑此區供主角決定實際移動方向及步數用↑")]

	#endregion

	#region 此區布林值供1.判斷行為模式、2.切換動畫用
	[Header ("↓此區布林值供1.判斷行為模式、2.切換動畫用↓")]
	[Header ("1.主角正在行走嗎？")]
	public bool isMoving;
	[Header ("2.主角正在推箱子嗎？")]
	public bool isMovingCube;
	[Header ("3.主角在推箱子嗎？")]
	public bool isPush;
	[Header ("4.主角在拉箱子嗎？")]
	public bool isPull;
	//只用於動畫控制
	[Header ("5.主角在跳嗎？")]
	public bool isJumping;
	[Header ("6.主角在丟嗎？")]
	public bool isThrow;
	[Header ("↑此區布林值供1.判斷行為模式、2.切換動畫用↑")]

	#endregion

	#region 此區儲放雷射物件，供接下來的布林值抓值用
	[Header ("↓此區儲放雷射物件，供接下來的布林值抓值用↓")]
	[Header ("1.用於偵測地板物件、抓取角色正面")]
	public GameObject[] detectFloorAndHeroFwdPoint = new GameObject[4];
	[Header ("2.用於偵測四個斜角落，是否有位子讓方塊停")]
	public GameObject[] detect4CorHasItem = new GameObject[4];
	[Header ("3.用於偵測方塊背後，是否有位子讓方塊停")]
	public GameObject[] detectBehindCubeHasItem = new GameObject[4];
	[Header ("↑此區儲放雷射物件，供接下來的布林值抓值用↑")]
	#endregion

	#region 此區供雷射物件回傳其是否有打到物體用

	[Header ("↓此區供雷射物件回傳其是否有打到物體用↓")]
	[Header ("主角面向的前方……")]
	[Header ("1.是可走的地形嗎？")]
	public bool[] canHeroGoFoward = new bool[4];
	[Header ("2.有障礙物嗎？")]
	public bool[] isHeroFowardHasItem = new bool[4];
	[Header ("3.欲推方向，有位子讓方塊停嗎？")]
	public bool[] canHeroPushCube = new bool[4];
	[Header ("4.欲丟方向，有位子讓方塊停嗎？")]
	public bool[] canHeroThrowCube = new bool[4];
	[Header ("↑此區供雷射物件回傳其是否有打到物體用↑")]

	#endregion

	[Header ("預先載入之可推物件")]
	public GameObject myReadyMoveCube;
	[Header ("模型本尊")]
	public GameObject faynaMod;
	[Header ("模型動畫")]
	public Animator faynaAni;

	public GameObject tog;

	void Start () {
		faynaAni = faynaMod.GetComponent<Animator> ();
		moveSpeed_Recent = moveSpeed_Basic;
	}
	
	// Update is called once per frame
	void Update () {
		#region 判斷行為模式用，將由主要輸入函數起動相關布林值，再由主要移動函數關閉、清空
		HeroRotation ();
		if (isMoving) {
			if (isMovingCube) {
				if (isPush) {
					faynaAni.SetBool ("isPush", true);
					moveSpeed_Recent = moveSpeed_Basic * 0.25f;
				} else {
					faynaAni.SetBool ("isPull", true);
					moveSpeed_Recent = moveSpeed_Basic * 0.25f;
				}
			} else {
				//這裡不確定是要放在區段裡，還是用else if
				if (isJumping) {
					faynaAni.SetBool ("isJumping", true);
				} else if(isThrow){
					faynaAni.SetBool ("isThrow", true);
				}else{
					faynaAni.SetBool ("isMoving", true);
				}
			}
			HeroMoveToTargetAndReset (heroMoveWayTarget);
		} else {
			faynaAni.SetBool ("isPush", false);
			faynaAni.SetBool ("isPull", false);
			faynaAni.SetBool ("isJumping", false);
			faynaAni.SetBool ("isThrow", false);
			faynaAni.SetBool ("isMoving", false);
			PlayerInput ();
		}
		#endregion
	}


	#region +定義用函式：按住方向鍵時，相當於哪個方向(字元)

	public char InputDefinition () {
		char InputType = 'N';
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
			InputType = 'W';
		} else if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			InputType = 'S';
		} else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
			InputType = 'A';
		} else if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			InputType = 'D';
		} else {
			InputType = 'N';
		}
		return InputType;
	}

	#endregion

	#region ◎ 判斷玩家輸入的模式種類

	//玩家輸入的模式種類如下：
	//1.　 Z+WSAD
	//2.　 X+WSAD(待處理)
	//3-1.  WSAD
	//3-2.  WSAD+Space

	public void PlayerInput () {
		if (Input.GetKey (KeyCode.CapsLock)) {
			int heroFWD_Int;
			switch (heroForwardID) {
			case 'W':
				if (InputDefinition () == 'W') {
					heroFWD_Int = 0;
					PushEvent (heroFWD_Int);
				} else if (InputDefinition () == 'S') {
					heroFWD_Int = 1;
					PullEvent (heroFWD_Int);
				}
				break;
			case 'S':
				if (InputDefinition () == 'S') {
					heroFWD_Int = 1;
					PushEvent (heroFWD_Int);
				} else if (InputDefinition () == 'W') {
					heroFWD_Int = 0;
					PullEvent (heroFWD_Int);
				}
				break;
			case 'A':
				if (InputDefinition () == 'A') {
					heroFWD_Int = 2;
					PushEvent (heroFWD_Int);
				} else if (InputDefinition () == 'D') {
					heroFWD_Int = 3;
					PullEvent (heroFWD_Int);
				}
				break;
			case 'D':
				if (InputDefinition () == 'D') {
					heroFWD_Int = 3;
					PushEvent (heroFWD_Int);
				} else if (InputDefinition () == 'A') {
					heroFWD_Int = 2;
					PullEvent (heroFWD_Int);
				}
				break;
			default:
				break;
			}
		} else if (Input.GetKey (KeyCode.LeftShift)) {
			int needRayMod;
			int needRayNum;
			switch (heroForwardID) {
			case 'W':
				if (InputDefinition () == 'W') {
					needRayMod = 0;
					needRayNum = 0;
					ThrowEvent (needRayMod, needRayNum);
				} else if (InputDefinition () == 'A') {
					needRayMod = 1;
					needRayNum = 0;
					ThrowEvent (needRayMod, needRayNum);
				} else if (InputDefinition () == 'D') {
					needRayMod = 1;
					needRayNum = 1;
					ThrowEvent (needRayMod, needRayNum);
				}
				break;
			case 'S':
				if (InputDefinition () == 'S') {
					needRayMod = 0;
					needRayNum = 1;
					ThrowEvent (needRayMod, needRayNum);
				} else if (InputDefinition () == 'A') {
					needRayMod = 1;
					needRayNum = 2;
					ThrowEvent (needRayMod, needRayNum);
				} else if (InputDefinition () == 'D') {
					needRayMod = 1;
					needRayNum = 3;
					ThrowEvent (needRayMod, needRayNum);
				}
				break;
			case 'A':
				if (InputDefinition () == 'A') {
					needRayMod = 0;
					needRayNum = 2;
					ThrowEvent (needRayMod, needRayNum);
				} else if (InputDefinition () == 'W') {
					needRayMod = 1;
					needRayNum = 0;
					ThrowEvent (needRayMod, needRayNum);
				} else if (InputDefinition () == 'S') {
					needRayMod = 1;
					needRayNum = 2;
					ThrowEvent (needRayMod, needRayNum);
				}
				break;
			case 'D':
				if (InputDefinition () == 'D') {
					needRayMod = 0;
					needRayNum = 3;
					ThrowEvent (needRayMod, needRayNum);
				} else if (InputDefinition () == 'W') {
					needRayMod = 1;
					needRayNum = 1;
					ThrowEvent (needRayMod, needRayNum);
				} else if (InputDefinition () == 'S') {
					needRayMod = 1;
					needRayNum = 3;
					ThrowEvent (needRayMod, needRayNum);
				}
				break;
			default:
				break;
			}
		} else {
			int heroFWD_Int;
			int heroLastID_Int;
			#region ● 跳躍+移動&&不跳躍，單純移動(在IEnumerator里判別)
			if (InputDefinition () == 'W') {
				heroForwardID = 'W';
				heroFWD_Int = 0;
				heroLastID_Int = 1;
				WalkJumpEvent (heroFWD_Int, heroLastID_Int);
			}
			if (InputDefinition () == 'S') {
				heroForwardID = 'S';
				heroFWD_Int = 1;
				heroLastID_Int = 0;
				WalkJumpEvent (heroFWD_Int, heroLastID_Int);
			}
			if (InputDefinition () == 'A') {
				heroForwardID = 'A';
				heroFWD_Int = 2;
				heroLastID_Int = 3;
				WalkJumpEvent (heroFWD_Int, heroLastID_Int);
			}
			if (InputDefinition () == 'D') {
				heroForwardID = 'D';
				heroFWD_Int = 3;
				heroLastID_Int = 2;
				WalkJumpEvent (heroFWD_Int, heroLastID_Int);
			}
			#endregion
		}
	}

	#endregion

	#region ◎ 將玩家輸入轉化為主角實際移動

	public void HeroMoveToTargetAndReset (Vector3 pos) {
		if (Vector3.Distance (this.transform.position, pos) < 0.01f) {
			//1.呼叫"讓Cube跟著detectFloorAndHeroFwdPoint[]"的功能，確保Cube有先移動到正確位置
			CubeFollowHasRoadRayPoint_Switcher (heroForwardID);

			//2.把在Update獲得的座標pos灌向此物件的座標
			this.transform.position = pos;

			//3.所有動作相關布林值重設，以保證下次偵測正確無誤
			isMoving = false;
			isMovingCube = false;
			isPull = false;
			isPush = false;
			isThrow = false;
			isJumping = false;

			//4.所有障礙物先假定為空，以保證射線程式經由FixedUpdate啟動射線時，射線偵測能正確無誤
			for (int i = 0; i < isHeroFowardHasItem.Length; i++) {
				isHeroFowardHasItem [i] = false;
			}

			//5.因為myReadyMoveCube已經抵達正確位置，為確保下次能正確偵測，須先重設原myReadyMoveCube的isMoving布林值，待下次置入myReadyMoveCube後重新偵測
			if (myReadyMoveCube) {
				myReadyMoveCube.GetComponent<Ctrl_OnCube> ().ToFloor ();
				myReadyMoveCube.GetComponent<Ctrl_OnCube> ().isMoving = false;
			}

			//6.因為myReadyMoveCube已經抵達正確位置，為確保下次能正確偵測，須先清空myReadyMoveCube欄位，待下次置入
			myReadyMoveCube = null;

			//7.因為推拉物件會減速，為了走路速度正常，須重置
			moveSpeed_Recent = moveSpeed_Basic;
		} else {
			//1.將pos的位置正式灌給此物件
			this.transform.position = Vector3.MoveTowards (this.transform.position, pos, Time.deltaTime * moveSpeed_Recent);

			//2.將此物件正確轉向
			CubeFollowHasRoadRayPoint_Switcher (heroForwardID);
		}
	}

	#endregion

	#region ◎ 指定主角實際轉動面向

	public void HeroRotation () {	
		switch (heroForwardID) {
		case 'W':
			faynaMod.transform.rotation = detectFloorAndHeroFwdPoint [0].transform.rotation;
			tog.transform.position = new Vector3((int)heroWASDVector3 [0].x,1,(int)heroWASDVector3 [0].z);
			break;
		case 'S':
			faynaMod.transform.rotation = detectFloorAndHeroFwdPoint [1].transform.rotation;
			tog.transform.position = new Vector3 ((int)heroWASDVector3 [1].x, 1, (int)heroWASDVector3 [1].z);
			break;
		case 'A':
			faynaMod.transform.rotation = detectFloorAndHeroFwdPoint [2].transform.rotation;
			tog.transform.position = new Vector3((int)heroWASDVector3 [2].x,1,(int)heroWASDVector3 [2].z);
			break;
		case 'D':
			faynaMod.transform.rotation = detectFloorAndHeroFwdPoint [3].transform.rotation;
			tog.transform.position = new Vector3((int)heroWASDVector3 [3].x,1,(int)heroWASDVector3 [3].z);
			break;
		default:
			break;
		}
	}

	#endregion

	public void CubeFollowHasRoadRayPoint_Switcher (char heroFWD_Temp) {
		if (isMovingCube) {
			int heroFWDInt_Temp;
			switch (heroFWD_Temp) {
			case 'W':
				heroFWDInt_Temp = 0;
				CubeFollowHasRoadRayPoint_Mover (heroFWDInt_Temp);
				break;
			case 'S':
				heroFWDInt_Temp = 1;
				CubeFollowHasRoadRayPoint_Mover (heroFWDInt_Temp);
				break;
			case 'A':
				heroFWDInt_Temp = 2;
				CubeFollowHasRoadRayPoint_Mover (heroFWDInt_Temp);
				break;
			case 'D':
				heroFWDInt_Temp = 3;
				CubeFollowHasRoadRayPoint_Mover (heroFWDInt_Temp);
				break;
			}
		}
	}

	public void CubeFollowHasRoadRayPoint_Mover (int heroFWDInt_Temp) {
		if (isHeroFowardHasItem [heroFWDInt_Temp]) {
			if (myReadyMoveCube) {
				myReadyMoveCube.GetComponent<Ctrl_OnCube> ().isMoving = true;
				Vector3 pos_Temp = detectFloorAndHeroFwdPoint [heroFWDInt_Temp].transform.position;
				pos_Temp.y = detectFloorAndHeroFwdPoint [heroFWDInt_Temp].GetComponent<Ctrl_LaserShooter> ().canMoveRay_Hit.transform.position.y;
				myReadyMoveCube.transform.position = pos_Temp;
			}
		}
	}

	public void WalkJumpEvent (int heroFWDID_Temp, int heroLastID_Temp) {
		if (canHeroGoFoward [heroFWDID_Temp]) {
			#region ● 跳躍+移動
			if (Input.GetKeyDown (KeyCode.Space)) {
				heroLastPos = this.transform.position;
				isMoving = true;
				isJumping = true;
				Vector3 pos_Temp = heroWASDVector3 [heroFWDID_Temp];
				//pos_Temp.y += 1;
				heroMoveWayTarget = pos_Temp;
			}
			#endregion
			#region ● 不跳躍，單純移動(在IEnumerator里判別)
			if (!isHeroFowardHasItem [heroFWDID_Temp]) {
				heroLastPos = this.transform.position;
				isMoving = true;
				Vector3 pos_Temp = heroWASDVector3 [heroFWDID_Temp];
				//pos_Temp.y += 1;
				heroMoveWayTarget = pos_Temp;
			}
			#endregion
		}
	}

	//--------------------------------------------↓面向轉成int輸入-----------------------------------------
	public void PushEvent (int heroFWDID_Temp) {
		if (canHeroGoFoward [heroFWDID_Temp] && isHeroFowardHasItem [heroFWDID_Temp] && canHeroPushCube [heroFWDID_Temp]) {
			isMovingCube = true;
			isMoving = true;
			isPush = true;
			heroMoveWayTarget = heroWASDVector3 [heroFWDID_Temp] - new Vector3 (0, 1, 0);
		} else if (canHeroGoFoward [heroFWDID_Temp] && !isHeroFowardHasItem [heroFWDID_Temp] && myReadyMoveCube) {
			isMovingCube = true;
			isMoving = true;
			Vector3 pos_Temp = heroWASDVector3 [heroFWDID_Temp];
			//pos_Temp.y += 1;
			heroMoveWayTarget = pos_Temp;
		}
	}
	//------------------------------------------↓面向轉成int輸入-----------------------------------------
	public void PullEvent (int heroFWDID_Temp) {
		if (canHeroGoFoward [heroFWDID_Temp] && !isHeroFowardHasItem [heroFWDID_Temp] && canHeroPushCube [heroFWDID_Temp]) {
			isMovingCube = true;
			isMoving = true;
			isPull = true;
			Vector3 pos_Temp = heroWASDVector3 [heroFWDID_Temp];
			//pos_Temp.y += 1;
			heroMoveWayTarget = pos_Temp;
		} else if (canHeroGoFoward [heroFWDID_Temp] && !isHeroFowardHasItem [heroFWDID_Temp] && myReadyMoveCube) {
			isMovingCube = true;
			isMoving = true;
			Vector3 pos_Temp = heroWASDVector3 [heroFWDID_Temp];
			//pos_Temp.y += 1;
			heroMoveWayTarget = pos_Temp;
		}
	}
	//------------------------------------------↓面向轉成int輸入-----------------------------------------
	public void ThrowEvent (int needRayMod_Temp, int needRayNum_Temp) {
		if (myReadyMoveCube) {
			if (myReadyMoveCube.GetComponent<Ctrl_OnCube> ().cubeModeID == 3) {
				isThrow = true;
				isMoving = true;
				Vector3 pos_Temp = myReadyMoveCube.transform.position;
				if (needRayMod_Temp == 0) {
					pos_Temp = detectBehindCubeHasItem [needRayNum_Temp].GetComponent<Ctrl_LaserShooter> ().canParkRay_Hit.transform.position;
				} else if (needRayMod_Temp == 1) {
					if (canHeroThrowCube [needRayNum_Temp]) {
						pos_Temp = detect4CorHasItem [needRayNum_Temp].GetComponent<Ctrl_LaserShooter> ().canThrowRay_Hit.transform.position;
					}
				}
				pos_Temp.y += 1;
				myReadyMoveCube.transform.position = pos_Temp;
			}
		}
	}
}
