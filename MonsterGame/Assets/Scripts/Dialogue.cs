using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueAndCheckIfPlayer
{
    public bool IsPlayer;
    public Sprite sprite;
    [TextArea(3, 10)]
    public string sentence;
}

[System.Serializable]
public class Dialogue
{

    public string name;
    public List<DialogueAndCheckIfPlayer> sentences = new List<DialogueAndCheckIfPlayer>();
}
