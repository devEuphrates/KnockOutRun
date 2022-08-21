using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Euphrates
{
    public static class GameTimer
    {
        static bool _isRunning = false;
        static readonly List<Timer> _timers = new List<Timer>();

        public static void CreateTimer(string name, float duration, 
            Action onFinish = null, Action<TickInfo> onTick = null, Action onCancle = null, 
            bool useScaledTime = false)
        {
            float start = useScaledTime ? Time.time : Time.realtimeSinceStartup;
            float end = start + duration;

            Timer timer = new Timer(name, start, end, useScaledTime)
            {
                OnTick = onTick,
                OnFinish = onFinish,
                OnCancle = onCancle
            };

            _timers.Add(timer);

            if (!_isRunning)
                RunTimers();
        }

        public static void CancleTimer(string name)
        {
            int indx = -1;
            for (int i = 0; i < _timers.Count; i++)
                if (_timers[i].Name == name)
                    indx = i;

            if (indx == -1)
                return;

            _timers[indx].OnCancle?.Invoke();
            _timers.RemoveAt(indx);
        }

        static async void RunTimers()
        {
            _isRunning = true;
            float last = Time.realtimeSinceStartup;
            float lastScaled = Time.time;

            while (_timers.Count > 0)
            {
                float now = Time.realtimeSinceStartup;
                float nowScaled = Time.time;

                float deltaTime = now - last;
                float deltaScaled = nowScaled - lastScaled;

                for (int i = _timers.Count - 1; i > -1; i--)
                {
                    float usedNow = (_timers[i].TimeScaled ? nowScaled : now);
                    float usedDeltaTime = (_timers[i].TimeScaled ? deltaScaled : deltaTime);

                    if (_timers[i].End < usedNow)
                    {
                        _timers[i].OnFinish?.Invoke();
                        _timers.RemoveAt(i);
                        continue;
                    }

                    if (_timers[i].OnTick == null)
                        continue;

                    var tInfo = new TickInfo(_timers[i].Name, _timers[i].Start, _timers[i].End, usedNow, usedDeltaTime);
                    _timers[i].OnTick.Invoke(tInfo);
                }

                last = Time.realtimeSinceStartup;
                lastScaled = Time.time;
                await Task.Yield();
            }
            _isRunning = false;
        }
    }

    struct Timer
    {
        public readonly string Name;
        public readonly float Start;
        public readonly float End;


        public Action<TickInfo> OnTick;
        public Action OnFinish;
        public Action OnCancle;

        public bool TimeScaled;

        public Timer(string name, float start, float end, bool useScaledTime)
        {
            Name = name;
            Start = start;
            End = end;

            OnTick = null;
            OnFinish = null;
            OnCancle = null;

            TimeScaled = useScaledTime;
        }
    }

    public struct TickInfo
    {
        public readonly string Name;
        public readonly float Start;
        public readonly float End;
        public readonly float Now;
        public readonly float Duration => End - Now;
        public readonly float TimePassed => Now - Start;
        public readonly float TimeLeft => End - Now;
        public readonly float DeltaTime;

        public TickInfo(string name, float start, float end, float now, float deltaTime)
        {
            Name = name;
            Start = start;
            End = end;
            Now = now;
            DeltaTime = deltaTime;
        }
    }
}
