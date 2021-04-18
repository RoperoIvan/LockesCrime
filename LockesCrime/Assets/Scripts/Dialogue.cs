using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 51)]
public class Dialogue : ScriptableObject
{
    public string[] dialogues;
    public Responses[] responses;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    public class Responses
    {
        public string response;
        public Dialogue dialogueNode;
    }
}
