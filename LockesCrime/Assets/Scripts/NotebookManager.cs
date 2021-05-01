using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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

    [System.Serializable]
    struct VerticalNotebook
    {
        public GameObject first;
        public GameObject second;
        public GameObject childFirst;
        public GameObject childSecond;
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
    private List<VerticalNotebook> notebookNav;

    [SerializeField]
    private InputField inputField;

    [SerializeField]
    private Material OriginalColor;

    [SerializeField]
    private Material SelectedColor;

    bool isNotebookOpen;

    private int currentFile = 0;
    private int currentPage = 0;
    private GameObject clueSelected;
    private GameObject currentTitle;
    private bool isClueSelected;

    private float depthVal = -27.62904F;
    void Awake()
    {
        notebookM = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isNotebookOpen = false;
        currentTitle = notebookNav[0].first;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) &&
            EventSystem.current.currentSelectedGameObject == inputField.gameObject)
        {
            notebookNav[currentPage].childSecond.GetComponent<TextMeshPro>().text = inputField.text;
            inputField.text = null;
            EventSystem.current.SetSelectedGameObject(null, null);
            inputField.gameObject.SetActive(false);
        }
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

    public void MoveVCursorPages(bool Down)
    {
        if (currentPage == 0 || notebookNav[currentPage].second == null)
            return;

        if (Down)
        {
            currentTitle = notebookNav[currentPage].second;
            notebookNav[currentPage].first.GetComponent<MeshRenderer>().material = OriginalColor;
            notebookNav[currentPage].second.GetComponent<MeshRenderer>().material = SelectedColor;

        }
        else
        {
            currentTitle = notebookNav[currentPage].first;
            notebookNav[currentPage].second.GetComponent<MeshRenderer>().material = OriginalColor;
            notebookNav[currentPage].first.GetComponent<MeshRenderer>().material = SelectedColor;
        }

        //notebookNav[currentPage].first.GetComponent<MeshRenderer>().material = SelectedColor;
    }
    public void MoveHCursorPages(bool right)
    {
        if (notebookNav[currentPage].first != null)
        {
            notebookNav[currentPage].first.GetComponent<MeshRenderer>().material = OriginalColor;
        }
        if (notebookNav[currentPage].second != null)
        {
            notebookNav[currentPage].second.GetComponent<MeshRenderer>().material = OriginalColor;
        }

        if (currentPage != notebookNav.Count - 1 && right)
        {
            if (currentPage % 2 == 0)
            {
                StartCoroutine(PasePage(true));
            }
            currentPage++;
        }
        else if (currentPage != 0 && !right)
        {
            if (currentPage % 2 == 1)
            {
                StartCoroutine(PasePage(false));
            }
            currentPage--;
        }

        if (notebookNav[currentPage].first != null)
        {
            currentTitle = notebookNav[currentPage].first;
            notebookNav[currentPage].first.GetComponent<MeshRenderer>().material = SelectedColor;
            if(isClueSelected)
            {
                Vector3 localPos = clueSelected.GetComponent<RectTransform>().position;
                clueSelected.GetComponent<RectTransform>().SetParent(notebookNav[currentPage].childFirst.transform);
                clueSelected.GetComponent<RectTransform>().localPosition = new Vector3(localPos.x,localPos.y,depthVal);
                clueSelected.GetComponent<RectTransform>().localScale = new Vector3(1.29F,1,1);
                if(currentPage%2 == 0 || currentPage == 1)
                    clueSelected.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
                else
                    clueSelected.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0,180,0);

            }
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

    public void MoveInSeccion(bool moveUp)
    {
        int movment = 1;
        if(!moveUp)
        {
            movment = -1;
        }
        if (currentTitle == null || clueSelected == null)
            return;

        if(currentTitle == notebookNav[currentPage].first)
        {
            GameObject obj = notebookNav[currentPage].childFirst;
            if (obj.transform.childCount > 0)
            {

                foreach (Transform d in obj.transform)
                {
                    if (clueSelected == d.gameObject)
                    {
                        clueSelected.GetComponent<MeshRenderer>().material = OriginalColor;
                        if (d.GetSiblingIndex() < obj.transform.childCount - 1 && moveUp)
                        {
                            clueSelected = obj.transform.GetChild(d.GetSiblingIndex() + movment).gameObject;
                            clueSelected.GetComponent<MeshRenderer>().material = SelectedColor;
                            return;
                        }
                        else if (d.GetSiblingIndex() == obj.transform.childCount - 1 && moveUp)
                        {
                            clueSelected = obj.transform.GetChild(0).gameObject;
                            clueSelected.GetComponent<MeshRenderer>().material = SelectedColor;
                            return;
                        }
                        else if (d.GetSiblingIndex() > 0 && !moveUp)
                        {
                            clueSelected = obj.transform.GetChild(d.GetSiblingIndex() + movment).gameObject;
                            clueSelected.GetComponent<MeshRenderer>().material = SelectedColor;
                            return;
                        }
                        else if (d.GetSiblingIndex() == 0 && !moveUp)
                        {
                            clueSelected = obj.transform.GetChild(obj.transform.childCount - 1).gameObject;
                            clueSelected.GetComponent<MeshRenderer>().material = SelectedColor;
                            return;
                        }
                    }
                }
            }
        }
    }

    public bool EnterParragraf(bool inside)
    {
        if (currentTitle == null)
            return false;
        if (notebookNav[currentPage].first == currentTitle)
        {
            if (notebookNav[currentPage].childFirst == null)
                return false;
            if (inside)
            {
                currentTitle.GetComponent<MeshRenderer>().material = OriginalColor;
                GameObject obj = notebookNav[currentPage].childFirst;
                if (obj.transform.childCount > 0)
                {
                    clueSelected = obj.transform.GetChild(0).gameObject;
                    clueSelected.transform.GetComponent<MeshRenderer>().material = SelectedColor;
                    return true;
                }
            }
            else
            {
                currentTitle.GetComponent<MeshRenderer>().material = SelectedColor;
                clueSelected.transform.GetComponent<MeshRenderer>().material = OriginalColor;
                clueSelected = null;
            }
        }
        else if(currentTitle == notebookNav[currentPage].second)
        {
            if (notebookNav[currentPage].second == null || notebookNav[currentPage].childSecond == null)
                return false;
            if(inside)
            {
                inputField.gameObject.SetActive(true);

                EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
            }
            else
            {
                clueSelected = null;
            }
        }
        return false;
    }

    public void SelectClue(bool selecting)
    {
        isClueSelected = selecting;
        if(!selecting)
        {
            clueSelected.transform.GetComponent<MeshRenderer>().material = OriginalColor;
            clueSelected = null;
        }
        else
        {
            currentTitle.GetComponent<MeshRenderer>().material = SelectedColor;
        }
    }
}
