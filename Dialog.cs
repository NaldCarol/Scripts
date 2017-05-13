using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour {

	public void ManDia(){
		GM_UIManager.Instance.ShowPanel ("ManDiaPanel");
	}
	public void BoyDia(){
		GM_UIManager.Instance.ShowPanel ("BoyDiaPanel");
	}

	public void GirlDia(){
		GM_UIManager.Instance.ShowPanel ("GirlDiaPanel");
	}
	public void OldLadyDia(){
		GM_UIManager.Instance.ShowPanel ("OldLadyDiaPanel");
	}
}
