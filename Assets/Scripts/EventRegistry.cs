using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventRegistry : IDisposable {
    // Single delegate to hold all unregister actions
    Action m_UnregisterActions;

    // Registers a callback for a specific VisualElement and event type (e.g. ClickEvent, MouseEnterEvent, etc.). 
    public void RegisterClick(Button element, UnityAction callback) {
        UnityAction eventCallback = new UnityAction(callback);
        element.onClick.AddListener(eventCallback);

        m_UnregisterActions += () => element.onClick.RemoveListener(eventCallback);
    }
    // Unregisters all callbacks by invoking the m_UnregisterActions delegate, then sets it to null.
    public void Dispose() {
        m_UnregisterActions?.Invoke();
        m_UnregisterActions = null;
    }
}