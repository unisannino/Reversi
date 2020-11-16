using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class PlayerPresenter : MonoBehaviour
{
    [SerializeField]
    PlayerModel _model;

    PlayerView _view;

    public PlayerModel Model => _model;

    public void Construct()
    {
        var score = GetComponent<TextMeshProUGUI>();

        _view = new PlayerView(score);

        Bind();
    }

    void Bind()
    {
        _model.ScoreReactiveProperty.Subscribe(value => _view.UpdateScore(value));
    }

}
