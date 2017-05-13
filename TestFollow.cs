using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestFollow : MonoBehaviour {
	public GameObject playerFather;
	public GameObject NPCHome;
	public Vector3 followPos;
	public int npcID;
	public Vector3 npcLastPos;
	[Header ("模型動畫")]
	public Animator npcAni;
	[Header ("模型本尊")]
	public GameObject npcMod;

	[HideInInspector]
	public float disNPCToPlayer;
	//計算Player至NPC距離，用以判別是否該啟動對話提示
	[Header ("供talkToNPC抓取DiaPan用，須先定義代碼")]
	public char needDiaPan;
	[Header ("供Dia_DiaSwitcher抓取DiaNumID用，須先定義代碼")]
	public int needDiaNum;

	public bool isTalking = false;
	public bool isFollow = false;

	public GameObject diaPanel;
	public Canvas warnCan;
	// Use this for initialization
	void Start () {
		playerFather = GameObject.Find ("myPlayer");
		NPCHome = GameObject.Find ("NPC_Home");
		npcAni = npcMod.GetComponent<Animator> ();
		warnCan.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () { 
		disNPCToPlayer = Vector3.Distance (playerFather.transform.position, this.transform.position);
		if (disNPCToPlayer <= 3f) {
			#region 看著主角
			Vector3 relativePos = new Vector3 (playerFather.transform.position.x, this.transform.position.y, playerFather.transform.position.z) - transform.position;
			Quaternion rotation = Quaternion.LookRotation (relativePos);
			transform.rotation = rotation;
			#endregion
			if (this.transform.parent != NPCHome.transform) {
				warnCan.gameObject.SetActive (true);
			} else {
				warnCan.gameObject.SetActive (false);
			}
		} else {
			warnCan.gameObject.SetActive (false);
		}
		if (isTalking) {
			DiaDatabase (needDiaPan);
		} else {
			if (isFollow) {
				StartCoroutine (LastStep ());
			}
		}

		if (this.transform.parent == NPCHome.transform) {
			if (Vector3.Distance (this.transform.position, followPos) < 0.01f) {
				npcAni.SetBool ("isMoving", false);
			} else {
				npcAni.SetBool ("isMoving", true);
			}
		}
	}


	public void talkToNPCSwitch () {
		if (warnCan.isActiveAndEnabled) {
			#region 防止已加入列隊者ID增加，造成ID錯誤
			if (this.transform.parent != NPCHome.transform) {
				isTalking = true;
				NPCHome.GetComponent<NPC_NPCHome> ().num_NPC += 1;
				npcID = NPCHome.GetComponent<NPC_NPCHome> ().num_NPC;
				this.transform.name = "NPC_" + npcID;
				this.transform.parent = NPCHome.transform;
			} else {
				isTalking = false;
			}
			#endregion
		}
	}



	//G=小女孩
	//B=小男孩
	//W=女人
	//M=男人
	//L=婆婆
	public void DiaDatabase (char panelChar) {
		switch (panelChar) {
		case 'G':
			GM_UIManager.Instance.ShowPanel ("GirlDiaPanel");
			diaPanel = GameObject.Find ("GirlDiaPanel");
			break;
		case 'B':
			GM_UIManager.Instance.ShowPanel ("BoyDiaPanel");
			diaPanel = GameObject.Find ("BoyDiaPanel");
			break;
		case 'W':
			GM_UIManager.Instance.ShowPanel ("WomanDiaPanel");
			diaPanel = GameObject.Find ("WomanDiaPanel");
			break;
		case 'M':
			GM_UIManager.Instance.ShowPanel ("ManDiaPanel");
			diaPanel = GameObject.Find ("ManDiaPanel");
			break;
		case 'L':			
			GM_UIManager.Instance.ShowPanel ("OldLadyDiaPanel");
			diaPanel = GameObject.Find ("OldLadyDiaPanel");
			break;
		default:
			break;
		}
		diaPanel.GetComponent<Dia_DiaSwitcher> ().Dia (needDiaNum);
		FollowPlayer ();

	}

	public IEnumerator LastStep () {
		if (Vector3.Distance (this.transform.position, npcLastPos) > 1.4f) {
			npcLastPos = new Vector3 ((int)(this.transform.position.x), (int)(this.transform.position.y), (int)(this.transform.position.z));
		}
		yield return new WaitForSeconds (1);
		FollowPlayer ();

	}

	public void FollowPlayer () {
		isTalking = false;
		isFollow = true;
		//Debug.Log ("關閉對話UI，進入跟隨階段");
		if (npcID == 1) {
			followPos = playerFather.GetComponent<Ctrl_OnPlayer> ().heroLastPos;
			if (Vector3.Distance (this.transform.position, playerFather.transform.position) > 1f) {
				this.transform.position = Vector3.MoveTowards (this.transform.position, followPos, Time.deltaTime);
			} 
		} else {
			GameObject senpai;
			senpai = GameObject.Find ("NPC_" + (npcID - 1));
			followPos = senpai.GetComponent<TestFollow> ().npcLastPos;
			if (Vector3.Distance (this.transform.position, senpai.transform.position) > 1f) {
				this.transform.position = Vector3.MoveTowards (this.transform.position, followPos, Time.deltaTime);
			}
		}

	}
}
