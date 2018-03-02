using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "achievement", menuName = "achievement")]
public class Achievements : ScriptableObject
{
    public new string name;
    [TextArea]
    public string description;
    public int id;
    public bool completed;
    public Sprite sprite;

    public List<Items> ItemAwards = new List<Items>();
    public int[] ExpAwards = new int[2];
    public int[] MaterialAwards = new int[4];
}

