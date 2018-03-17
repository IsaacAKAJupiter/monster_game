using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    //MAIN QUESTS

    
    //SIDE QUESTS

    
    //MOVE CONSTRUCTING
    public int MoveConstructingLevel = 1;
    public int MoveConstructingExp = 0;

    //CHEMISTRY
    public int ChemistryLevel = 1;
    public int ChemistryExp = 0;

    public void GainMoveConstructingExp(int ExpGained)
    {
        PlayerScript playerScript = GetComponent<PlayerScript>();
        if (this.MoveConstructingLevel >= 50)
        {
            //Level is 50 or more! Not Gaining XP!
            this.MoveConstructingLevel = 50;
            //Making sure the level isn't over 50, resetting to 50!
            this.MoveConstructingExp = 0;
            //Making sure the XP isn't greater than 0, since you can't gain XP if you are level 50.
            return;
        }
        int ExpNeeded = (int)Mathf.Ceil(20 * (this.MoveConstructingLevel + 0.5f));
        print("Exp Needed: " + ExpNeeded);
        print("Exp Gained: " + ExpGained);
        print("Current XP: " + this.MoveConstructingExp);
        print("Current Level: " + this.MoveConstructingLevel);
        this.MoveConstructingExp += ExpGained;
        if (this.MoveConstructingExp > ExpNeeded)
        {
            //Too Much XP!
            int LeftoverExp = this.MoveConstructingExp - ExpNeeded;
            print("LeftoverExp: " + LeftoverExp);
            this.MoveConstructingLevel += 1;
            playerScript.StartOtherEventTexts("You have leveled up in MoveConstructing! Now level " + this.MoveConstructingLevel + "!", playerScript.MoveConstructingSprite);
            //Starting the function again!
            this.MoveConstructingExp = 0;
            //Making sure that I reset XP to try to fix a bug!
            this.GainMoveConstructingExp(LeftoverExp);
            return;

        } else if (this.MoveConstructingExp == ExpNeeded)
        {
            //XP is equal to ExpNeeded!
            this.MoveConstructingExp = 0;
            this.MoveConstructingLevel += 1;
            playerScript.StartOtherEventTexts("You have leveled up in MoveConstructing! Now level " + this.MoveConstructingLevel + "!", playerScript.MoveConstructingSprite);
            return;
        } 
    }

    public void GainChemistryExp(int ExpGained)
    {
        PlayerScript playerScript = GetComponent<PlayerScript>();
        if (this.ChemistryLevel >= 50)
        {
            //Level is 50 or more! Not Gaining XP!
            this.ChemistryLevel = 50;
            //Making sure the level isn't over 50, resetting to 50!
            this.ChemistryExp = 0;
            //Making sure the XP isn't greater than 0, since you can't gain XP if you are level 50.
            return;
        }
        int ExpNeeded = (int)Mathf.Ceil(20 * (this.ChemistryLevel + 0.5f));
        print("Exp Needed: " + ExpNeeded);
        print("Exp Gained: " + ExpGained);
        print("Current XP: " + this.ChemistryExp);
        print("Current Level: " + this.ChemistryLevel);
        this.ChemistryExp += ExpGained;
        if (this.ChemistryExp > ExpNeeded)
        {
            //Too Much XP!
            int LeftoverExp = this.ChemistryExp - ExpNeeded;
            print("LeftoverExp: " + LeftoverExp);
            this.ChemistryLevel += 1;
            playerScript.StartOtherEventTexts("You have leveled up in Chemistry! Now level " + this.MoveConstructingLevel + "!", playerScript.ChemistrySprite);
            //Starting the function again!
            this.ChemistryExp = 0;
            //Making sure that I reset XP to try to fix a bug!
            this.GainChemistryExp(LeftoverExp);
            return;

        }
        else if (this.ChemistryExp == ExpNeeded)
        {
            //XP is equal to ExpNeeded!
            this.ChemistryExp = 0;
            this.ChemistryLevel += 1;
            playerScript.StartOtherEventTexts("You have leveled up in Chemistry! Now level " + this.MoveConstructingLevel + "!", playerScript.ChemistrySprite);
            return;
        }
    }
}
