using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_UIRootHandler : MonoBehaviour {
	void Awake () {
		GM_UIManager.Instance.sceneCanvasRoot = gameObject;
	}
}
