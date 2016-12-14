using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainLoop : MonoBehaviour {

	public GameObject WhiteChessPrefab;
	public GameObject BlackChessPrefab;

	public Text ai_win_text;
	public Text player_win_text;
	public Text StatusText;

	public const float CrossSize = 40;
	public const int CrossCount = 15;
	public const int Size = 560;
	public const int HalfSize = Size >> 1;
	public const int MAXIMUM_STEP = CrossCount * CrossCount;
	public int cnt_step = 0;
	int winner = -1;

	int player_win = 0;
	int AI_win = 0;
	DialogModel Message;

	enum Status{
		BlackTurn,
		WhiteTurn,
		Over,
		Pending
	}

	Status _Status;
	Board ChessBoard;
	BoardModel _ChessBoardModel;
	AI Computer;

	Button btnRestart;

	bool ValidPlace(int i,int j){
		return _ChessBoardModel.getType (i, j) == ChessType.None;
	}

	bool SetChessToBoard(Cross place,bool isBlack){
		if (place == null)
			return false;
		var newChess = GameObject.Instantiate<GameObject> (isBlack ? BlackChessPrefab : WhiteChessPrefab);
		var ctype = isBlack ? ChessType.Black : ChessType.White;
		newChess.transform.SetParent (place.gameObject.transform,false);
		var pos = newChess.transform.localPosition;
		pos.x = pos.x-40;
		newChess.transform.localPosition = pos;
		_ChessBoardModel.SetChess (place.posX, place.posY, ctype);
		int win = _ChessBoardModel.CheckLink (place.posX, place.posY,ctype);
		cnt_step += 1;
		Computer.setChess (place.posX, place.posY, isBlack ? ChessType.Black : ChessType.White);
		return win >= 5;
	}

	public void Restart(){
		_Status = Status.BlackTurn;
		_ChessBoardModel = new BoardModel ();
		Computer = new AI ();
		cnt_step = 0;
		winner = -1;
		ChessBoard.Reset ();
	}

	int lastPlayerX,lastPlayerY;

	public void OnClick(Cross cross){
		Debug.Log ("Click");
		if (_Status != Status.BlackTurn)
			return;
		if (ValidPlace (cross.posX, cross.posY)) {
			StatusText.text = "AI的回合";
			lastPlayerX = cross.posX;
			lastPlayerY = cross.posY;

			if (SetChessToBoard (cross, true)) {
				winner = 0;
				_Status = Status.Over;
			} else {
				_Status = Status.WhiteTurn;
			}
		}
	}

	// Use this for initialization
	void Start () {
		ChessBoard = GetComponent<Board> ();
		Message = new DialogModel ();
		ai_win_text = GameObject.Find ("Canvas/AI_WIN_NUMBER").GetComponent<Text> ();
		GameObject.Find ("Canvas/btnRestart").GetComponent<Button> ().onClick.AddListener (() => {
			if(_Status == Status.Pending) Restart();
		});
		StatusText = GameObject.Find ("Canvas/status_info").GetComponent<Text> ();
		player_win_text = GameObject.Find ("Canvas/PLAYER_WIN_NUMBER").GetComponent<Text> ();
		Restart ();
	}

	string int2string(int x){
		int[] num = new int[100];
		int cur = 0;
		while (x>0) {
			num [cur++] = x % 10;
			x /= 10;
		}
		string ret = "";
		for (int i = cur - 1; i >= 0; --i) {
			ret += (char)(num [i] + '0');
		}
		return ret;
	}

	void ShowResult(ChessType type){
		if (type == ChessType.Black) {
			StatusText.text = "你赢啦！我的AI太弱啦！";
			player_win++;
			player_win_text.text = int2string (player_win);
		} else {
			StatusText.text =  "你输啦！连我的AI都打不过\n，您活着还有什么意义呢？";
			AI_win++;
			ai_win_text.text = int2string (AI_win);
		}
		_Status = Status.Pending;
	}
	
	// Update is called once per frame
	void Update () {
		if (_Status == Status.Pending)
			return;
		if (cnt_step == 15 * 15) {
			_Status = Status.Pending;
			StatusText.text =  "平手！你还是有那么一点微小的水平嘛";
		}
		if (winner != -1) {
			ShowResult (winner == 1?ChessType.White : ChessType.Black);
		}
		switch (_Status) {
		case Status.WhiteTurn:{
				int posX, posY;
				SetPoint fin = Computer.getPos();
				if(SetChessToBoard(ChessBoard.getCross(fin.pX,fin.pY),false)){
					_Status = Status.Over;
					winner = 1;
				}else{
					_Status = Status.BlackTurn;
				}
				StatusText.text = "你的回合";
				break;
			}
		case Status.BlackTurn:{
				break;
			}
		}
	}
}