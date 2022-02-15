using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangareObjController : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LevelController>())
        {
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }    
    }
}
