using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager dialogueManager;
    public static bool hasDialog = false;
    public Dialogue currentDialogueNode;
    public GameObject responseContainer;
    public GameObject responsePrefab;
    public TMP_Text dialogueTxt;

    private Dialogue[] lastDialogueNodes = new Dialogue[5]; //0: James, 1: Grace, 2: Diane, 3: Officer, 4: Sheriff
    public Dialogue[] nodesJames;
    public Dialogue[] nodesGrace;
    public Dialogue[] nodesDiane;
    public Dialogue[] nodesOfficer;
    public Dialogue[] nodesSheriff;

    private int currentChar;

    private void Awake()
    {
        if (dialogueManager != null)
        {
            Debug.LogError("There is more than one instance!");
            return;
        }

        dialogueManager = this;
        gameObject.SetActive(false);
        nodesJames = Resources.LoadAll<Dialogue>("DialogueNodes/James");
        nodesGrace = Resources.LoadAll<Dialogue>("DialogueNodes/Grace");
        nodesDiane = Resources.LoadAll<Dialogue>("DialogueNodes/Diane");
        nodesOfficer = Resources.LoadAll<Dialogue>("DialogueNodes/Officer");
        nodesSheriff = Resources.LoadAll<Dialogue>("DialogueNodes/Sheriff");
    }
    public void RefreshDialogueContainer(int character)
    {
        Dialogue[] charNodes = { };
        hasDialog = true;
        CharacterController.characterController.hasInteraction = true;
        MouseCamLook.mouseCamLook.moveCam = false;
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
                case 3:
                    charNodes = nodesOfficer;
                    break;
                case 4:
                    charNodes = nodesSheriff;
                    break;
                default:
                    break;
            }
            lastDialogueNodes[character] = charNodes[0];
        }

        currentDialogueNode = lastDialogueNodes[character];

        for (int i = 0; i <= currentDialogueNode.responses.Length -1; ++i)
        {
            GameObject newResponse = Instantiate(responsePrefab, responseContainer.transform);
            Dialogue nDial = currentDialogueNode.responses[i].dialogueNode;
            if(nDial == null)
                newResponse.GetComponent<Button>().onClick.AddListener(() => { CloseDialogue(); });
            else
                newResponse.GetComponent<Button>().onClick.AddListener(() => { GoToNextNode(nDial); });

            newResponse.transform.GetChild(0).GetComponent<TMP_Text>().text = currentDialogueNode.responses[i].response;

            if (currentDialogueNode.responses[i].isInvisible)
                newResponse.SetActive(false);
        }
        StartCoroutine(RevealText(currentDialogueNode.dialogues));
    }

    public void GoToNextNode(Dialogue nexNode)
    {
        currentDialogueNode = nexNode;

        CleanResponses();
        CreateResponses();

        
        StartCoroutine(RevealText(currentDialogueNode.dialogues));
    }

    public void CloseDialogue()
    {
        CleanResponses();
        lastDialogueNodes[currentChar] = currentDialogueNode;
        hasDialog = false;
        CharacterController.characterController.hasInteraction = false;
        MouseCamLook.mouseCamLook.moveCam = true;
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
        for (int i = 0; i < currentDialogueNode.responses.Length; ++i)
        {
            GameObject newResponse = Instantiate(responsePrefab, responseContainer.transform);
            Dialogue nDial = currentDialogueNode.responses[i].dialogueNode;
            if (nDial == null)
                newResponse.GetComponent<Button>().onClick.AddListener(() => { CloseDialogue(); });
            else
                newResponse.GetComponent<Button>().onClick.AddListener(() => { GoToNextNode(nDial); });
            newResponse.transform.GetChild(0).GetComponent<TMP_Text>().text = currentDialogueNode.responses[i].response;

            if (currentDialogueNode.responses[i].isInvisible)
                newResponse.SetActive(false);
        }
    }

    private IEnumerator RevealText(string[] dialogue)
    {
        for(int i = 0; i <  dialogue.Length; ++i)
        {
            dialogueTxt.text = dialogue[i];
            var originalString = dialogueTxt.text;
            dialogueTxt.text = "";

            var numCharsRevealed = 0;
            while (numCharsRevealed < originalString.Length)
            {
                ++numCharsRevealed;
                dialogueTxt.text = originalString.Substring(0, numCharsRevealed);

                yield return new WaitForSecondsRealtime(0.07f);
            }
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
