using System;
using System.Collections.Generic;

namespace App.Models;

public sealed class Unsubscriber<T> : IDisposable
{
    private readonly List<IObserver<T>> _observers;
    private readonly IObserver<T> _observer;

    public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
    {
        _observers = observers ?? throw new ArgumentNullException(nameof(observers));
        _observer = observer ?? throw new ArgumentNullException(nameof(observer));
    }

    public void Dispose()
    {
        _observers.Remove(_observer);
        Console.WriteLine("Observer Removed");
    }
}