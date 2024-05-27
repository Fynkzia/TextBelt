using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class PhaseThree {

    private VisualElement _question;
    private VisualElement _answerBox;
    public ReactiveProperty<bool> PhaseReseted = new ReactiveProperty<bool>(false);
    private EventRegistry m_EventRegistry = new EventRegistry();
    public PhaseThree(VisualElement question, VisualElement answerBox) { 
        _question = question;
        _answerBox = answerBox;
    }
    public void StartPhase() {
        PhaseReseted.Value = false;
        _question.style.display = DisplayStyle.Flex;
        _answerBox.style.display = DisplayStyle.Flex;
        _question.RemoveFromClassList("question_small");
        _answerBox.RemoveFromClassList("question_small");
    }

    public void ResetPhase(bool setForPhaseTwo = true) {
        _question.AddToClassList("question_small");
        _answerBox.AddToClassList("question_small");
        m_EventRegistry.RegisterCallback<TransitionEndEvent>
        (_question,evt => {
            _question.style.display = DisplayStyle.None;
            _answerBox.style.display = DisplayStyle.None;
            
            PhaseReseted.Value = setForPhaseTwo;
            m_EventRegistry.Dispose();
        });
    }
}
