using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dia_DiaSwitcher : MonoBehaviour {
	public Text txt;
	void Start () {
	}
		// Update is called once per frame
	void Update () {

	}

	public void Dia(int diaID_Temp){
		switch (diaID_Temp) {
		case 1:
			txt.text="泛亞哥哥，大石頭不再發光了！\n為什麼呢？";//G
			break;
		case 2:
			txt.text=("唉，靈石已經無法眷顧我們，食物愈來愈少......\n這下可怎麼辦？");//L
			break;
		case 3:
			txt.text=("我在這邊等了好久......我和你一起去找他好了。");//W
			break;
		case 4:
			txt.text=("剛剛顧著追趕獵物，不小心被同伴拋下了。");//M
			break;
		case 5:			
			txt.text= ("哼，那個膽小鬼！\n明明說好要幫我採花的！");//G
			break;
		case 6:
			txt.text=("這裡好危險啊，我想去找她，可是我過不去......你能幫幫我嗎？");//B
			break;
		case 7:
			txt.text= ("唉呀，剛剛和朋友聊著聊著就走到這邊了！\n我能和你一起回去嗎？");//W
			break;
		case 8:
			txt.text= ("這是我族留下的遺跡。\n原本想找有關於靈石的線索，沒想到卻被困在這兒。");//L
			break;
		case 9:
			txt.text= ("這些石頭好好玩哦！它會發光呢！");//B
			break;
		case 10:
			txt.text=("泛亞哥哥，你說要帶我去新地方玩，是不是在門的另一端？");//G
			break;
		case 11:
			txt.text=("謝謝你，願意帶我這把老骨頭，一起去新住處。");//L
			break;
		case 12:
			txt.text=("這裡沙子太軟，腳都陷下去了......總覺得海浪隨時會打上來。");//M
			break;
		default:
			break;
		}

	}
}
