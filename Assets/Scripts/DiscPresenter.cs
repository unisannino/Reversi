using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.Assertions;

public class DiscPresenter : MonoBehaviour
{
    DiscModel _model;
    DiscView _view;

    public Vector2 Size => Vector2.one;

    public void UpdateImage(PlayerModel player)
    {
        _view.UpdateImage(player.Image);
    }

    public void Construct()
    {
        var renderer = GetComponent<SpriteRenderer>();
        var tf = transform;

        _view = new DiscView(renderer, tf);
    }

    public void Init(Vector2Int pos, PlayerModel player, DiscModel model)
    {
        _model = model;
        _model.Init(pos, player);
        Bind(model);

        transform.position = new Vector3(pos.x, pos.y, 0);
    }

    void Bind(DiscModel model)
    {
        model.Player
             .Where(player => player?.Type != DiscModel.Type.None || player?.Type != DiscModel.Type.Count)
             .Subscribe(player => _view.UpdateImage(player.Image));
    }
}