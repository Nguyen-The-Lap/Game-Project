using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(string animationName)
    {
        if (animator)
            animator.Play(animationName);
    }

    public void SetBool(string parameter, bool value)
    {
        if (animator)
            animator.SetBool(parameter, value);
    }

    public void SetTrigger(string parameter)
    {
        if (animator)
            animator.SetTrigger(parameter);
    }
}
