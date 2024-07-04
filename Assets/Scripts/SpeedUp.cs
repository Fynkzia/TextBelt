using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class SpeedUp : MonoBehaviour,IPointerDownHandler, IPointerUpHandler {
    [Inject] private UIController uiController;
    private int speedTextBoost = 2;

    public void OnPointerDown(PointerEventData data) {
        uiController._duration /= speedTextBoost;
    }

    public void OnPointerUp(PointerEventData data) {
        uiController._duration *= speedTextBoost;
    }
}
