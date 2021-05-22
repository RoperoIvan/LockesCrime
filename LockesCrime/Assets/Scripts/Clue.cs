using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    public string clueName;
    public string description;
    public UnBlockedResponse[] unblockedResponses;
    [System.Serializable]
    public struct UnBlockedResponse
    {
        public string character;
        public int idNode;
        public int idResponse;
    }

    [SerializeField]
    private float interactionDistance;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject interactGO;

    [SerializeField]
    private Transform posFrontCam;

    private Vector3 initPos;
    private Quaternion initRotation;

    private bool checking_clue;

    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;
    public float angleClamp = 90f;
    // smooth the mouse moving
    private Vector2 smoothV;
    // get the incremental value of mouse moving
    private Vector2 mouseLook;

    private void Start()
    {
        initPos = transform.position;
    }

    private void Update()
    {
        if(checking_clue)
        {
            var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            // the interpolated float result between the two float values
            smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
            // incrementally add to the camera look
            mouseLook += smoothV;

            // vector3.right means the x-axis
            transform.rotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right) * Quaternion.AngleAxis(mouseLook.x, Vector3.up);


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MouseCamLook.mouseCamLook.moveCam = true;
                CharacterController.characterController.hasInteraction = false;
                StartCoroutine(MoveClueToOriginalPos());
                transform.parent = null;
                checking_clue = false;
            }
        }
    }
    private void OnMouseOver()
    {

        if (Vector3.Distance(transform.position, player.transform.position) < interactionDistance)
        {
            interactGO.SetActive(true);
            if (transform.position == initPos)
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    StartCoroutine(MoveClueFrontCamera());
                    CluesManager.CluesM.OnCol(this);
                    transform.parent = posFrontCam;
                    MouseCamLook.mouseCamLook.moveCam = false;
                    checking_clue = true;
                    CharacterController.characterController.hasInteraction = true;
                }
            }
        }
            else
                interactGO.SetActive(false);
    }

    private void OnMouseExit()
    {
        interactGO.SetActive(false);
    }

    private IEnumerator MoveClueFrontCamera()
    {
        Vector3 initPoint = transform.position;

        float lerpDuration = 1.0f;
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(initPoint, posFrontCam.position, timeElapsed / lerpDuration);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = posFrontCam.position;
    }

    private IEnumerator MoveClueToOriginalPos()
    {
        Vector3 initPoint = transform.position;
        Quaternion initRot = transform.rotation;
        float lerpDuration = 1.0f;
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(initPoint, initPos, timeElapsed / lerpDuration);

            transform.rotation = Quaternion.Lerp(initRot, initRotation, timeElapsed / lerpDuration);


            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = initPos;
        transform.rotation = initRotation;
    }
}
