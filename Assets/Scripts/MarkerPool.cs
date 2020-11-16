using UnityEngine;
using System.Collections;
using UniRx.Toolkit;

public class MarkerPool : ObjectPool<SpriteRenderer>
{
    SpriteRenderer _prefab;
    public MarkerPool(SpriteRenderer prefab)
    {
        _prefab = prefab;
    }

    protected override SpriteRenderer CreateInstance()
    {
        var marker = GameObject.Instantiate(_prefab);
        return marker;
    }
}