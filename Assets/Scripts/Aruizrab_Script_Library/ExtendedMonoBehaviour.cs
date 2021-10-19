using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class ExtendedMonoBehaviour : MonoBehaviour
{
    protected CoroutineBuilder Invoke(UnityAction action)
    {
        return gameObject.AddComponent<CoroutineBuilder>().Invoke(action);
    }

    protected class CoroutineBuilder : MonoBehaviour
    {
        private UnityAction _action;
        private bool _cancelOnDisable;
        private IEnumerator _currentCoroutine;
        private float _interval;

        private void OnDisable()
        {
            if (!_cancelOnDisable) return;
            Cancel();
        }

        public CoroutineBuilder Invoke(UnityAction action)
        {
            _action = action;
            return this;
        }

        public CoroutineBuilder AfterSeconds(float seconds)
        {
            _currentCoroutine = RunAfterSeconds(seconds);
            StartCoroutine(_currentCoroutine);
            return this;
        }

        private IEnumerator RunAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _action?.Invoke();
            Destroy(this);
        }

        public CoroutineBuilder ForTimes(int times)
        {
            _currentCoroutine = RunForTimes(times);
            StartCoroutine(_currentCoroutine);
            return this;
        }

        private IEnumerator RunForTimes(int times)
        {
            for (var i = 0; i < times; i++)
            {
                yield return new WaitForSeconds(_interval);
                _action?.Invoke();
            }

            Destroy(this);
        }

        public CoroutineBuilder EverySeconds(float seconds)
        {
            _interval = seconds;
            return this;
        }

        public CoroutineBuilder While([NotNull] Func<bool> predicate)
        {
            _currentCoroutine = RunWhile(predicate);
            StartCoroutine(_currentCoroutine);
            return this;
        }

        private IEnumerator RunWhile(Func<bool> predicate)
        {
            while (predicate.Invoke())
            {
                yield return new WaitForSeconds(_interval);
                _action?.Invoke();
            }

            Destroy(this);
        }

        public void Cancel()
        {
            StopCoroutine(_currentCoroutine);
            Destroy(this);
        }

        public CoroutineBuilder AfterEndOfFrame()
        {
            _currentCoroutine = RunAfterEndOfFrame();
            StartCoroutine(_currentCoroutine);
            return this;
        }

        private IEnumerator RunAfterEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            _action?.Invoke();
        }

        public CoroutineBuilder CancelOnDisable(bool condition)
        {
            _cancelOnDisable = condition;
            return this;
        }
    }
}