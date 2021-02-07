using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBird : Bird
{
    [SerializeField]
    public float _explodeX = 2;
    public float _explodeY = 2;

    void OnCollisionEnter2D(Collision2D col)
    {
        _state = BirdState.HitSomething;

        transform.localScale = new Vector3(_explodeX, _explodeY, 0);

        _flagDestroy = true;
        StartCoroutine(DestroyAfter((float)0.3));
    }
}
