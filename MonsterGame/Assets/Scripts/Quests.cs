using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "quest", menuName = "quest")]
public class Quests : ScriptableObject {

    //General SO Things
    public new string name;
    public int id;
    [TextArea]
    public string description;

    //Requirements
    public List<Quests> QuestRequirements = new List<Quests>();
    public int[] SkillLevelRequirements = new int[2];

    //Rewards
    public List<Items> ItemAwards = new List<Items>();
    public int[] ExpAwards = new int[2];
    public int[] MaterialAwards = new int[4];

    //Quest stuffs.
    public bool MainQuest;
    public bool IsStarted;
    public bool IsCompleted;
    public int CurrentPart;
    public int MaxParts;
}
