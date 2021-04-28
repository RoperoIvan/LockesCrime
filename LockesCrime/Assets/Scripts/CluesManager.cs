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

    private List<string> cluesSaved;

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
        cluesSaved = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCol(Collider collision)
    {
        Debug.Log(cluesSaved.Count);

        for (int i = 0; i < cluesSaved.Count; ++i)
        {
            if (cluesSaved[i].CompareTo(collision.gameObject.name) == 0)
            {
                return;
            }
        }

        GameObject newText = Instantiate(TextPrefab, CluesPage);
        newText.GetComponent<TextMeshPro>().text = collision.gameObject.name;
        cluesSaved.Add(collision.gameObject.name);
    }

}
