using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private static LevelController instance;
    public static LevelController Instance => instance;

    [SerializeField] private PlayerController player;
    
    [SerializeField] private GameObject mainGround;
    [SerializeField] private GameObject paralaksGround;

    [SerializeField] private float rotateSpeed = 10;

    [SerializeField] private Transform[] spawnPos = new Transform[3];

    [SerializeField] private GameObject pointObj;
    [SerializeField] private GameObject dangerObj;

    private List<GameObject> pointPool = new List<GameObject>();
    private List<GameObject> dangerObjPool = new List<GameObject>();

    [SerializeField] private float respawnTime = 0;
    private float timer = 0;

    private void Awake()
    {
        instance = this;

        //greate pools

        for (int i = 0; i < 25; i++)
        {
            GameObject _go = Instantiate(pointObj, Vector3.zero, Quaternion.identity, mainGround.transform);
            _go.SetActive(false);
            pointPool.Add(_go);

            GameObject _go1 = Instantiate(dangerObj, Vector3.zero, Quaternion.identity, mainGround.transform);
            _go1.SetActive(false);
            dangerObjPool.Add(_go1);
        }

        timer = respawnTime;
    }

    private GameObject GetFree(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
            if (!list[i].activeInHierarchy)
                return list[i];

        return null;
    }

    [Range(0, 100)]
    [SerializeField] private int doSpawnChange = 80;
    [Range(0, 100)]
    [SerializeField] private int doDuoSpawnChange = 50;
    [Range(0, 100)]
    [SerializeField] private int pointSpawnChange = 50;

    [SerializeField] private float rotateSpeedMultiplay = 0.01f;

    public void UpdateSpeed()
    {
        rotateSpeed += rotateSpeedMultiplay;
        player.moveSpeed += player.moveSpeed * 0.01f;
        respawnTime -= Mathf.Clamp(rotateSpeedMultiplay * 0.75f, 0.01f, 100);
    }

    private Coroutine rotateStopCoroutine;
    public void DamageIn()
    {
        if (rotateStopCoroutine != null)
            return; // StopCoroutine(rotateStopCoroutine);

        rotateStopCoroutine = StartCoroutine(MoveTougch());
    }

    private IEnumerator MoveTougch()
    {
        float facktSpeed = rotateSpeed;
        float facktPlayerSpeed = player.moveSpeed;

        rotateSpeed = rotateSpeed / 2f;
        player.moveSpeed = player.moveSpeed / 2f;

        yield return new WaitForSeconds(0.25f);

        rotateSpeed = facktSpeed;
        player.moveSpeed = facktPlayerSpeed;
        rotateStopCoroutine = null;
    }

    public void FixedUpdate()
    {
        //if (!GameController.Instance.gameIsPlayed) return;

        mainGround.transform.Rotate(-rotateSpeed, 0, 0);
        paralaksGround.transform.Rotate(-rotateSpeed / 2, 0, 0);

        if (timer < respawnTime)
            timer += Time.deltaTime;
        else
        {
            timer = 0;
            int change = Random.Range(0, 100);
            bool[] sPos = new bool[spawnPos.Length];
            
            if (change <= doSpawnChange) // определить будет ли препядствие да/нет 80%
            {
                change = Random.Range(0, 100);
                if (change <= doDuoSpawnChange) //определить сколько будет препядствий 2 или 1 50% 
                {
                    for (int i = 0; i < 2; i++)
                    {
                        GameObject go = GetFree(dangerObjPool);
                        if (go == null) return;

                        go.SetActive(true);

                        List<Transform> _spawnPos = new List<Transform>();
                        for (int j = 0; j < spawnPos.Length; j++)
                            if (!sPos[j])
                                _spawnPos.Add(spawnPos[j]);

                        change = Random.Range(0, _spawnPos.Count);
                        sPos[change] = true;

                        go.transform.position = _spawnPos[change].position;
                        go.transform.rotation = _spawnPos[change].rotation;
                    }
                }
                else
                {
                    GameObject go = GetFree(dangerObjPool);
                    if (go == null) return;

                    go.SetActive(true);

                    change = Random.Range(0, spawnPos.Length);
                    sPos[change] = true;

                    go.transform.position = spawnPos[change].position;
                    go.transform.rotation = spawnPos[change].rotation;
                }
                        
            }

            change = Random.Range(0, 100);
            if (change <= pointSpawnChange)// будет ли спавнится поинт 50% 
            {
                GameObject go = GetFree(pointPool);
                if (go == null) return;

                go.SetActive(true);

                List<Transform> _spawnPos = new List<Transform>();
                for (int j = 0; j < spawnPos.Length; j++)
                    if (!sPos[j])
                        _spawnPos.Add(spawnPos[j]);

                change = Random.Range(0, _spawnPos.Count);
                sPos[change] = true;

                go.transform.position = _spawnPos[change].position;
                go.transform.rotation = _spawnPos[change].rotation;
            }//если поинт спавнится то рандомно в свободной позиции
        }
    }
}
