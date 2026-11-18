using UnityEngine;
using DG.Tweening;

public class UIIcon : MonoBehaviour
{
    // putting generic wiggle/jump animations in here
    private Tweener currentTween;

    public void Wiggle()
    {
        // currentTween?.Kill();
        //
        // currentTween = transform
        //     .DOLocalRotate(new Vector3(0, 0, 5), 0.4f)
        //     .SetLoops(-1, LoopType.Yoyo);
    }

    public void Jump(bool loop=false)
    {
        currentTween?.Kill();
        if (loop)
        {
            currentTween = transform.DOPunchPosition(new Vector3(0, 1f, 0), 2.0f).SetLoops(-1, LoopType.Restart);
        }
        else
        {
            currentTween = transform.DOPunchPosition(new Vector3(0, 2f, 0), 2.0f);             
        }
    }

    public void StopAnimation()
    {
        currentTween?.Kill();
    }
}