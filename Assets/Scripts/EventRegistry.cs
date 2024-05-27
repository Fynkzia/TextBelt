using System;
using UnityEngine.UIElements;

public class EventRegistry : IDisposable {
    // Single delegate to hold all unregister actions
    Action m_UnregisterActions;

    // Registers a callback for a specific VisualElement and event type (e.g. ClickEvent, MouseEnterEvent, etc.). 
    public void RegisterCallback<TEvent>(VisualElement visualElement, Action<TEvent> callback) where TEvent : EventBase<TEvent>, new() {
        EventCallback<TEvent> eventCallback = new EventCallback<TEvent>(callback);
        visualElement.RegisterCallback(eventCallback);

        m_UnregisterActions += () => visualElement.UnregisterCallback(eventCallback);
    }
    // Unregisters all callbacks by invoking the m_UnregisterActions delegate, then sets it to null.
    public void Dispose() {
        m_UnregisterActions?.Invoke();
        m_UnregisterActions = null;
    }
}