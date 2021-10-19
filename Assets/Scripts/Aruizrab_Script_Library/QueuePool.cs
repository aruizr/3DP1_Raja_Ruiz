using System;
using System.Collections.Generic;

public class QueuePool<T> : ObjectPool<T>
{
    private readonly Queue<T> _pool;

    public QueuePool(Func<T> createFunc, Action<T> actionOnGet, Action<T> actionOnRelease, int size) : base(createFunc,
        actionOnGet, actionOnRelease, size)
    {
        _pool = new Queue<T>();
        for (var i = 0; i < this.size; i++) _pool.Enqueue(this.createFunc());
    }

    public override T Get()
    {
        var t = _pool.Dequeue();
        actionOnGet?.Invoke(t);
        return t;
    }

    public override void Return(T t)
    {
        actionOnRelease?.Invoke(t);
        _pool.Enqueue(t);
    }
}