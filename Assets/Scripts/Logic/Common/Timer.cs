﻿using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 游戏计时器 间隔触发
/// </summary>
public class Timer{

    // 倒计时状态
    public enum TimerState
    {
        None,   // 无
        Run,    // 运行
        Stop,   // 停止
        Invalid,// 无效
    }

    // CS注册回调
    public delegate bool FinishCall(float delta);
    public FinishCall timerEvenFunc;

    // 倒计时信息
    public class TimerEvent
    {
        public void Clear()
        {
            timerState = TimerState.None;
            finishCall = null;
            intervalTime = 0;
            uniqueId = 0;
            cumulativeTime = 0;
        }

        public TimerState timerState;
        public FinishCall finishCall;
        public float intervalTime;
        public int uniqueId;
        public float cumulativeTime;
    }

    public static Timer Instance;
    // 每个倒计时都有唯一Id
    private int _timerUniqueId = 0;
    // 当前保存的所有倒计时信息
    private Dictionary<int, TimerEvent> _timerEventDic = new Dictionary<int, TimerEvent>();
    // 缓存的倒计时信息（优化）
    private List<TimerEvent> _freeTimerEventList = new List<TimerEvent>();

    // 回收一个倒计时信息
    private void RecoveryTimerEvent(int uniqueId)
    {
        TimerEvent timerEvent = null;
        if (_timerEventDic.TryGetValue(uniqueId,out timerEvent))
        {
            return;
        }
        timerEvent.Clear();
        _freeTimerEventList.Add(timerEvent);
    }

    // 获取一个倒计时信息
    private TimerEvent GetTempTimerEvent()
    {
        TimerEvent tempTimerEvent = null;
        if (_freeTimerEventList.Count > 0)
        {
            tempTimerEvent = _freeTimerEventList[0];
            _freeTimerEventList.RemoveAt(0);
        }
        else
        {
            tempTimerEvent = new TimerEvent();
        }

        return tempTimerEvent;
    }

    public Timer()
    {
        Instance = this;
    }

    public void FixedUpdate()
    {
        CheckInvalid();
        CheckValid(Time.deltaTime);
    }

    // 检测倒计时是否已经失效
    void CheckInvalid()
    {
        var uniqueIdList = _timerEventDic.Keys;
        foreach(int uniqueId in uniqueIdList)
        {
            if (_timerEventDic[uniqueId].timerState == TimerState.Invalid)
            {
                RecoveryTimerEvent(uniqueId);
            }
        }
    }

    // 检查有效的的倒计时信息
    void CheckValid(float delta)
    {
        foreach(TimerEvent timerEvent in _timerEventDic.Values)
        {
            if (timerEvent.timerState == TimerState.Run)
            {
                timerEvent.cumulativeTime += delta;
                if (timerEvent.cumulativeTime >= timerEvent.intervalTime)
                {
                    // 间隔时间到
                    timerEvent.cumulativeTime = 0;
                    bool isInvalid = false;
                    if (timerEvent.finishCall != null)
                    {
                        isInvalid = timerEvent.finishCall(delta);
                    }
                    if (isInvalid == true)
                    {
                        EnterState(timerEvent.uniqueId,TimerState.Invalid);
                    }
                }
            }
        }
    }

    // 进入状态
    private void EnterState(int uniqueId,TimerState state)
    {
        TimerEvent timerEvent = null;
        if (_timerEventDic.TryGetValue(uniqueId, out timerEvent))
        {
            return;
        }
        timerEvent.timerState = state;
    }

    // 增加倒计时
    public int AddTimer(float intervalTime, FinishCall finishCall)
    {
        TimerEvent timerEvent = GetTempTimerEvent();
        timerEvent.intervalTime = intervalTime;
        timerEvent.finishCall = finishCall;
        timerEvent.uniqueId = ++_timerUniqueId;
        timerEvent.timerState = TimerState.Run;
        _timerEventDic.Add(_timerUniqueId, timerEvent);
        return _timerUniqueId;
    }

    // 删除倒计时
    public void RemoveTimer(int uniqueId)
    {
        EnterState(uniqueId, TimerState.Invalid);
    }

    // 停止倒计时
    public void PauseTimer(int uniqueId)
    {
        EnterState(uniqueId, TimerState.Stop);
    }

    // 继续倒计时
    public void ResumeTimer(int uniqueId)
    {
        EnterState(uniqueId, TimerState.Run);
    }
}
