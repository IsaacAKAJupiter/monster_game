using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropDownItem
{
    potion, item, boost
}

[CreateAssetMenu(fileName = "item", menuName = "item")]
public class Items : ScriptableObject
{
    public new string name;
    [TextArea]
    public string description;
    public int id;
    public int sellPrice;
    public int buyPrice;

    public Sprite sprite;
    public DropDownItem type;

    //If this is a potion
    public int healAmount;
    public bool healsPoison;
    public bool stopsBleeding;

    //If this is a boost
    public int boostAmount;
    public bool boostAttack;
    public bool boostSpeed;
    public bool boostDefence;
    public bool boostHealth;
    public int boostLength;

    public int expGiven;
    public int[] materials = new int[4];
}

