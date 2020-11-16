using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class DiscModel
{
    public const string LayerName = "Disc";

    [Serializable]
    public enum Type
    {
        None,
        Light,
        Dark,

        Count
    }

    ReactiveProperty<PlayerModel> _player;
    Vector2Int _position;

    public Vector2Int Position => _position;
    public ReactiveProperty<PlayerModel> Player => _player;
    public bool IsExist => _player.Value != null;

    public DiscModel(int x, int y)
    {
        _position = new Vector2Int(x, y);
        _player = new ReactiveProperty<PlayerModel>();
    }

    public void Init(Vector2Int pos, PlayerModel player)
    {
        _position = pos;
        _player.Value = player;
    }

    public void Reverse(PlayerModel player)
    {
        _player.Value = player;
    }
}

