using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheSTAR.Utility
{
    public static class TimeUtility
    {
        public static void Wait(float timeSeconds, Action action) => Wait((int)(timeSeconds * 1000), action);

        public async static void Wait(int timeMilliseconds, Action action)
        {
            await Task.Run(() => Task.Delay(timeMilliseconds));
            action?.Invoke();
        }

        public async static void Wait(int timeMillisecondsMin, int timeMillisecondsMax, Action action)
        {
            int randomDelay = Random.Range(timeMillisecondsMin, timeMillisecondsMax);
            await Task.Run(() => Task.Delay(randomDelay));
            action?.Invoke();
        }

        public static TimeCycleControl DoWhile(WaitWhileCondition condition, float timeSeconds, Action action) => DoWhile(condition, (int)(timeSeconds * 1000), action);

        public static TimeCycleControl DoWhile(WaitWhileCondition condition, int timeMilliseconds, Action action)
        {
            action?.Invoke();
            return While(condition, timeMilliseconds, action);
        }

        public static TimeCycleControl While(WaitWhileCondition condition, float timeSecondsMin, float timeSecondsMax, Action action) => While(condition, (int)(timeSecondsMin * 1000), (int)(timeSecondsMax * 1000), action);

        public static TimeCycleControl While(WaitWhileCondition condition, float timeSeconds, Action action) => While(condition, (int)(timeSeconds * 1000), action);

        public static TimeCycleControl While(WaitWhileCondition condition, int timeMillisecondsMin, int timeMillisecondsMax, Action action)
        {
            TimeCycleControl control = new();
            WaitWhile(condition, timeMillisecondsMin, timeMillisecondsMax, action, control);
            return control;
        }

        public static TimeCycleControl While(WaitWhileCondition condition, int timeMilliseconds, Action action)
        {
            TimeCycleControl control = new ();
            WaitWhile(condition, timeMilliseconds, action, control);
            return control;
        }

        private static void WaitWhile(WaitWhileCondition condition, int timeMilliseconds, Action action, TimeCycleControl control)
        {
            if (control.IsBreak) return;

            Wait(timeMilliseconds, () =>
            {
                if (control.IsBreak) return;

                action?.Invoke();

                if (!condition.Invoke()) return;

                WaitWhile(condition, timeMilliseconds, action, control);
            });
        }

        private static void WaitWhile(WaitWhileCondition condition, int timeMillisecondsMin, int timeMillisecondsMax, Action action, TimeCycleControl control)
        {
            if (control.IsBreak) return;

            Wait(timeMillisecondsMin, timeMillisecondsMax, () =>
            {
                if (control.IsBreak) return;

                action?.Invoke();

                if (!condition.Invoke()) return;

                WaitWhile(condition, timeMillisecondsMin, timeMillisecondsMax, action, control);
            });
        }

        private enum CycleStatus
        {
            Alive,
            Breaked
        }

        public delegate bool WaitWhileCondition();
    }

    public class TimeCycleControl
    {
        public bool IsBreak
        {
            get;
            private set;
        }

        public void Stop() => IsBreak = true;
    }
}