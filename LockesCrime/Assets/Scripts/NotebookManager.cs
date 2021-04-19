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
    private float NoteBookOpen;

    [SerializeField]
    private float NoteBookClose;

    [SerializeField]
    private float lerpDuration;

    [SerializeField]
    private List<Pages> pages;

    bool isNotebookOpen;

    private int currentFile = 0;

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
        float destination;
        float initPoint;
        if (isNotebookOpen)
        {
            destination = NoteBookClose;
            initPoint = NoteBookOpen;
        }
        else
        { 
            destination = NoteBookOpen;
            initPoint = NoteBookClose;
        }

        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            transform.position = new Vector3(transform.position.x,
                Mathf.Lerp(initPoint, destination, timeElapsed / lerpDuration),
                transform.position.z);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = new Vector3 (transform.position.x, destination, transform.position.z);
        isNotebookOpen = !isNotebookOpen;
    }

    public void MoveCursorPages(bool right)
    {

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
