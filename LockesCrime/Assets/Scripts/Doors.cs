using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField]
    private Quaternion openRot;
    
    [SerializeField]
    private float interactionDistance;

    [SerializeField]
    private float lerpOpenTime;

    [SerializeField]
    private GameObject doorLabel;

    private Quaternion closeRot;

    private bool isOpen;

    private bool isInteracting;

    private Transform pivotTransform;

    // Start is called before the first frame update
    void Start()
    {
        pivotTransform = transform.parent;
        isOpen = false;
        closeRot = pivotTransform.rotation;
    }

    private void OnMouseOver()
    {
        if (isInteracting)
        {
            doorLabel.SetActive(false);
            return;
        }

        if (Vector3.Distance(transform.position, CharacterController.Player.transform.position) < interactionDistance)
        {
            doorLabel.SetActive(true);

            if (Input.GetKeyDown(KeyCode.T))
            {
                StartCoroutine(MoveDoor());
                isInteracting = true;
            }

        }
        else
            doorLabel.SetActive(false);
    }
    IEnumerator MoveDoor()
    {
        float timeElapsed = 0;

        Quaternion initPos = pivotTransform.rotation;
        Quaternion finalPos = openRot;

        if (isOpen)
        {
            finalPos = closeRot;
            isOpen = false;
        }
        else
        {
            isOpen = true;
        }

        while (timeElapsed < lerpOpenTime)
        {
            pivotTransform.rotation = Quaternion.Lerp(initPos, finalPos, timeElapsed / lerpOpenTime);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        isInteracting = false;
        pivotTransform.rotation = finalPos;
    }
}
