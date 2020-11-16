using UnityEngine;
using System.Collections;
using UniRx.Toolkit;

public class DiscPool : ObjectPool<DiscPresenter>
{
    DiscPresenter _prefab;

    public DiscPool(DiscPresenter prefab)
    {
        _prefab = prefab;
    }

    protected override DiscPresenter CreateInstance()
    {
        var disc = GameObject.Instantiate(_prefab);

        disc.Construct();
        return disc;
    }
}