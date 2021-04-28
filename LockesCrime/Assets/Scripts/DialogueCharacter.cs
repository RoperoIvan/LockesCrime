using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCharacter : MonoBehaviour
{
    public float interactionDistance;
    public GameObject player;
    public GameObject interactGO;
    public DialogueManager dManager;
    public Characters character;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseOver()
    {
        if(!DialogueManager.hasDialog)
        {
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) < interactionDistance)
            {
                interactGO.SetActive(true);
                if (Input.GetKeyDown(KeyCode.T))
                {
                    interactGO.SetActive(false);
                    dManager.gameObject.SetActive(true);
                    dManager.RefreshDialogueContainer((int)character);
                }
            }
            else
                interactGO.SetActive(false);
        }
    }

    private void OnMouseExit()
    {
        interactGO.SetActive(false);
    }

    public enum Characters
    {
        JAMES,
        GRACE,
        DIANE
    }
}
