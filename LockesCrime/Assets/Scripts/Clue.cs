using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    public string clueName;
    public string description;
    public UnBlockedResponse[] unblockedResponses;
    [System.Serializable]
    public struct UnBlockedResponse
    {
        public string character;
        public int idNode;
        public int idResponse;
    }
}
