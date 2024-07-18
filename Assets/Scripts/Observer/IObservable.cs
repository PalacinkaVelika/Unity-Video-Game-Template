using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IObservable : MonoBehaviour {
    List<IObserver> observers = new List<IObserver>();
    public void Subscribe(IObserver observer) {
        observers.Add(observer);
    }
    public void UnSubscribe(IObserver observer) {
        observers.Remove(observer);
    }
    protected void NotifyObservers<T>(T data) {
        foreach(IObserver o in observers) {
            o.OnNotify(data);
        }
    }
}
