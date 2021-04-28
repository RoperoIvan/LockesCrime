using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 51)]
public class Dialogue : ScriptableObject
{
    public string[] dialogues;
    public Responses[] responses;

    [System.Serializable]
    public class Responses
    {
        public bool isInvisible;
        public string response;
        public Dialogue dialogueNode;
    }
}
