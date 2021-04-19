using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static bool hasDialog = false;
    public Dialogue currentDialogueNode;
    public GameObject responseContainer;
    public GameObject responsePrefab;
    public TMP_Text dialogueTxt;

    private Dialogue[] lastDialogueNodes = new Dialogue[3]; //0: James, 1: Grace, 2: Diane
    private Dialogue[] nodesJames;
    private Dialogue[] nodesGrace;
    private Dialogue[] nodesDiane;

    private int currentChar;

    private void Awake()
    {
        nodesJames = Resources.LoadAll<Dialogue>("DialogueNodes/James");
        nodesGrace = Resources.LoadAll<Dialogue>("DialogueNodes/Grace");
        nodesDiane = Resources.LoadAll<Dialogue>("DialogueNodes/Diane");

        //RefreshDialogueContainer(1);
    }
    public void RefreshDialogueContainer(int character)
    {
        Dialogue[] charNodes = { };
        hasDialog = true;
        Cursor.lockState = CursorLockMode.None;
        currentChar = character;

        if (lastDialogueNodes[character] == null)
        {
            switch (character)
            {
                case 0:
                    charNodes = nodesJames;
                    break;
                case 1:
                    charNodes = nodesGrace;
                    break;
                case 2:
                    charNodes = nodesDiane;
                    break;
                default:
                    break;
            }
            lastDialogueNodes[character] = charNodes[0];
        }

        currentDialogueNode = lastDialogueNodes[character];
        dialogueTxt.text = currentDialogueNode.dialogues[0];

        for (int i = 0; i <= currentDialogueNode.responses.Length -1; ++i)
        {
            GameObject newResponse = Instantiate(responsePrefab, responseContainer.transform);
            Dialogue nDial = currentDialogueNode.responses[i].dialogueNode;
            if(nDial == null)
                newResponse.GetComponent<Button>().onClick.AddListener(() => { CloseDialogue(); });
            else
                newResponse.GetComponent<Button>().onClick.AddListener(() => { GoToNextNode(nDial); });

            newResponse.transform.GetChild(0).GetComponent<TMP_Text>().text = currentDialogueNode.responses[i].response;
        }
    }

    public void GoToNextNode(Dialogue nexNode)
    {
        currentDialogueNode = nexNode;

        CleanResponses();
        CreateResponses();

        dialogueTxt.text = currentDialogueNode.dialogues[0];

    }

    public void CloseDialogue()
    {
        CleanResponses();
        lastDialogueNodes[currentChar] = currentDialogueNode;
        hasDialog = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);
    }

    private void CleanResponses()
    {
        foreach(Transform child in responseContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateResponses()
    {
        for (int i = 0; i <= currentDialogueNode.responses.Length - 1; ++i)
        {
            GameObject newResponse = Instantiate(responsePrefab, responseContainer.transform);
            Dialogue nDial = currentDialogueNode.responses[i].dialogueNode;
            if (nDial == null)
                newResponse.GetComponent<Button>().onClick.AddListener(() => { CloseDialogue(); });
            else
                newResponse.GetComponent<Button>().onClick.AddListener(() => { GoToNextNode(nDial); });
            newResponse.transform.GetChild(0).GetComponent<TMP_Text>().text = currentDialogueNode.responses[i].response;
        }
    }
}
