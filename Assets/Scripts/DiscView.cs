using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class DiscView 
{
    SpriteRenderer _renderer;
    Transform _transform;

    public DiscView(SpriteRenderer renderer, Transform tf)
    {
        _renderer = renderer;
        _transform = tf;
    }

    public void UpdateImage(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }
}