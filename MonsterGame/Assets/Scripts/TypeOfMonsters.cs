using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeOfMonsters : MonoBehaviour {

    public List<int> Grass1 = new List<int>();
    public List<int> Grass2 = new List<int>();

    public List<int> FindType(string Type)
    {
        List<int> newList = (List<int>)this.GetType().GetField(Type).GetValue(this);
        return newList;
    }
}
