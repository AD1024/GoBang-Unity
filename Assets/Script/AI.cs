using System;
struct SetPoint{
	public int pX,pY;
}
class AI{
	enum Action{
		Attack,
		Defend,
		Pending
	}
	int[,] scoreAI = new int[15,15];
	int[,] scorePlayer = new int[15, 15];
	int[,] score = new int[15, 15];
	int[] playerWeight;
	int[] AIWeight;
	int[] dx;
	int[] dy;
	ChessType[,] chessBoard = new ChessType[15, 15];

	public AI(){
		playerWeight = new int[]{ 5, 40, 600, 15000, 400000 };
		AIWeight = new int[]{5, 20, 300, 2000, 100000 };
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
				scoreAI [i, j] = 0;
				scorePlayer [i, j] = 0;
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

	public SetPoint getPos(){
		SetPoint finalPos = new SetPoint ();
		resetScore ();
		int ai, player;
		int cai, cplayer;
		for (int x = 0; x < 15; ++x) {
			for (int y = 0; y < 15; ++y) {
				for (int k = 0; k < 4; ++k) {
					if (validPos (x + dx [k] * 4, y + dy [k] * 4)) {
						ai = player = 0;
						cai = cplayer = 0;
						checkCount (x, y, k, ChessType.Black, out cplayer);
						checkCount (x, y, k, ChessType.White, out cai);
						if (cplayer != 0 && cai != 0)
							continue;
						else if (cplayer == 0) {
							for (int i = 0; i <= 4; ++i) {
								if (chessBoard [x + dx [k] * i, y + dy [k] * i] == ChessType.None) {
									scoreAI [x + dx [k] * i, y + dy [k] * i] += AIWeight [cai];
									score [x + dx [k] * i, y + dy [k] * i] += AIWeight [cai];
								}
							}
						} else if (cai == 0) {
							for (int i = 0; i <= 4; ++i) {
								if (chessBoard [x + dx [k] * i, y + dy [k] * i] == ChessType.None) {
									scorePlayer [x + dx [k] * i, y + dy [k] * i] += playerWeight [cplayer];
									score [x + dx [k] * i, y + dy [k] * i] += playerWeight [cplayer];
								}
							}
						} else if (cplayer == 0 && cai == 0) {
							for (int i = 0; i <= 4; ++i) {
								if (chessBoard [x + dx [k] * i, y + dy [k] * i] == ChessType.None) {
									scoreAI [x + dx [k] * i, y + dy [k] * i] += 3;
									scorePlayer [x + dx [k] * i, y + dy [k] * i] += 3;
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
		int AIsecondMax = -1000;
		int PlayersecondMax = -1000;
		int cs = -1000;
		SetPoint P1 = new SetPoint ();
		SetPoint P2 = new SetPoint ();
		SetPoint g_Point = new SetPoint ();
		Random rand = new Random ();
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
				if (chessBoard [i, j] == ChessType.None && scoreAI [i, j] > tmp) {
					tmp = scoreAI [i, j];
					P1.pX = i;
					P1.pY = j;
				} else if (chessBoard [i, j] == ChessType.None && scoreAI [i, j] == tmp) {
					int cg = rand.Next ()%10;
					if (cg >= 5) {
						P1.pX = i;
						P1.pY = j;
					}
				}
				if (chessBoard [i, j] == ChessType.None && scorePlayer [i, j] > tmp1) {
					P2.pX = i;
					P2.pY = j;
					tmp1 = scorePlayer [i, j];
				}
			}
		}
		finalPos = tmp1>tmp?P2:P1;
		int rnd = rand.Next () % 10;
		if (rnd >= 5) {
			finalPos = g_Point;
		}

		return g_Point;
	}
}

