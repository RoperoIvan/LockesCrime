/* 
 * author : jiankaiwang
 * description : The script provides you with basic operations of first personal control.
 * platform : Unity
 * date : 2017/12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public static CharacterController characterController;
    public static GameObject Player;

    public float speed = 10.0f;
    public bool hasInteraction = false;
    private float translation;
    private float straffe;

    public enum STATE
    {
        NOTEBOOK_OPEN,
        INSIDE_TITLE,
        CLUE_SELECTED,
        EXAMINING_CLUE,
        NONE
    }

    private STATE state;
    //notebook variables
    private bool isNotebookOpen;
    private bool isInsideTitle;
    private bool isClueSelected;
    private void Awake()
    {
        if (characterController == null)
            characterController = this;
    }
    // Use this for initialization
    void Start()
    {
        if(Player == null)
        {
            Player = gameObject;
        }

        isNotebookOpen = false;
        isInsideTitle = false;
        isClueSelected = false;
        state = STATE.NONE; 
        // turn off the cursor
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        if(!hasInteraction)
        {
            translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            straffe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

            if (state == STATE.NONE)
            {
                transform.Translate(straffe, 0, translation);
            }

            //if (Input.GetKeyDown("escape"))
            //{
            //    // turn on the cursor
            //    Cursor.lockState = CursorLockMode.None;
            //}
            //notebook controllers
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                StartCoroutine(NotebookManager.notebookM.TakeNotebook());
                if (state == STATE.NOTEBOOK_OPEN)
                    state = STATE.NONE;
                else
                    state = STATE.NOTEBOOK_OPEN;
            }
            if (state == STATE.NOTEBOOK_OPEN || state == STATE.CLUE_SELECTED)
            {
                NotebookMovment();
            }
            else if (state == STATE.INSIDE_TITLE)
            {
                TitleMovment();
            }
        }
        // Input.GetAxis() is used to get the user's input
        // You can furthor set it on Unity. (Edit, Project Settings, Input)

    }

    private void NotebookMovment()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NotebookManager.notebookM.MoveHCursorPages(false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NotebookManager.notebookM.MoveHCursorPages(true);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NotebookManager.notebookM.MoveVCursorPages(false);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NotebookManager.notebookM.MoveVCursorPages(true);
        }
        else if(Input.GetKeyDown(KeyCode.Space) && state == STATE.NOTEBOOK_OPEN)
        {
            isInsideTitle = NotebookManager.notebookM.EnterParragraf(true);
            state = STATE.INSIDE_TITLE;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && state == STATE.CLUE_SELECTED)
        {
            state = STATE.NOTEBOOK_OPEN;
            NotebookManager.notebookM.SelectClue(false);
        }
    }

    private void TitleMovment()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NotebookManager.notebookM.MoveInSeccion(false);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NotebookManager.notebookM.MoveInSeccion(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            state = STATE.NOTEBOOK_OPEN;
            NotebookManager.notebookM.EnterParragraf(false);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            state = STATE.CLUE_SELECTED;
            NotebookManager.notebookM.SelectClue(true);
        }

    }
}
