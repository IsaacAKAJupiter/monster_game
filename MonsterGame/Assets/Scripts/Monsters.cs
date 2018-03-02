using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonstersLearnableMoves
{
    public Moves learnableMoves;
    public int moveLevelLearned;
}

[System.Serializable]
public class MonstersDropTable
{
    public Items drop;
    public int percentageChance;
    public int amountDropped;
}

[CreateAssetMenu(fileName = "monster", menuName = "monster")]
public class Monsters : ScriptableObject {

    public new string name;
    public string type;

    public int defence;
    public int attack;
    public int speed;
    public int health;
    public int id;
    public int maxHealth;

    public int baseExpMultiplier;

    public int[] CraftingMaterialsT1 = new int[2];
    public int[] CraftingMaterialsT2 = new int[2];
    public int[] CraftingMaterialsT3 = new int[2];
    public int[] CraftingMaterialsT4 = new int[2];

    public List<MonstersLearnableMoves> MonstersLearnableMoves = new List<MonstersLearnableMoves>();
    public List<MonstersDropTable> MonstersDropTable = new List<MonstersDropTable>();

    public Sprite sprite;
    public GameObject model;

    [TextArea]
    public string description;
}