using System;
using UnityEngine.Events;

public abstract class ObjectPool<T> : IObjectPool<T>
{
    protected readonly Action<T> actionOnGet;
    protected readonly Action<T> actionOnRelease;
    protected readonly Func<T> createFunc;
    protected readonly int size;

    protected ObjectPool(Func<T> createFunc, Action<T> actionOnGet, Action<T> actionOnRelease,
        int size)
    {
        this.createFunc = createFunc;
        this.actionOnGet = actionOnGet;
        this.actionOnRelease = actionOnRelease;
        this.size = size;
    }

    public virtual T Get()
    {
        throw new NotImplementedException();
    }

    public virtual void Return(T t)
    {
        throw new NotImplementedException();
    }
}