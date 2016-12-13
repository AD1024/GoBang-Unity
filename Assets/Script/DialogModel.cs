using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogModel : MonoBehaviour {

	public static bool show = false;

	public GUISkin skin;

	Rect Window = new Rect(200,80,240,100);
	public static string _msg;

	void func(int id){
		if (GUI.Button (new Rect (70, 40, 100, 20), "RESTART")) {
			show = false;
		}
	}

	public void setMsg(string msg){
		_msg = msg;
	}
	public void toggle(){
		show = !show;
	}

	void OnGUI(){
		if(show){
			GUI.skin = skin;
			Window = GUI.Window (0,Window,func,_msg);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
