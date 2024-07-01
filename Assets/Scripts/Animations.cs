using UnityEngine;

public class Animations : MonoBehaviour {
    [SerializeField] private float scale;
    [SerializeField] private float expandDuration;
    private Vector3 breatheIn;
    private Vector3 breatheOut;
    private bool breathingIn = false;
    private float currentTime = 0f;

    private void Start() {
        breatheIn = transform.localScale;
        breatheOut = new Vector3(scale, scale, 1);
    }
    private void Update() {
        Vector3 targetScale = breathingIn ? breatheIn : breatheOut;
        Vector3 startScale = breathingIn ? breatheOut : breatheIn;

        currentTime += Time.deltaTime;
        float lerpFactor = currentTime / expandDuration;

        transform.localScale = Vector3.Lerp(startScale,targetScale, lerpFactor);

        if (lerpFactor >= 1.0f) {
            breathingIn = !breathingIn;
            currentTime = 0f;
        }
    }
}