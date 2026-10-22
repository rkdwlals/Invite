using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject target;//카메라가 따라다닐 타겟

    [SerializeField]
    private float moveSpeed;//따라가는 속도


    void Start()
    {

    }

    void Update()
    {
        Vector3 tpos = new Vector3(target.transform.position.x, target.transform.position.y, -10);
        transform.position = Vector3.Lerp(transform.position, tpos, moveSpeed * Time.deltaTime);
    }
}