using UnityEngine;
using System.Collections;
using System.Linq;

public class ScaleAnimator : MonoBehaviour
{
    [SerializeField] bool useFixedUpdate;
    [SerializeField] bool loopAnimationOnStart;
    [SerializeField] AnimationCurve shrinkCurve;
    [SerializeField] AnimationCurve bumpCurve;
    [SerializeField] AnimationCurve loopCurve;

    Vector3 initialScale;
    readonly YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
    readonly YieldInstruction waitForUpdate = new WaitForEndOfFrame();

    YieldInstruction Wait { get { return useFixedUpdate ? waitForFixedUpdate : waitForUpdate; } }

    public void Bump()
    {
        StartCoroutine(AnimateScale(bumpCurve));
    }

    public void Shrink()
    {
        StartCoroutine(AnimateScale(shrinkCurve));
    }

    public void Loop()
    {
        StartCoroutine(LoopScale(loopCurve));
    }

    public void ResetScale()
    {
        transform.localScale = initialScale;
        StopAllCoroutines();
    }

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    private void Start()
    {
        if (loopAnimationOnStart)
        {
            Loop();
        }
    }

    private void OnEnable()
    {
        ResetScale();
    }

    IEnumerator AnimateScale(AnimationCurve curve)
    {
        var duration = curve.keys.Last().time;
        var elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = initialScale * curve.Evaluate(elapsed);
            yield return Wait;
        }
    }

    IEnumerator LoopScale(AnimationCurve curve)
    {
        var duration = curve.keys.Last().time;
        var elapsed = 0f;
        while (true)
        {
            elapsed += Time.deltaTime;
            elapsed %= duration;
            transform.localScale = initialScale * curve.Evaluate(elapsed);
            yield return Wait;
        }
    }
}

