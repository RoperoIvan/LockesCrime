using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWorld : MonoBehaviour
{
    public Transform lookAt;
    public Vector3 offset;

    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.current;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePosition()
    {
        Vector3 pos = cam.WorldToScreenPoint(lookAt.position + offset);

        if (transform.position != pos)
            transform.position = pos;
    }
}
