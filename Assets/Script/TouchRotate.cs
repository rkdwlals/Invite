using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour
{
    private void OnMouseDown()//마우스 클릭 시
    {
        if (!GameControl.youwin)//이긴 상태 아닐 시
            transform.Rotate(0f, 0f, 90f);//클릭할 때 마다 90도씩 돌림

    }
}

