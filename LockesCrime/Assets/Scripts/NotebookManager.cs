using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookManager : MonoBehaviour
{
    public static NotebookManager notebookM;

    [System.Serializable]
    struct Pages
    {
        public GameObject page;
        public Quaternion openPageRotation;
        public Quaternion closePageRotation;
    }

    [SerializeField]
    private GameObject NoteBookOpen;

    [SerializeField]
    private GameObject NoteBookClose;

    [SerializeField]
    private float lerpDuration;

    [SerializeField]
    private List<Pages> pages;

    [SerializeField]
    private List<GameObject> notebookPages;

    [SerializeField]
    private Color NotebookCol;

    [SerializeField]
    private Color SelectedPageColor;

    bool isNotebookOpen;

    private int currentFile = 0;

    private int currentPage = 0;

    void Awake()
    {
        notebookM = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isNotebookOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator TakeNotebook()
    {
        Vector3 destination;
        Vector3 initPoint;
        if (isNotebookOpen)
        {
            destination = NoteBookClose.transform.position;
            initPoint = NoteBookOpen.transform.position;
        }
        else
        { 
            destination = NoteBookOpen.transform.position;
            initPoint = NoteBookClose.transform.position;
        }

        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(initPoint, destination, timeElapsed / lerpDuration);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = destination;
        isNotebookOpen = !isNotebookOpen;
    }

    public void MoveCursorPages(bool right)
    {
        if(currentPage != notebookPages.Count - 1 && right)
        {
            if(currentPage%2 == 0)
            {
                StartCoroutine(PasePage(true));
            }

            notebookPages[currentPage].GetComponent<Renderer>().material.SetColor("_Color", NotebookCol);
            currentPage++;
            notebookPages[currentPage].GetComponent<Renderer>().material.SetColor("_Color", SelectedPageColor);
        }
        else if(currentPage != 0 && !right)
        {
            if(currentPage % 2 == 1)
            {
                StartCoroutine(PasePage(false));
            }
            notebookPages[currentPage].GetComponent<Renderer>().material.SetColor("_Color", NotebookCol);
            currentPage--;
            notebookPages[currentPage].GetComponent<Renderer>().material.SetColor("_Color", SelectedPageColor);
        }
    }

    public IEnumerator PasePage(bool next)
    {
        Quaternion destination;
        Quaternion initPoint;
        if (next)
        {
            if (currentFile == pages.Count)
                yield break;
            destination = transform.rotation * pages[currentFile].openPageRotation;
            initPoint = pages[currentFile].page.transform.rotation;
        }
        else
        {
            if (currentFile == 0)
                yield break;
            --currentFile;
            destination = transform.rotation * pages[currentFile].closePageRotation;
            initPoint = pages[currentFile].page.transform.rotation;
        }

        if (initPoint != pages[currentFile].page.transform.rotation)
            yield break;

        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            pages[currentFile].page.transform.rotation = Quaternion.Lerp(initPoint, destination, timeElapsed / lerpDuration);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        pages[currentFile].page.transform.rotation = destination;
        if (next)
            ++currentFile;
    }
}
