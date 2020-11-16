using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Grid
{
    DiscModel _disc;
    Vector2Int _position;
    bool _canPutDisc;

    public Vector2Int Position => _position;

    public DiscModel Disc => _disc;
    public bool CanPutDisc { set { _canPutDisc = value; } get { return _canPutDisc; } }

    public Grid(int x, int y)
    {
        _position = new Vector2Int(x, y);

        _disc = new DiscModel(x, y);
    }

    public void Init()
    {
        _canPutDisc = false;
    }
}