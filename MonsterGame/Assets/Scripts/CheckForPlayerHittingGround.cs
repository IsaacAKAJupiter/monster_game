using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckForPlayerHittingGround : MonoBehaviour {

    private CharacterController charController;
    private CurrentMonsterOnPlayer currentMonsterOnPlayer;
    private PlayerStats playerStats;
    private PlayerScript playerScript;
    public TypeOfMonsters typeOfMonsters;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        playerStats = this.transform.parent.GetComponent<PlayerStats>();
        playerScript = this.transform.parent.GetComponent<PlayerScript>();
        currentMonsterOnPlayer = this.transform.parent.GetComponent<CurrentMonsterOnPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ExpGain")
        {
            playerStats.GainMoveConstructingExp(200);
        }

        if (other.gameObject.tag == "ButtonTrigger")
        {
            print("Started Button Trigger.");
            StaticClasses.IsInteractable = true;
            StaticClasses.WhatInteractableItem = other.gameObject.transform.GetChild(0).tag;
            StaticClasses.WhatInteractableItemGO = other.gameObject;
            playerScript.InteractableUI.SetActive(true);
            playerScript.InteractableUI.transform.GetChild(1).GetComponent<Text>().text = "Press " + StaticClasses.InteractableButton + " to interact.";
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ExpGain")
        {
            print("Stopped Gaining Exp.");
        }

        if (other.gameObject.tag == "ButtonTrigger")
        {
            print("Stopped Button Trigger");
            StaticClasses.IsInteractable = false;
            StaticClasses.WhatInteractableItem = null;
            StaticClasses.WhatInteractableItemGO = null;
            playerScript.InteractableUI.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.transform.parent) {
            if (collision.gameObject.transform.parent.tag == "Battle")
            {
                if (StaticClasses.CharControllerVelocity != false)
                {
                    if (StaticClasses.IsInBattle == false && currentMonsterOnPlayer.health > 0)
                    {
                        if (currentMonsterOnPlayer.monsterMoves[0] == null && currentMonsterOnPlayer.monsterMoves[1] == null) {
                            print("Your monster doesn't have any moves!");
                        } else
                        {
                            float randomNumber = Random.Range(0f, 100f);
                            if (randomNumber < 0.5f)
                            {
                                BattleScript battleScript = transform.parent.GetComponent<BattleScript>();
                                string WhatTypeOfMonsters = collision.gameObject.tag;
                                List<int> TypeOfMonstersList = typeOfMonsters.FindType(WhatTypeOfMonsters);
                                int MinMonsterLevel = Mathf.Clamp(currentMonsterOnPlayer.level - 2, 1, int.MaxValue);
                                int MaxMonsterLevel = Mathf.Clamp(currentMonsterOnPlayer.level + 2, 1, int.MaxValue);
                                print("Min Monster Level = " + MinMonsterLevel + " | Max Monster Level = " + MaxMonsterLevel);
                                int RandomNumberLevel = Random.Range(MinMonsterLevel, MaxMonsterLevel + 1);
                                print("Monster Level = " + RandomNumberLevel);
                                int RandomNumber = Random.Range(1, 101);
                                if (RandomNumber < StaticClasses.BossChance)
                                {
                                    battleScript.Battle(TypeOfMonstersList[Random.Range(0, TypeOfMonstersList.Count - 1)], currentMonsterOnPlayer.level + 10, true);
                                } else
                                {
                                    battleScript.Battle(TypeOfMonstersList[Random.Range(0, TypeOfMonstersList.Count - 1)], RandomNumberLevel, false);
                                }
                                StaticClasses.IsInBattle = true;
                            }
                        }
                    } else
                    {
                        //Already in battle or monster is at no health.
                    }
                }
            }
        }
    }
}
