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

    private void OnMouseOver()
    {

        if (Vector3.Distance(transform.position, player.transform.position) < interactionDistance)
        {
            interactGO.SetActive(true);
            if (Input.GetKeyDown(KeyCode.T))
            {
                CluesManager.CluesM.OnCol(this);
            }

        }
            else
                interactGO.SetActive(false);
    }

    private void OnMouseExit()
    {
        interactGO.SetActive(false);
    }
}
