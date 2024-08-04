using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class FrameAnimation : MonoBehaviour
{
    public enum FrameAnimationType
    {
        Auto,
        Idle,
        Move,
        Attack,
        BeAttack,
    }
    [Serializable]
    public class FrameAnimationItem
    {
        public FrameAnimationType it;
        [SerializeField]public List<Sprite> images = new List<Sprite>();
    }

    public float Speed;
    public bool IsLoop;
    [SerializeField] public FrameAnimationType curAnimationState;
    [SerializeField]public List<FrameAnimationItem> items = new List<FrameAnimationItem>();
    private FrameAnimationItem _item;
    private int _frameIndex;
    private float _speed;
    private float _passTime;
    private Image _image;

    

    private Action _playFinishCall;

    private void OnEnable()
    {
        _image = transform.Find("Image").GetComponent<Image>();
        if(FrameAnimationType.Auto == curAnimationState)
        {
            Play(FrameAnimationType.Auto, null, IsLoop, Speed);
        }
    }

    public void Play(FrameAnimationType it ,Action playFinishCall = null, bool isLoop = false, float speed = 1f)
    {
        foreach(var item in items)
        {
            if(item.it == it)
            {
                _item = item;
                break;
            }
        }
        if (_item == null) return;
        curAnimationState = it;
        _speed = speed / _item.images.Count;
        _frameIndex = -1;
        _passTime = 0;
        IsLoop = isLoop;
        _playFinishCall = playFinishCall;
    }

    private void Update()
    {
        if(_item == null) return;
        if (_item.images.Count == 0) return;

        _passTime += Time.deltaTime;
        int newFrameIndex = (int)(_passTime / _speed);

        if (newFrameIndex > _item.images.Count - 1)
        {
            newFrameIndex = 0;

            if (IsLoop) _passTime = 0;
            else
            {
                if (_playFinishCall != null) _playFinishCall();
                return;
            }
        }

        if (_frameIndex != newFrameIndex)
        {
            _frameIndex = newFrameIndex;
            _image.sprite = _item.images[_frameIndex];
        }
    }
}