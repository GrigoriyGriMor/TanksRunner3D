using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public void AnimStart()
    {
        anim.SetTrigger("Start");
        if (GameController.Instance)
            GameController.Instance.UpdatePoint(1);
    }
}
