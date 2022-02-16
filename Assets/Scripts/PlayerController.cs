using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject playerVisual;

    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform centerPos;
    [SerializeField] private Transform rightPos;

    [Range(0, 2)]
    [SerializeField] private int currentPos;

    public float moveSpeed = 0.1f;
    [SerializeField] private float moveAngle = 15;

    private Vector2 startInputPos;

    public void InputStart()
    {
#if UNITY_ANDROID || UNITY_IOS
        startInputPos = Touchscreen.current.position.ReadValue();
#else
        startInputPos = Mouse.current.position.ReadValue();
#endif
    }

    public void InputEnd()
    {
        Vector2 newVector = Vector2.zero;
#if UNITY_ANDROID || UNITY_IOS
        newVector = Touchscreen.current.position.ReadValue();
#else
        newVector = Mouse.current.position.ReadValue();
#endif

        if (newVector.x < startInputPos.x)
        {
            switch (currentPos)
            {
                case 1:
                    currentPos = 0;
                    break;
                case 2:
                    currentPos = 1;
                    break;
            }
        }
        else
        if (newVector.x > startInputPos.x)
        {
            switch (currentPos)
            {
                case 0:
                    currentPos = 1;
                    break;
                case 1:
                    currentPos = 2;
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        //if (!GameController.Instance.gameIsPlayed) return;

        switch (currentPos)
        {
            case 0:
                if (Vector3.Distance(transform.position, new Vector3(leftPos.position.x, transform.position.y, transform.position.z)) > 0.01f)
                {
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x - moveSpeed, leftPos.position.x, transform.position.x), transform.position.y, transform.position.z);
                    playerVisual.transform.localEulerAngles = new Vector3(0, -moveAngle, 0);
                }
                else
                    playerVisual.transform.localEulerAngles = new Vector3(0, 0, 0);

                break;
            case 1:
                if (Vector3.Distance(transform.position, new Vector3(centerPos.position.x, transform.position.y, transform.position.z)) > 0.01f)
                {
                    if (transform.position.x < centerPos.position.x)
                    {
                        transform.position = new Vector3(Mathf.Clamp(transform.position.x + moveSpeed, transform.position.x, centerPos.position.x), transform.position.y, transform.position.z);
                        playerVisual.transform.localEulerAngles = new Vector3(0, moveAngle, 0);
                    }
                    else
                    {
                        transform.position = new Vector3(Mathf.Clamp(transform.position.x - moveSpeed, centerPos.position.x, transform.position.x), transform.position.y, transform.position.z);
                        playerVisual.transform.localEulerAngles = new Vector3(0, -moveAngle, 0); 
                    }
                }
                else
                    playerVisual.transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case 2:
                if (Vector3.Distance(transform.position, new Vector3(rightPos.position.x, transform.position.y, transform.position.z)) > 0.01f)
                {
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x + moveSpeed, transform.position.x, rightPos.position.x), transform.position.y, transform.position.z);
                    playerVisual.transform.localEulerAngles = new Vector3(0, moveAngle, 0); 
                }
                else
                    playerVisual.transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DangareObjController>())
            other.GetComponent<DangareObjController>().SetDamage();

        if (other.GetComponent<PointController>())
            other.GetComponent<PointController>().AnimStart();
    }
}
