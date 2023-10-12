using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectObject : MonoBehaviour
{
    Animator anim;
    public bool isDestroyParent;
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            if (isDestroyParent)
            {
                Destroy(transform.parent.gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}
