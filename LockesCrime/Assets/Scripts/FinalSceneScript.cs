using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalSceneScript : MonoBehaviour
{
    [SerializeField]
    private Transform cluesFound;

    [SerializeField]
    private Transform cluesNotFound;

    [SerializeField]
    private Text guilty;

    [SerializeField]
    private Text description;

    [SerializeField]
    private GameObject TextPrefab;

    // Start is called before the first frame update
    void Start()
    {
        description.text = CluesManager.CluesM.getDescription();

        guilty.text = CluesManager.CluesM.getGuilty();

        List<GameObject> cluesSaved = CluesManager.CluesM.getCluesSaved();

        for(int i = 0; i < cluesSaved.Count; ++i)
        {
            GameObject t = Instantiate(TextPrefab, cluesFound);
            t.GetComponent<Text>().text = cluesSaved[i].GetComponent<Clue>().clueName;
        }

        List<GameObject> clues = CluesManager.CluesM.getClues();
        for (int i = 0; i < clues.Count; ++i)
        {
            bool found = false;
            for (int j = 0; j < cluesSaved.Count; ++j)
            {
                if (cluesSaved[j].GetComponent<Clue>().clueName == clues[i].GetComponent<Clue>().clueName)
                {
                    found = true;
                    break;
                }
            }
            if(!found)
            {
                GameObject t = Instantiate(TextPrefab, cluesNotFound);
                t.GetComponent<Text>().text = clues[i].GetComponent<Clue>().clueName;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
