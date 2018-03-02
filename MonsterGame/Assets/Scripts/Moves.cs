using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "move", menuName = "move")]
public class Moves : ScriptableObject {

    public new string name;
    public int id;

    public int damage;
    public int accuracy;

    public int[] materials = new int[4];

    public Sprite sprite;

    public int expGiven;

    public bool InstaKill;

    [TextArea]
    public string description;
}
