using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cross : MonoBehaviour {

	public int posX;
	public int posY;
	public float realX,realY;
	public MainLoop mainLoop;
	// Use this for initialization

	void Start () {
		GetComponent<Button> ().onClick.AddListener (delegate(){
			Debug.Log("Pos:"+posX+" "+posY);
			mainLoop.OnClick(this);
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}