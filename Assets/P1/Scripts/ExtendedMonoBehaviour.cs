using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public class ExtendedMonoBehaviour : MonoBehaviour
{
    protected static CoroutineBuilder Invoke(Action action)
    {
        return new CoroutineBuilder(action);
    }

    protected class CoroutineBuilder
    {
        private readonly Action _action;
        private float _interval;

        public CoroutineBuilder(Action action)
        {
            _action = action;
            _interval = 0;
        }

        public void AfterSeconds(float seconds)
        {
            Task.Run(async delegate
            {
                await Task.Delay(TimeSpan.FromSeconds(seconds));
                _action?.Invoke();
            });
        }

        public void ForTimes(int times)
        {
            Task.Run(async delegate
            {
                for (var i = 0; i < times; i++)
                {
                    await Task.Delay(TimeSpan.FromSeconds(_interval));
                    _action?.Invoke();
                }
            });
        }

        public CoroutineBuilder EverySeconds(float seconds)
        {
            _interval = seconds;
            return this;
        }

        public void While([NotNull] Func<bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            Task.Run(async delegate
            {
                while (predicate.Invoke())
                {
                    await Task.Delay(TimeSpan.FromSeconds(_interval));
                    _action?.Invoke();
                }
            });
        }
    }
}