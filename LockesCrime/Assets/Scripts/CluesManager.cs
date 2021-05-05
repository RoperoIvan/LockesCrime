using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CluesManager : MonoBehaviour
{
    public static CluesManager CluesM;

    [SerializeField]
    private GameObject TextPrefab;

    [SerializeField]
    private Transform CluesPage;

    [SerializeField]
    private Transform NoteBook;

    private List<Clue> cluesSaved;

    void Awake()
    {
        if (CluesM != null)
        {
            Debug.LogError("There is more than one instance!");
            return;
        }

        CluesM = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        cluesSaved = new List<Clue>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCol(Clue clue)
    {
        Debug.Log(cluesSaved.Count);

        for (int i = 0; i < cluesSaved.Count; ++i)
        {
            if (cluesSaved[i].Equals(clue))
                return;
        }

        Dialogue.Responses resp = null;
        foreach(Clue.UnBlockedResponse node in clue.unblockedResponses)
        {
            if(node.character.Contains("Grace"))
            {

                resp = DialogueManager.dialogueManager.nodesGrace[node.idNode].responses[node.idResponse];
                break;
            }
            else if(node.character.Contains("James"))
            {
                resp = DialogueManager.dialogueManager.nodesJames[node.idNode].responses[node.idResponse];
                break;
            }
            else
            {
                resp = DialogueManager.dialogueManager.nodesDiane[node.idNode].responses[node.idResponse];
                break;
            }
        }
        
        if(resp != null)
        {
            resp.isInvisible = !resp.isInvisible;
        }

        GameObject newText = Instantiate(TextPrefab, CluesPage);
        newText.GetComponent<TextMeshPro>().text = clue.clueName;
        cluesSaved.Add(clue);
    }
}
