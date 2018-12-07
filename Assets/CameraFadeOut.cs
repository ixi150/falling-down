using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFadeOut : MonoBehaviour {
    [SerializeField]
    Gradient gradient;
    [SerializeField] float duration = 1;

    public static CameraFadeOut Ref { get; private set; }
    new Camera camera;

    private void Awake()
    {
        Ref = this;
        camera = GetComponent<Camera>();
    }

    public void FadeOut()
    {
        StartCoroutine(AnimateColor());
    }

    IEnumerator AnimateColor()
    {
        var elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            camera.backgroundColor = gradient.Evaluate(elapsed / duration);
            yield return null;
        }
    }
}
