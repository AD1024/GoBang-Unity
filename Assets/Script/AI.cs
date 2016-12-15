using System;
using UnityEngine;

struct SetPoint{
	public int pX,pY;
}
class AI{
	int[,] scoreAI = new int[15,15];
	int[,] scorePlayer = new int[15, 15];
	int[,] score = new int[15, 15];
	int[] playerWeight;
	int[] dieAIScore = new int[15];
	int[] diePlayerScore = new int[15];
	int[] AIWeight;
	int[] dx;
	int[] dy;
	ChessType[,] chessBoard = new ChessType[15, 15];

<<<<<<< HEAD
	public AI(){                       //900
		playerWeight = new int[]{ 5, 40, 800, 8000, 200000 };
		AIWeight = new int[]{5, 20, 500, 7500, 100000 };
=======
	public AI(){
		playerWeight = new int[]{ 5, 40, 900, 5000, 200000 };
		AIWeight = new int[]{5, 20, 500, 4500, 130000 };
>>>>>>> 190e4a35caace403dfba4b152dfd78001425f9e1
		dieAIScore = new int[]{ 0, -7, -100, -400, 0 };
		diePlayerScore = new int[]{ 0, -10, -150, -500, 0 };
		dx = new int[]{1,0,1,1};
		dy = new int[]{0,1,1,-1};
		for (int i = 0; i < 15; ++i) {
			for (int j = 0; j < 15; ++j) {
				chessBoard [i, j] = ChessType.None;
			}
		}
	}

	private void resetScore(){
		for (int i = 0; i < 15; ++i) {
			for (int j = 0; j < 15; ++j) {
				score [i, j] = 0;
			}
		}
	}

	private bool validPos(int px,int py){
		return px >= 0 && px <= 14 && py >= 0 && py <= 14;
	}

	private void checkCount(int px,int py,int dir,ChessType type,out int val){
		ChessType cur;
		int cx, cy;
		val = 0;
		for (int i = 0; i <= 4; ++i) {
			cx = px + dx [dir] * i;
			cy = py + dy [dir] * i;
			cur = chessBoard [cx, cy];
			if (cur == type) {
				++val;
			}
		}
	}

	public void setChess(int px,int py,ChessType type){
		chessBoard [px, py] = type;
	}

	public string int2string(int x){
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

	private void printWeight(){
		string line;
		for (int i = 0; i < 15; ++i) {
			line = "";
			for (int j = 0; j < 15; ++j) {
				line += int2string (score[i,j]);
				line += " ";
			}
			Debug.Log (i + " " + line);
		}
	}

	public SetPoint getPos(){
		SetPoint finalPos = new SetPoint ();
		resetScore ();
		int ai, player;
		int cai, cplayer;
		for (int x = 0; x < 15; ++x) {
			for (int y = 0; y < 15; ++y) {
				for (int k = 0; k < 4; ++k) {
					if (validPos (x + dx [k] * 4, y + dy [k] * 4)) {
						cai = cplayer = 0;
						checkCount (x, y, k, ChessType.Black, out cplayer);
						checkCount (x, y, k, ChessType.White, out cai);
						if (cplayer != 0 && cai != 0) {
							continue;
						}else if (cplayer == 0) {
							for (int i = 0; i <= 4; ++i) {
								if (chessBoard [x + dx [k] * i, y + dy [k] * i] == ChessType.None) {
//									scoreAI [x + dx [k] * i, y + dy [k] * i] += AIWeight [cai];
									score [x + dx [k] * i, y + dy [k] * i] += AIWeight [cai];
								}
							}
						} else if (cai == 0) {
							for (int i = 0; i <= 4; ++i) {
								if (chessBoard [x + dx [k] * i, y + dy [k] * i] == ChessType.None) {
//									scorePlayer [x + dx [k] * i, y + dy [k] * i] += playerWeight [cplayer];
									score [x + dx [k] * i, y + dy [k] * i] += playerWeight [cplayer];
								}
							}
						} else if (cplayer == 0 && cai == 0) {
							for (int i = 0; i <= 4; ++i) {
								if (chessBoard [x + dx [k] * i, y + dy [k] * i] == ChessType.None) {
//									scoreAI [x + dx [k] * i, y + dy [k] * i] += 3;
//									scorePlayer [x + dx [k] * i, y + dy [k] * i] += 3;
									score [x + dx [k] * i, y + dy [k] * i] += 3;
								}
							}
						}
					}
				}
			}
		}
		int tmp = -1000;
		int tmp1 = -1000;
		int cs = -1000;
//		SetPoint P1 = new SetPoint ();
//		SetPoint P2 = new SetPoint ();
		SetPoint g_Point = new SetPoint ();
		System.Random rand = new System.Random ();
		for (int i = 0; i < 15; ++i) {
			for (int j = 0; j < 15; ++j) {
				if (chessBoard [i, j] == ChessType.None && score [i, j] > cs) {
					cs = score [i, j];
					g_Point.pX = i;
					g_Point.pY = j;
				} else if (chessBoard [i, j] == ChessType.None && score [i, j] == cs) {
					int r = rand.Next () % 10;
					if (r >= 5) {
						g_Point.pX = i;
						g_Point.pY = j;
					}
				}
			}
		}
		finalPos = g_Point;
		return finalPos;
	}
}

