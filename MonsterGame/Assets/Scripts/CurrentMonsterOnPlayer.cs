using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentMonsterOnPlayer : MonoBehaviour {

    public new string name = "TestMonster";
    [TextArea]
    public string description = "This is a test monster.";
    public string type = "Grass";
    public int defence = 1;
    public int attack = 1;
    public int speed = 1;
    public int health = 10;
    public int maxHealth = 10;
    public int id = 1;
    public int exp = 0;
    public int level;
    public Sprite sprite;
    public Moves[] monsterMoves = new Moves[2];

    public int MaxSpace = 12;
    public int Monsto = 0;

    public int[] materials = new int[4];
    public List<Moves> moveCards = new List<Moves>();
    public List<Items> itemsPotions = new List<Items>();
    public List<Items> itemsBoosts = new List<Items>();
    public List<Items> items = new List<Items>();

    public void UpdateAllStats(string newName, string newDescription, string newType, int newDefence, int newAttack, int newSpeed, int newHealth, int newMaxHealth, int newId, int newLevel, Sprite newSprite)
    {
        this.name = newName;
        this.description = newDescription;
        this.type = newType;
        this.defence = newDefence;
        this.attack = newAttack;
        this.speed = newSpeed;
        this.health = newHealth;
        this.maxHealth = newMaxHealth;
        this.id = newId;
        this.level = newLevel;
        this.sprite = newSprite;
    }

    public void UpdateHealth(int newHealth)
    {
        this.health = newHealth;
    }

    public void UpdateStatsUponLevelUp(int newDefence, int newAttack, int newSpeed, int newHealth, int newMaxHealth)
    {
        this.defence = newDefence;
        this.attack = newAttack;
        this.speed = newSpeed;
        this.health = newHealth;
        this.maxHealth = newMaxHealth;
    }

    public void AddMoveToMonster(Moves NewMove, int NewMoveID, int ReplacingMove)
    {
        if (monsterMoves[ReplacingMove])
        {
            Moves MoveInInventory = this.moveCards.Find(moveCards => moveCards.id == NewMoveID);
            this.moveCards.Remove(MoveInInventory);
            this.monsterMoves.SetValue(NewMove, ReplacingMove);
        }
        else
        {
            Moves MoveInInventory = this.moveCards.Find(moveCards => moveCards.id == NewMoveID);
            this.moveCards.Remove(MoveInInventory);
            this.monsterMoves.SetValue(NewMove, ReplacingMove);
        }
    }

    public void GainExp(int ExpGained)
    {
        if (this.level >= 50)
        {
            //Level is 50 or more! Not Gaining XP!
            this.level = 50;
            //Making sure the level isn't over 50, resetting to 50!
            this.exp = 0;
            //Making sure the XP isn't greater than 0, since you can't gain XP if you are level 50.
            return;
        }
        int ExpNeeded = (int)Mathf.Ceil(20 * (this.level + 0.5f));
        print("Exp Needed: " + ExpNeeded);
        print("Exp Gained: " + ExpGained);
        print("Current XP: " + this.exp);
        print("Current Level: " + this.level);
        this.exp += ExpGained;
        if (this.exp > ExpNeeded)
        {
            //Too Much XP!
            int LeftoverExp = this.exp - ExpNeeded;
            print("LeftoverExp: " + LeftoverExp);
            this.level += 1;
            this.defence += 1;
            this.speed += 1;
            this.attack += 1;
            this.health += 2;
            this.maxHealth += 10;
            //Starting the function again!
            this.exp = 0;
            //Making sure that I reset XP to try to fix a bug!
            this.GainExp(LeftoverExp);
            return;

        }
        else if (this.exp == ExpNeeded)
        {
            //XP is equal to ExpNeeded!
            this.exp = 0;
            this.level += 1;
            this.defence += 1;
            this.speed += 1;
            this.attack += 1;
            this.health += 2;
            this.maxHealth += 10;
            return;
        }

    }
}
