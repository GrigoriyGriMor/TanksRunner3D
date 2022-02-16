using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float moveToCenterSpeed = 0.05f;


    public void AnimStart()
    {
        if (anim != null) anim.SetTrigger("Start");

        if (GameController.Instance)
            GameController.Instance.UpdatePoint(1);

        if (LevelController.Instance)
            LevelController.Instance.UpdateSpeed();

        StartCoroutine(PosToCenter());
    }

    private IEnumerator PosToCenter()
    {
        while (Vector3.Distance(transform.position, new Vector3(0, transform.position.y, transform.position.z)) > 0.1f)
        {
            yield return new WaitForFixedUpdate();
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y, transform.position.z), moveToCenterSpeed);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LevelController>())
        {
            transform.position = Vector3.zero;
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
    }
}
