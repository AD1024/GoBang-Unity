

class AI{
	const int MaxFiveChainCount = 572;

	bool[,,] _ptable = new bool[Board.CrossCount, Board.CrossCount, MaxFiveChainCount];

	// computer probabilities
	bool[, ,] _ctable = new bool[Board.CrossCount, Board.CrossCount, MaxFiveChainCount];

	// All winning strategies
	int[,] _win = new int[2, MaxFiveChainCount];

	// Grade recorder
	int[,] _cgrades = new int[Board.CrossCount, Board.CrossCount];
	int[,] _pgrades = new int[Board.CrossCount, Board.CrossCount];

	// Board Recorder
	int[,] _board = new int[Board.CrossCount, Board.CrossCount];

	int _cgrade, _pgrade;
	int _icount, _m, _n;
	int _mat, _nat, _mde, _nde;

	public AI(){
		for ( int i = 0;i<Board.CrossCount;i++){
			for ( int j = 0;j<Board.CrossCount;j++){
				_pgrades[i, j] = 0;
				_cgrades[i, j] = 0;
				_board[i, j] = 0;
			}
		}
		for ( int i = 0;i<Board.CrossCount;i++){
			for ( int j = 0;j<Board.CrossCount - 4 ;j++){
				for( int k = 0;k < BoardModel.WinChessCount;k++){
					_ptable[j + k, i, _icount] = true;
					_ctable[j + k, i, _icount] = true;
				}

				_icount++;
			}
		}

		//横  
		for (int i = 0; i < Board.CrossCount; i++){
			for (int j = 0; j < Board.CrossCount - 4; j++){
				for (int k = 0; k < BoardModel.WinChessCount; k++){
					_ptable[i, j + k,  _icount] = true;
					_ctable[i, j + k,  _icount] = true;
				}

				_icount++;
			}
		}

		// 右斜
		for (int i = 0; i < Board.CrossCount - 4; i++){
			for (int j = 0; j < Board.CrossCount - 4; j++){
				for (int k = 0; k < BoardModel.WinChessCount; k++){
					_ptable[j+k, i + k, _icount] = true;
					_ctable[j + k, i + k, _icount] = true;
				}

				_icount++;
			}
		}

		// 左斜
		for (int i = 0; i < Board.CrossCount - 4; i++){
			for (int j = Board.CrossCount - 1; j >= 4; j--){
				for (int k = 0; k < BoardModel.WinChessCount; k++){
					_ptable[j - k, i + k, _icount] = true;
					_ctable[j - k, i + k, _icount] = true;
				}

				_icount++;
			}
		}

		for (int i = 0; i < 2; i++){
			for (int j = 0; j < MaxFiveChainCount; j++){
				_win[i, j] = 0;
			}
		}

		_icount = 0;    
	}




	void CalcScore(){
		_cgrade = 0;
		_pgrade = 0;
		_board[_m, _n] = 1;

		for (int i = 0; i < MaxFiveChainCount; i++){
			if (_ctable[_m, _n, i] && _win[0, i] != -1){
				_win[0, i]++;
			}

			if (_ptable[_m, _n, i]){
				_ptable[_m, _n, i] = false;
				_win[1, i] = -1;
			}
		}
	}

	void CalcCore(){
		for (int i = 0; i < Board.CrossCount; i++){
			for (int j = 0; j < Board.CrossCount; j++){
				_pgrades[i, j] = 0;
				if (_board[i, j] == 0){
					for (int k = 0; k < MaxFiveChainCount; k++){
						if (_ptable[i, j, k]){
							switch (_win[1, k])
							{
							case 1:
								_pgrades[i, j] += 5;
								break;
							case 2:
								_pgrades[i, j] += 70;
								break;
							case 3:
								_pgrades[i, j] += 500;
								break;
							case 4:
								_pgrades[i, j] += 20000;
								break;
							}
						}
					}

					_cgrades[i, j] = 0;
					if (_board[i, j] == 0){  
						for (int k = 0; k < MaxFiveChainCount; k++){
							if (_ctable[i, j, k]){
								switch (_win[0, k]){
								case 1:
									_cgrades[i, j] += 5;
									break;
								case 2:
									_cgrades[i, j] += 52;
									break;
								case 3:
									_cgrades[i, j] += 200;
									break;
								case 4:
									_cgrades[i, j] += 10000;
									break;
								}
							}
						}

					}


				}
			}
		}

	}
	public void ComputerDo(int playerX, int playerY, out int finalX, out int finalY ){
		setPlayerPiece(playerX, playerY);

		CalcCore();

		for (int i = 0; i < Board.CrossCount; i++){
			for (int j = 0; j < Board.CrossCount; j++){
				if (_board[i, j] == 0){
					if (_cgrades[i, j] >= _cgrade){
						_cgrade = _cgrades[i, j];
						_mat = i;
						_nat = j;
					}

					if (_pgrades[i, j] >= _pgrade){
						_pgrade = _pgrades[i, j];
						_mde = i;
						_nde = j;
					}

				}
			}
		}
		if (_cgrade >= _pgrade){
			_m = _mat;
			_n = _nat;
		}
		else{
			_m = _mde;
			_n = _nde;
		}
			
		CalcScore();

		finalX = _m;
		finalY = _n;
	}

	void setPlayerPiece( int playerX, int playerY ){
		int m = playerX;
		int n = playerY;

		if ( _board[m, n ] == 0 ){
			_board[m, n] = 2;

			for( int i = 0;i<MaxFiveChainCount;i++){
				if ( _ptable[m, n, i ] && _win[1, i] != -1 ){
					_win[1, i]++;
				}
				if (_ctable[m,n,i]){
					_ctable[m, n, i] = false;
					_win[0, i] = -1;
				}
			}
		}
	}

}

