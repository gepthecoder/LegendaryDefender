using UnityEngine;

public class AE_AnimatorSpeed : MonoBehaviour {

    public AnimationCurve SpeedCurve = AnimationCurve.EaseInOut(0, 1, 1, 1);
    public bool IsLoop;

    Animator anim;
    [HideInInspector]
    public bool canUpdate;
    
    AnimatorStateInfo info;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        info = anim.GetCurrentAnimatorStateInfo(0);
        canUpdate = true;
    }

    private void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        if (canUpdate)
        {
            var eval = SpeedCurve.Evaluate(Mathf.Clamp01(info.normalizedTime));
            anim.speed = eval;
        }
    }
}
