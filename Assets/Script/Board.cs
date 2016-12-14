using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Board : MonoBehaviour {

	public GameObject CrossPrefab;
	public const float CrossSize = 40;
	public const int CrossCount = 15;
	public const int Size = 560;
	public const int HalfSize = Size >> 1;

	public Dictionary<int,Cross> crossMap = new Dictionary<int,Cross>();

	public static int makeKey(int x,int y){
		return x * 100000 + y;
	}

	public void Reset(){
		// Recycler
		foreach (Transform child in gameObject.transform) {
			GameObject.Destroy (child.gameObject);
		}
		var mainloop = GetComponent<MainLoop> (); 
		crossMap.Clear ();
		// Initializer
		for (int i = 0; i < CrossCount; ++i) {
			for (int j = 0; j < CrossCount; ++j) {
				var crossObj = GameObject.Instantiate<GameObject> (CrossPrefab);
				crossObj.transform.SetParent (gameObject.transform);
				crossObj.transform.localScale = Vector3.one;
				var pos = crossObj.transform.localPosition;
				pos.x = -HalfSize + i * CrossSize;
				pos.y = -HalfSize + j * CrossSize;
				pos.z = 0;
				crossObj.transform.localPosition = pos;

				var cross = crossObj.GetComponent<Cross> ();
				cross.mainLoop = mainloop;
				cross.posX = i;
				cross.posY = j;
				crossMap.Add (makeKey (i, j), cross);
			}
		}
	}

	public Cross getCross(int x,int y){
		Cross ret;
		if (crossMap.TryGetValue (makeKey (x, y), out ret)) {
			return ret;
		} else
			return null;
	}

	// Use this for initialization
	void Start () {
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}