using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    DiscPresenter _prefab;

    [SerializeField]
    Tilemap _boardView;

    [SerializeField]
    DiscPresenter _currentPlayerDisc;

    [SerializeField]
    PlayerPresenter _light;
    [SerializeField]
    PlayerPresenter _dark;

    [SerializeField]
    SpriteRenderer _markerPrefab;

    Board _board;
    ReactiveProperty<PlayerModel> _currentPlayer;
    DiscModel.Type _currentType;

    bool _isEndGame;

    private void Start()
    {
        _board = new Board(_prefab, _markerPrefab);
        _dark.Construct();
        _light.Construct();
        _currentPlayerDisc.Construct();

        Init();

        _board.SearchPositionCanPutDisc(_currentPlayer.Value);

        _boardView.OnMouseDownAsObservable()
            .Select(_ => CalcMousePosInBoard(Input.mousePosition))
            .Where(pos => !_isEndGame &&_board.CanPutDisc(pos, _prefab.Size))
            .Subscribe(pos => PutDiscAndCheckBoard(pos))
            .AddTo(this);

        _currentPlayer.Subscribe(p => _currentPlayerDisc.UpdateImage(p));
    }

    void Init()
    {
        _board.ResetBoard();
        _currentPlayer = new ReactiveProperty<PlayerModel>(_dark.Model);

        _board.PutDisc(new Vector2Int(3, 3), _dark.Model);
        _board.PutDisc(new Vector2Int(4, 4), _dark.Model);
        _board.PutDisc(new Vector2Int(4, 3), _light.Model);
        _board.PutDisc(new Vector2Int(3, 4), _light.Model);
        _dark.Model.Score = 2;
        _light.Model.Score = 2;
    }

    void SwitchCurrentPlayer()
    {
        if (_currentPlayer.Value.Type == DiscModel.Type.Dark)
        {
            _currentPlayer.Value = _light.Model;
        }
        else
        {
            _currentPlayer.Value = _dark.Model;
        }
    }

    void PutDiscAndCheckBoard(Vector2Int pos)
    {
        var score = _board.PutDisc(pos, _currentPlayer.Value);
        UpdateScore(score);
        var canPutAmount = _board.SearchPositionCanPutDisc(_currentPlayer.Value);
        if (canPutAmount == 0)
        {
            SwitchCurrentPlayer();
            canPutAmount = _board.SearchPositionCanPutDisc(_currentPlayer.Value);
            if (canPutAmount == 0)
            {
                _isEndGame = true;
            }
        }
    }

    void UpdateScore(int score)
    {
        _currentPlayer.Value.Score += score;
        SwitchCurrentPlayer();
        // 置かれたコマの分は取り除いておく
        _currentPlayer.Value.Score -= score - 1;
    }


    Vector2Int CalcMousePosInBoard(Vector3 pos)
    {
        var gridPos = _boardView.WorldToCell(Camera.main.ScreenToWorldPoint(pos));
        return new Vector2Int(gridPos.x, gridPos.y);
    }
}
