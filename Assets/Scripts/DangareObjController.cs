using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangareObjController : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public void SetDamage()
    {
        if (anim != null) anim.SetTrigger("Start");

        if (LevelController.Instance)
            LevelController.Instance.DamageIn();

        if (GameController.Instance)
            GameController.Instance.SetDamage();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LevelController>())
        {
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }    
    }
}
