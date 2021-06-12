using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class CluesManager : MonoBehaviour
{
    public static CluesManager CluesM;

    [System.Serializable]
    struct histories
    {
        public string guilty;
        public string descrition;
        public List<GameObject> clues;
    }

    [SerializeField]
    private GameObject TextPrefab;

    [SerializeField]
    private Transform CluesPage;

    [SerializeField]
    private Transform NoteBook;

    [SerializeField]
    private List<histories> historyBranches;

    private List<Clue> cluesSaved;

    private List<GameObject> cluesFounded;

    private int history;

    void Awake()
    {
        if (CluesM != null)
        {
            Debug.LogError("There is more than one instance!");
            return;
        }

        CluesM = this;

        DontDestroyOnLoad(this.gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        cluesSaved = new List<Clue>();
        cluesFounded = new List<GameObject>();
        selectHistoy();
    }

    void selectHistoy()
    {
        history = Random.Range(0, historyBranches.Count);

       for(int i = 0; i < historyBranches[history].clues.Count; ++i){
            historyBranches[history].clues[i].SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            SceneManager.LoadScene(2);
        }
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
        foreach (Clue.UnBlockedResponse node in clue.unblockedResponses)
        {
            if (node.character.Contains("Grace"))
            {

                resp = DialogueManager.dialogueManager.nodesGrace[node.idNode].responses[node.idResponse];
                break;
            }
            else if (node.character.Contains("James"))
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

        if (resp != null)
        {
            resp.isInvisible = !resp.isInvisible;
        }

        GameObject newText = Instantiate(TextPrefab, CluesPage);
        newText.GetComponent<TextMeshPro>().text = clue.clueName;
        cluesSaved.Add(clue);
        cluesFounded.Add(clue.gameObject);
    }

    public string getDescription()
    {
        return historyBranches[history].descrition;
    }

    public string getGuilty()
    {
        return historyBranches[history].guilty;
    }

    public List<GameObject> getCluesSaved()
    {
        return cluesFounded;
    }

    public List<GameObject> getClues() { 
        return historyBranches[history].clues;
    }
}
