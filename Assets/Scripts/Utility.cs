using UnityEngine;

public static class Utility
{
    public static bool AnimationFinished(Animator animator, string animation)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animation) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
    }
}
