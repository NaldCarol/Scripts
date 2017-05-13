using UnityEngine;
using System.Collections;

public class GM_Cam : MonoBehaviour {
	public Transform target;//目標
	public float distance = 10f;//和目標的距離
	public float rotateHSpeed = 100f;//水平旋轉速度
	public float rotateVSpeed = 50f;//垂直旋轉速度
	public float rotateVMinLimit = 10f;//垂直最低角度
	public float rotateVMaxLimit = 75f;//垂直高低角度
	public float zoomSpeed=200f;//拉近拉遠速度
	public float zoomMinDistance= 5f;//最近距離
	public float zoomMaxDistance= 30f;//最遠距離
	private float rot_h=0f;
	private float rot_v=0f;

	void Awake(){
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, 0);//把z軸歸零 歐拉角
		rot_v=transform.eulerAngles.x;//取得當前垂直旋轉角度
		rot_h=transform.eulerAngles.y;//取得當前水平旋轉角度
	}

	// Use this for initialization
	void Start () {
		GM_UIManager.Instance.ShowPanel ("CamPanel");

	}
	
	// Update is called once per frame
	void Update () {
	}

	void LateUpdate(){
		if (!target) {
			return;//若沒有指定目標,直接返回
		}

//		rot_h+=Input.GetAxis("CamHori")*	rotateHSpeed * Time.deltaTime;
//		rot_v-=Input.GetAxis("CamVerti")*	rotateHSpeed * Time.deltaTime;
//
			if(Input.GetMouseButton(1)){
			if (GameObject.Find ("CamPanel")) {
				GM_UIManager.Instance.ClosePanel ("CamPanel");
			}

			rot_h += Input.GetAxis ("Mouse X") * rotateHSpeed * Time.deltaTime;//計算水平角度
			rot_v -= Input.GetAxis ("Mouse Y") * rotateVSpeed * Time.deltaTime;//計算垂直角度
			}


		rot_h = rot_h % 360;//限制數值在0~360之間 取餘數
		rot_v = rot_v % 360;
		rot_v = Mathf.Clamp (rot_v,rotateVMinLimit,rotateVMaxLimit);
		rot_h = Mathf.Clamp (rot_h, -40f, 40f);

	
//			float zoom=-Input.GetAxis("CamZoom")*zoomSpeed*Time.deltaTime;//取得滾輪數值
//			distance += zoom;//計算距離
//			distance = Mathf.Clamp (distance,zoomMinDistance,zoomMaxDistance);//限制距離


		if(Input.GetAxis("Mouse ScrollWheel")!=0){//若有滾輪
			float zoom=-Input.GetAxis("Mouse ScrollWheel")*zoomSpeed*Time.deltaTime;//取得滾輪數值
			distance += zoom;//計算距離
			distance = Mathf.Clamp (distance,zoomMinDistance,zoomMaxDistance);//限制距離
		}

		Quaternion rotation = Quaternion.Euler (rot_v,rot_h,0);//轉成四元數 取得角度向量 旋轉角度後再把z轉0
		Vector3 position = rotation * new Vector3 (0,0,-distance) + target.position;//取得位置座標
		transform.rotation = rotation;
		transform.position = position;

	}
}
