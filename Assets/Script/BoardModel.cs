using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public enum ChessType{
	None = 0,
	Black = 1,
	White = 2
}

public class BoardModel{
	private int[] dx = new int[8]{1,0,-1,0,1,1,-1,-1};
	private int[] dy = new int[8]{0,1,0,-1,1,-1,1,-1};
	public const int WinChessCount = 5;

	public const int PlayerColor = 0;
	public const int AIColor = 1;

	public ChessType[,] BoardData = new ChessType[Board.CrossCount,Board.CrossCount];

	public int[,] ChessBoardStatus = new int[15, 15];
	public int[,] ChessBoardColor = new int[15, 15];

	static bool ValidPosition(int x,int y){
		return x >= 0 && x < Board.CrossCount && y >= 0 && y < Board.CrossCount;
	}
	static bool ValidChessType(ChessType type){
		return type == ChessType.None || type == ChessType.Black || type == ChessType.White;
	}

	public ChessType getType(int i,int j){
		if(!ValidPosition(i,j)){
			return ChessType.None;
		}else 
			return BoardData[i,j];
	}
	public BoardModel(){
		for(int i=0;i<15;++i){
			for (int j = 0; j < 15; ++j) {
				ChessBoardStatus [i, j] = 0;
				ChessBoardColor [i, j] = -1;
			}
		}
	}
	// Set Chess
	public bool SetChess(int i,int j,ChessType type){
		if (!ValidPosition (i, j) || !ValidChessType (type)) {
			return false;
		} else {
			BoardData [i, j] = type;
			ChessBoardStatus [i, j] = 1;
			if (type == ChessType.Black) {
				ChessBoardColor [i, j] = PlayerColor;
			} else
				ChessBoardColor [i, j] = AIColor;
		}
		return true;
	}
	// Winner Checker
	/*
	public bool CheckWinner(int px,int py){
		bool f = false;
		ChessType currentType = getType (px, py);
		if (currentType == ChessType.None)
			return false;
		for (int i = 0; i < 8; ++i) {
			f = true;
			for (int j = 0; j < 4; ++j) {
				int nx = px + dx [i];
				int ny = py + dy [i];
				if (ValidPosition (nx, ny)) {
					if (BoardData [nx, ny] != currentType) {
						f = false;
						break;
					}
				} else {
					f = false;
					break;
				}
			}
			if (f) {
				return true;
			}
		}
		return false;
	}*/

	int CheckVerticalLink(int px, int py, ChessType type){
		int linkCount = 1;

		for (int y = py  + 1; y < Board.CrossCount; y++){
			if (getType(px, y) == type){
				linkCount++;

				if (linkCount >= WinChessCount){
					return linkCount;
				}
			}else{
				break;
			}
		}


		for (int y = py - 1; y >= 0; y--){
			if (getType(px, y) == type){
				linkCount++;

				if (linkCount >= WinChessCount){
					return linkCount;
				}
			}else{
				break;
			}
		}

		return linkCount;
	}
		
	int CheckHorizentalLink(int px, int py, ChessType type){
		int linkCount = 1;

		for (int x = px+1; x < Board.CrossCount; x++){
			if (getType(x, py) == type){
				linkCount++;

				if (linkCount >= WinChessCount){
					return linkCount;
				}
			}else{
				break;
			}
		}
			
		for (int x = px-1; x >= 0; x--){
			if (getType(x, py) == type){
				linkCount++;

				if (linkCount >= WinChessCount){
					return linkCount;
				}
			}
			else{
				break;
			}
		}

		return linkCount;
	}
		
	int CheckBiasLink(int px, int py, ChessType type){
		int ret = 0;
		int linkCount = 1;

		for (int x = px - 1, y = py - 1; x>=0 && y >=0 ; x--, y--){
			if (getType(x, y) == type){
				linkCount++;

				if (linkCount >= WinChessCount){
					return linkCount;
				}
			}else{
				break;
			}
		}
			
		for (int x = px + 1, y = py + 1; x < Board.CrossCount && y < Board.CrossCount; x++, y++){
			if (getType(x, y) == type){
				linkCount++;

				if (linkCount >= WinChessCount){
					return linkCount;
				}
			}
			else{
				break;
			}
		}

		ret = linkCount;
		linkCount = 1;
		for (int x = px -1 , y = py+ 1; x >=0 && y < Board.CrossCount; x--, y++){
			if (getType(x, y) == type){
				linkCount++;

				if (linkCount >= WinChessCount){
					return linkCount;
				}
			}else{
				break;
			}
		}
		for (int x = px + 1, y = py - 1; x < Board.CrossCount && y >= 0; x++, y--){
			if (getType(x, y) == type)
			{
				linkCount++;

				if (linkCount >= WinChessCount){
					return linkCount;
				}
			}else{
				break;
			}
		}

		return Mathf.Max(ret, linkCount);
	}
	public int CheckLink(int px, int py, ChessType type ){
		int linkCount = 0;

		linkCount = Mathf.Max(CheckHorizentalLink(px, py, type), linkCount);
		linkCount = Mathf.Max(CheckVerticalLink(px, py, type), linkCount);
		linkCount = Mathf.Max(CheckBiasLink(px, py, type), linkCount);

		return linkCount;
	}
}