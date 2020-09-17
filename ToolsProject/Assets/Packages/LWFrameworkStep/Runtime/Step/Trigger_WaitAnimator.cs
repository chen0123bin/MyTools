using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_WaitAnimator : Trigger_Base
{
    private Animator animator;
    public string AnimaProgress;
    private bool isTigger = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        TestTrigger();
        if (!isTigger )
        {

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > float.Parse(AnimaProgress))
            {
                OnNextEvent(gameObject);
                isTigger = true;
            }
        }

    }
}
