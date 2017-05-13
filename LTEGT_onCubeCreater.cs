using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LTEGT_onCubeCreater : MonoBehaviour {
    [Header("0floor 1plane")]
    public int myMod;//0floor 1plane
    public GameObject myCube;
    public GameObject myCubeManager;
    
    [Header("設定X起點")]
    public int myStartX;
    [Header("設定Z起點")]
    public int myStartZ;
    [Header("設定高度")]
    public float myY;
    [Header("設定長")]
    public int myLength;
    [Header("設定寬")]
    public int myWidth;
    // Use this for initialization
    void Start () {
        for (int a = myStartX; a < myWidth; a++) {
            for (int b = myStartZ; b < myLength; b++)
            {
                GameObject Cube = Instantiate(myCube, new Vector3(a, myY, b), transform.rotation)as GameObject;
                //Cube.name = a.ToString()+","+b.ToString();
                if (myMod == 1)
                {
                    Cube.name = "Plane";
                }
                else {
                    Cube.name = "floor";
                }
                
                Cube.transform.parent = myCubeManager.transform;
            }
        }
	}
}
