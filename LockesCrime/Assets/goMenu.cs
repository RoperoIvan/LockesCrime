using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goMenu : MonoBehaviour
{

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void returnToLobby()
    {
        SceneManager.LoadScene(0);
    }
}
