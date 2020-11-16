using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class PlayerModel
{
    [SerializeField]
    DiscModel.Type _type;
    [SerializeField]
    Sprite _image;
    [SerializeField]
    IntReactiveProperty _score;

    public DiscModel.Type Type => _type;
    public Sprite Image => _image;
    public int Score { set { _score.Value = value > 0 ? value : 0; } get { return _score.Value; } }
    public IntReactiveProperty ScoreReactiveProperty => _score;

    public PlayerModel(DiscModel.Type type, Sprite image)
    {
        _type = type;
        _image = image;

        _score.Value = 0;
    }
}
