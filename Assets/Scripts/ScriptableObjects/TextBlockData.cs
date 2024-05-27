using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New TextBlockData", menuName = "TextBlockData", order = 51)]
public class TextBlockData : ScriptableObject
{
    [SerializeField]
    public string text;
    [SerializeField]
    public List<Question> questions;

    [System.Serializable]
    public class Question {
        [SerializeField]
        public string text;

        [SerializeField]
        public List<Answer> answers;
    }
    [System.Serializable]
    public class Answer {
        [SerializeField]
        public string text;
        [SerializeField]
        public bool isRight;
    }
}
