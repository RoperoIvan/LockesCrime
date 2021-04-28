/* 
 * author : jiankaiwang
 * description : The script provides you with basic operations 
 *               of first personal camera look on mouse moving.
 * platform : Unity
 * date : 2017/12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamLook : MonoBehaviour
{
    public float speedV = 2.0f;
    public float speedH = 2.0f;
    public float angleClamp = 0f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if(!DialogueManager.hasDialog)
        {
            rotationY += speedV * Input.GetAxis("Mouse X");
            rotationX -= speedH * Input.GetAxis("Mouse Y");

            rotationX = Mathf.Clamp(rotationX, -angleClamp, angleClamp);

            transform.eulerAngles = new Vector3(rotationX, rotationY, 0);

        }
    }
}