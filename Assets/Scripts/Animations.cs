using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Animations : MonoBehaviour {
    private Button playButton;
    private EventRegistry m_EventRegistry = new EventRegistry();
    private void OnEnable() {
        var root = GetComponent<UIDocument>().rootVisualElement;
        playButton = root.Q<Button>("PlayButton");
    }

    private void Start() {
        StartCoroutine(AnimateButton());
    }

    private IEnumerator AnimateButton() {
        m_EventRegistry.RegisterCallback<TransitionEndEvent>
        (
            playButton,
            evt => playButton.ToggleInClassList("playButton_anim")
        );
        yield return new WaitForEndOfFrame();
        playButton.ToggleInClassList("playButton_anim");
    }


    private void OnDisable() {
        playButton.RemoveFromClassList("playButton_anim");
        m_EventRegistry.Dispose();
    }
}