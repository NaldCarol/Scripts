using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FaceCam : MonoBehaviour {
	public Transform target;
	public  Transform fatherOBJ;
	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Main Camera").transform;
		fatherOBJ = gameObject.transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.forward = new Vector3(transform.position.x-target.position.x , 0, transform.position.z-target.position.z);
	}

	public void ClickTalk(){
		fatherOBJ.GetComponent<TestFollow> ().talkToNPCSwitch ();
	}
}
