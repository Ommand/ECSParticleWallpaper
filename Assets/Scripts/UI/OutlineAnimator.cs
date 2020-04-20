using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class OutlineAnimator : MonoBehaviour
{
    [SerializeField] private float _time;
    [SerializeField] private int _count;
    [SerializeField] private bool _playOnStart;

    private void Start()
    {
        if (_playOnStart)
            Animate();
    }

    public void Animate()
    {
        var outline = GetComponent<Outline>();
        outline.enabled = true;

        outline.DOFade(0, _time)
            .SetEase(Ease.Linear)
            .SetLoops(_count, LoopType.Yoyo)
            .OnComplete(() =>
            {
                outline.enabled = false;
            });
    }
}
