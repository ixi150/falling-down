using UnityEngine;
using System.Collections;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Gradient gradient;
    [SerializeField] float duration = 1;
    [SerializeField] bool isFinal = false;

    ScaleAnimator scaleAnimator;
    Animator anim;

    private void Awake()
    {
        scaleAnimator = GetComponent<ScaleAnimator>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        scaleAnimator.Bump();
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateColor());
        anim.SetBool("Active", true);

        if (isFinal)
        {
            CameraFadeOut.Ref.FadeOut();
            Player.Ref.PlayEnd();
        }
    }

    IEnumerator AnimateColor()
    {
        var elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            yield return null;
        }
    }
}
