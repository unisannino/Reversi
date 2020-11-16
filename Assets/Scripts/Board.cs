using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class Board
{
    const int GRID_NUM = 8;

    [SerializeField]
    Grid[,] _grids;

    DiscPool _pool;
    MarkerPool _markerPool;

    Queue<SpriteRenderer> _rentMarkers;

    public Board(DiscPresenter prefab, SpriteRenderer marker)
    {
        _grids = new Grid[GRID_NUM, GRID_NUM];
        for (int j = 0; j < _grids.GetLength(1); j++)
        {
            for (int i = 0; i < _grids.GetLength(0); i++)
            {
                _grids[i, j] = new Grid(i, j);
            }
        }

        _pool = new DiscPool(prefab);
        _markerPool = new MarkerPool(marker);

        _rentMarkers = new Queue<SpriteRenderer>(GRID_NUM * GRID_NUM);
    }
    public void ResetBoard()
    {
    }

    public bool CanPutDisc(Vector2Int pos, Vector2 size)
    {
        if (pos.x < 0 || pos.y < 0)
        {
            return false;
        }

        var disc = Physics2D.OverlapBox(pos, size, 0, LayerMask.NameToLayer(DiscModel.LayerName));

        var flag = disc != null && _grids[pos.x, pos.y].CanPutDisc;
        return flag;
    }
    public int PutDisc(Vector2Int pos, PlayerModel player)
    {
        // 置いた分を含んでおく
        var score = 1;
        var disc = _pool.Rent();
        disc.Init(pos, player, _grids[pos.x, pos.y].Disc);
        score += ReverseDiscs(pos, player);

        return score;
    }

    public int SearchPositionCanPutDisc(PlayerModel player)
    {
        int canPutAmount = 0;
        Vector2Int pos = Vector2Int.zero;

        while (_rentMarkers.Count > 0)
        {
            _markerPool.Return(_rentMarkers.Dequeue());
        }


        for (int j = 0; j < _grids.GetLength(1); j++)
        {
            for (int i = 0; i < _grids.GetLength(0); i++)
            {
                pos.x = i;
                pos.y = j;
                _grids[i, j].Init();
                if (IsExistDisc(pos))
                {
                    continue;
                }
                var result = SearchPositionAroundCanPutDisc(pos, player);
                if (result > 0)
                {
                    canPutAmount++;
                    var marker = _markerPool.Rent();
                    marker.transform.position = new Vector3(pos.x, pos.y, 0);
                    _rentMarkers.Enqueue(marker);
                    _grids[i, j].CanPutDisc = true;
                }
            }
        }

        return canPutAmount;
    }

    int SearchPositionAroundCanPutDisc(Vector2Int pos, PlayerModel player)
    {
        var canPutPosAmount = 0;
        Vector2Int direction = Vector2Int.zero;

        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                direction.x = i;
                direction.y = j;

                if (IsValidGridPosition(pos + direction) && IsExistDisc(pos + direction) && IsEnemyDisc(pos + direction, player))
                {
                    var length = SearchFinishDisc(pos, direction, player);
                    if (length > 0)
                    {
                        canPutPosAmount++;
                    }
                }
            }
        }

        return canPutPosAmount;
    }

    int SearchFinishDisc(Vector2Int currentPos, Vector2Int direction, PlayerModel player, int count = 0)
    {
        var pos = currentPos + direction;
        if (!IsValidGridPosition(pos) || !IsExistDisc(pos))
        {
            return 0;
        }

        if (IsPlayerDisc(pos, player))
        {
            return ++count;
        }

        return SearchFinishDisc(pos, direction, player, ++count);
    }

    int ReverseDiscs(Vector2Int pos, PlayerModel player)
    {
        var score = 0;
        Vector2Int direction = Vector2Int.zero;
        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                direction.x = i;
                direction.y = j;

                if (IsValidGridPosition(pos + direction) && IsExistDisc(pos + direction) && IsEnemyDisc(pos + direction, player))
                {
                    var length = SearchFinishDisc(pos, direction, player);
                    if (length > 0)
                    {
                        for (int k = 1; k < length; k++)
                        {
                            var discPos = pos + direction * k;
                            _grids[discPos.x, discPos.y].Disc.Reverse(player);
                            score++;
                        }
                    }
                }

            }
        }

        return score;
    }

    bool IsValidGridPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < GRID_NUM && pos.y >= 0 && pos.y < GRID_NUM;
    }

    bool IsExistDisc(Vector2Int pos)
    {
        return _grids[pos.x, pos.y].Disc.IsExist;
    }

    bool IsEnemyDisc(Vector2Int pos, PlayerModel player)
    {
        var disc = _grids[pos.x, pos.y].Disc;
        return disc.IsExist && disc.Player.Value.Type != player.Type;
    }

    bool IsPlayerDisc(Vector2Int pos, PlayerModel player)
    {
        var disc = _grids[pos.x, pos.y].Disc;
        return disc.IsExist && disc.Player.Value.Type == player.Type;
    }

}
