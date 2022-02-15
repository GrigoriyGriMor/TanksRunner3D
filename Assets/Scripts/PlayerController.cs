using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject mainGround;

    [SerializeField] private Transform[] spawnPos = new Transform[3];

    [SerializeField] private GameObject pointObj;
    [SerializeField] private GameObject dangerObj;

    private List<GameObject> pointPool = new List<GameObject>();
    private List<GameObject> dangerObjPool = new List<GameObject>();

    private void Awake()
    {
        //greate pools

        for (int i = 0; i < 10; i++)
        {
            GameObject _go = Instantiate(pointObj, Vector3.zero, Quaternion.identity, mainGround.transform);
            _go.SetActive(false);
            pointPool.Add(_go);

            GameObject _go1 = Instantiate(dangerObj, Vector3.zero, Quaternion.identity, mainGround.transform);
            _go1.SetActive(false);
            dangerObjPool.Add(_go1);
        }
    }

}
