using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CurrentActiveBattleBoosts
{
    public Items boost;
    public int length;
    public float amount;
    public float AttackBoost;
    public float DefenceBoost;
    public float SpeedBoost;
    public float HealthBoost;

    public CurrentActiveBattleBoosts(Items boostItem, int lengthInt, float amountFloat, float AttackBoostInt, float DefenceBoostInt, float SpeedBoostInt, float HealthBoostInt)
    {
        boost = boostItem;
        length = lengthInt;
        amount = amountFloat;
        AttackBoost = AttackBoostInt;
        DefenceBoost = DefenceBoostInt;
        SpeedBoost = SpeedBoostInt;
        HealthBoost = HealthBoostInt;
    }
}

public class BattleScript : MonoBehaviour {

    CurrentMonsterOnPlayer currentMonsterScript;
    public GameObject Battle_UI;
    public GameObject BattleMenuBottom;
    public GameObject Health;
    public GameObject Name;
    public GameObject MaxHealth;
    public GameObject Slider;

    public Text BattleAttackMove1Text;
    public Text BattleAttackMove2Text;

    public GameObject PotionChoicesPotions;
    public GameObject BoostChoicesBoosts;

    public GameObject ENEMY_Battle_UI;
    public GameObject ENEMY_Health;
    public GameObject ENEMY_Name;
    public GameObject ENEMY_MaxHealth;
    public GameObject ENEMY_Slider;

    private Text HealthText;
    private Text MaxHealthText;
    private Text NameText;
    private Slider SliderComponent;

    public List<CurrentActiveBattleBoosts> currentActiveBattleBoosts = new List<CurrentActiveBattleBoosts>();

    private Text ENEMY_HealthText;
    private Text ENEMY_MaxHealthText;
    private Text ENEMY_NameText;
    private Slider ENEMY_SliderComponent;

    private string ENEMY_Name_MONSTER;
    private int ENEMY_Attack_MONSTER;
    private int ENEMY_Health_MONSTER;
    private int ENEMY_MaxHealth_MONSTER;
    private int ENEMY_Defence_MONSTER;
    private int ENEMY_Speed_MONSTER;
    private int ENEMY_Level_MONSTER;
    private int ENEMY_ID_MONSTER;
    private string ENEMY_Type_MONSTER;
    private Sprite ENEMY_Sprite_MONSTER;
    private Monsters ENEMY_ScriptableObject_MONSTER;
    private Moves[] ENEMY_Moves_MONSTER = new Moves[2];

    private PlayerScript playerScript;

    public List<Monsters> monsterList = new List<Monsters>();

    private int BattleAmountOfTurns;

    private bool BeginningHealthLerp = false;
    private bool ENEMY_BeginningHealthLerp = false;

    private float t = 0f;
    private float ENEMY_t = 0f;

    private bool IsBeginningHealthLerpsOver = false;
    private bool IsTurnToBattle = false;

    // Use this for initialization
    void Start () {
        currentMonsterScript = GetComponent<CurrentMonsterOnPlayer>();
        HealthText = Health.GetComponent<Text>();
        MaxHealthText = MaxHealth.GetComponent<Text>();
        NameText = Name.GetComponent<Text>();
        SliderComponent = Slider.GetComponent<Slider>();

        ENEMY_HealthText = ENEMY_Health.GetComponent<Text>();
        ENEMY_MaxHealthText = ENEMY_MaxHealth.GetComponent<Text>();
        ENEMY_NameText = ENEMY_Name.GetComponent<Text>();
        ENEMY_SliderComponent = ENEMY_Slider.GetComponent<Slider>();

        playerScript = GetComponent<PlayerScript>();

        //MonsterScriptableObjectTable = MonstersListGameObject.GetComponent<MonsterTableForMonsterGameObject>();

        Battle_UI.SetActive(false);
        ENEMY_Battle_UI.SetActive(false);

        
	}

    private void Update()
    {
        if (BeginningHealthLerp == true)
        {
            SliderComponent.value = Mathf.Lerp(SliderComponent.value, currentMonsterScript.health, t);
            t += 0.4f * Time.deltaTime;

            if (t > 1f)
            {
                BeginningHealthLerp = false;
                t = 0f;
            }
        }

        if (ENEMY_BeginningHealthLerp == true)
        {
            ENEMY_SliderComponent.value = Mathf.Lerp(ENEMY_SliderComponent.value, ENEMY_MaxHealth_MONSTER, ENEMY_t);
            ENEMY_t += 0.4f * Time.deltaTime;

            if (ENEMY_t > 1f)
            {
                ENEMY_BeginningHealthLerp = false;
                this.IsTurnToBattle = true;
                IsBeginningHealthLerpsOver = true;
                ENEMY_t = 0f;
            }
        }

        if (IsBeginningHealthLerpsOver == true)
        {
            BattleMenuBottom.SetActive(true);
            BattleMenuBottom.transform.GetChild(1).gameObject.SetActive(true);
            BattleMenuBottom.transform.GetChild(2).gameObject.SetActive(false);
            BattleMenuBottom.transform.GetChild(3).gameObject.SetActive(false);
            BattleMenuBottom.transform.GetChild(4).gameObject.SetActive(false);
            IsBeginningHealthLerpsOver = false;
        }
    }

    private IEnumerator SmallWait(float amount)
    {
        yield return new WaitForSeconds(amount);
    }

    public void UpdateBoostsUISlots()
    {
        for (int i = 0; i < currentMonsterScript.MaxSpace; i++)
        {
            BoostChoicesBoosts.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = null;
        }
        for (int i = 0; i < currentMonsterScript.MaxSpace; i++)
        {
            if (currentMonsterScript.itemsBoosts.Count > i)
            {
                BoostChoicesBoosts.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = currentMonsterScript.itemsBoosts[i].sprite;
            }
        }
        for (int i = 0; i < currentMonsterScript.MaxSpace; i++)
        {
            int copy = i;
            BoostChoicesBoosts.transform.GetChild(copy).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        for (int i = 0; i < currentMonsterScript.MaxSpace; i++)
        {
            if (currentMonsterScript.itemsBoosts.Count > i)
            {
                int copy = i;
                BoostChoicesBoosts.transform.GetChild(copy).gameObject.GetComponent<Button>().onClick.AddListener(delegate { UseBoost(currentMonsterScript.itemsBoosts[copy].id); });
            }
        }
    }

    public void UpdatePotionsUISlots()
    {
        for (int i = 0; i < currentMonsterScript.MaxSpace; i++)
        {
            PotionChoicesPotions.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = null;
        }
        for (int i = 0; i < currentMonsterScript.MaxSpace; i++)
        {
            if (currentMonsterScript.itemsPotions.Count > i)
            {
                PotionChoicesPotions.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = currentMonsterScript.itemsPotions[i].sprite;
            }
        }
        for (int i = 0; i < currentMonsterScript.MaxSpace; i++)
        {
            int copy = i;
            PotionChoicesPotions.transform.GetChild(copy).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        for (int i = 0; i < currentMonsterScript.MaxSpace; i++)
        {
            if (currentMonsterScript.itemsPotions.Count > i)
            {
                int copy = i;
                PotionChoicesPotions.transform.GetChild(copy).gameObject.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(UsePotion(currentMonsterScript.itemsPotions[copy].id)); });
            }
        }
    }

    private void UseBoost(int BoostID)
    {
        if (IsTurnToBattle == false)
        {
            print("You can't use a boost right now, you are mid attacking.");
            return;
        }

        //Set to false to make sure you can't attack again.
        IsTurnToBattle = false;

        print("Using boost: " + playerScript.items[BoostID - 1].name + " with the id: " + playerScript.items[BoostID - 1].id);
        float AttackBoost = 0;
        float DefenceBoost = 0;
        float SpeedBoost = 0;
        float HealthBoost = 0;
        if (playerScript.items[BoostID - 1].boostAttack == true)
        {
            print("This is a boost for attack.");
            AttackBoost += playerScript.items[BoostID - 1].boostAmount;
        }
        if (playerScript.items[BoostID - 1].boostDefence == true)
        {
            print("This is a boost for defence.");
            DefenceBoost += playerScript.items[BoostID - 1].boostAmount;
        }
        if (playerScript.items[BoostID - 1].boostSpeed == true)
        {
            print("This is a boost for speed.");
            SpeedBoost += playerScript.items[BoostID - 1].boostAmount;
        }
        if (playerScript.items[BoostID - 1].boostHealth == true)
        {
            print("This is a boost for health.");
            HealthBoost += playerScript.items[BoostID - 1].boostAmount;
            //HealthText.transform.parent.transform.GetChild(7).GetComponent<Text>().text = this.HealthBoost.ToString();
        }
        //Adding boost struct to a list to try to allow slowly getting worse.
        CurrentActiveBattleBoosts boost = new CurrentActiveBattleBoosts(playerScript.items[BoostID - 1], playerScript.items[BoostID - 1].boostLength, playerScript.items[BoostID - 1].boostAmount, AttackBoost, DefenceBoost, SpeedBoost, HealthBoost);
        currentActiveBattleBoosts.Add(boost);

        //Checking all the boosts before you attack.
        SpeedBoost = 0;
        AttackBoost = 0;
        DefenceBoost = 0;
        HealthBoost = 0;
        if (currentActiveBattleBoosts.Count > 0)
        {
            for (int i = 0; i <= currentActiveBattleBoosts.Count - 1; i++)
            {
                SpeedBoost += currentActiveBattleBoosts[i].SpeedBoost;
                AttackBoost += currentActiveBattleBoosts[i].AttackBoost;
                DefenceBoost += currentActiveBattleBoosts[i].DefenceBoost;
                HealthBoost += currentActiveBattleBoosts[i].HealthBoost;
            }
        }
        else
        {
            print("You have no boosts right now.");
        }
        print("SpeedBoost: " + SpeedBoost);
        print("AttackBoost: " + AttackBoost);
        print("DefenceBoost: " + DefenceBoost);
        print("HealthBoost: " + HealthBoost);
        HealthText.transform.parent.transform.GetChild(7).GetComponent<Text>().text = HealthBoost.ToString();

        //Setting the boost choices UI to invisible and removing the boost from your inventory.
        BoostChoicesBoosts.transform.parent.gameObject.SetActive(false);
        currentMonsterScript.itemsBoosts.Remove(playerScript.items[BoostID - 1]);

        //Starting the enemy attack since you used your turn.
        StartCoroutine(BattleJustEnemy((myReturnValue) => {
            if (myReturnValue == true)
            {
                //Ended battle.
            }
        }));
    }

    private IEnumerator UsePotion(int ItemID)
    {
        if (IsTurnToBattle == false)
        {
            print("You can't use a potion right now, you are mid attacking.");
            yield break;
        }

        //Set to false to say that an attack is going on.
        IsTurnToBattle = false;

        print("Using potion: " + playerScript.items[ItemID - 1].name + " with the id: " + playerScript.items[ItemID - 1].id);
        print(Mathf.Clamp(currentMonsterScript.health + playerScript.items[ItemID - 1].healAmount, 1, currentMonsterScript.maxHealth));
        PotionChoicesPotions.transform.parent.gameObject.SetActive(false);

        if (currentMonsterScript.health == currentMonsterScript.maxHealth)
        {
            print("Already max health, don't need to lerp.");
            currentMonsterScript.itemsPotions.Remove(playerScript.items[ItemID - 1]);
            StartCoroutine(BattleJustEnemy((myReturnValue) => {
                if (myReturnValue == true)
                {
                    //Ended battle.
                }
            }));
            yield break;
        }

        currentMonsterScript.UpdateHealth(Mathf.Clamp(currentMonsterScript.health + playerScript.items[ItemID - 1].healAmount, 1, currentMonsterScript.maxHealth));
        currentMonsterScript.itemsPotions.Remove(playerScript.items[ItemID - 1]);
        HealthText.text = currentMonsterScript.health.ToString();

        for (float i = 0.0f; i <= 1.0f; i += 0.001f)
        {
            if (SliderComponent.value < currentMonsterScript.health + 0.01f && SliderComponent.value > currentMonsterScript.health - 0.01f)
            {
                SliderComponent.value = currentMonsterScript.health;
                break;
            }
            SliderComponent.value = Mathf.Lerp(SliderComponent.value, currentMonsterScript.health, i);
            //print("i: " + i.ToString() + " | SliderValue: " + SliderComponent.value);
            yield return new WaitForSeconds(0.01f);
        }

        print("Done waiting: POTION USING LERP");
        StartCoroutine(BattleJustEnemy((myReturnValue) => {
            if (myReturnValue == true)
            {
                //Ended battle.
            }
        }));
        yield return null;
    }

    public void Battle(int ENEMY_ID, int ENEMY_Level, bool IsBossMonster)
    {
        Battle_UI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        BattleAmountOfTurns = 0;
        ENEMY_Level_MONSTER = ENEMY_Level;

        if (IsBossMonster == true)
        {
            print("THIS MONSTER IS A BOSS MONSTER");
        }

        HealthText.text = currentMonsterScript.health.ToString();
        MaxHealthText.text = currentMonsterScript.maxHealth.ToString();
        NameText.text = currentMonsterScript.name.ToString();
        SliderComponent.maxValue = currentMonsterScript.maxHealth;

        if (currentMonsterScript.monsterMoves[0] != null)
        {
            BattleAttackMove1Text.text = currentMonsterScript.monsterMoves[0].name;
        } else
        {
            BattleAttackMove1Text.text = "No move for slot 1.";
        }

        if (currentMonsterScript.monsterMoves[1] != null)
        {
            BattleAttackMove2Text.text = currentMonsterScript.monsterMoves[1].name;
        }
        else
        {
            BattleAttackMove2Text.text = "No move for slot 1.";
        }

        BeginningHealthLerp = true;

        ENEMY_Battle_UI.SetActive(true);

        ENEMY_ScriptableObject_MONSTER = this.monsterList[ENEMY_ID - 1];
        ENEMY_ID_MONSTER = ENEMY_ID;
        ENEMY_Attack_MONSTER = ENEMY_ScriptableObject_MONSTER.attack * ENEMY_Level;
        ENEMY_Name_MONSTER = ENEMY_ScriptableObject_MONSTER.name;
        ENEMY_Defence_MONSTER = ENEMY_ScriptableObject_MONSTER.defence * ENEMY_Level;
        ENEMY_Speed_MONSTER = ENEMY_ScriptableObject_MONSTER.speed * ENEMY_Level;
        ENEMY_Sprite_MONSTER = ENEMY_ScriptableObject_MONSTER.sprite;
        ENEMY_Health_MONSTER = ENEMY_ScriptableObject_MONSTER.health * ENEMY_Level;
        ENEMY_MaxHealth_MONSTER = ENEMY_ScriptableObject_MONSTER.maxHealth * ENEMY_Level;
        ENEMY_Type_MONSTER = ENEMY_ScriptableObject_MONSTER.type;

        //Make the enemy monster have random moves based on its level.
        int HowManyMovesCanENEMYLearn = 0;
        for (int i = 0; i <= ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves.Count - 1; i++)
        {
            if (ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[i].moveLevelLearned <= ENEMY_Level)
            {
                print(i + " <- i");
                print("i: " + i + " is less than " + ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves.Count);
                HowManyMovesCanENEMYLearn++;
                print("Can learn move: " + ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[i].learnableMoves + " at level: " + ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[i].moveLevelLearned);
            } else
            {
                print("Monster isn't high enough level.");
                break;
            }
            
        }

        //If the monster can only learn 1 move, then it should just learn that move in the first slot and leave the second empty.
        if (HowManyMovesCanENEMYLearn < 2)
        {
            ENEMY_Moves_MONSTER.SetValue(ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[HowManyMovesCanENEMYLearn - 1].learnableMoves, 0);
            print("Enemies first move: " + ENEMY_Moves_MONSTER[0].name);
            if (ENEMY_Moves_MONSTER[1] == null)
            {
                print("second move is null");
            }
        } else
        {
            while (ENEMY_Moves_MONSTER[0] == null || ENEMY_Moves_MONSTER[1] == null)
            {
                int RandomNumber = Random.Range(0, HowManyMovesCanENEMYLearn);
                if (ENEMY_Moves_MONSTER[0] == null && ENEMY_Moves_MONSTER[1] == null)
                {
                    if (ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[RandomNumber].learnableMoves.InstaKill == false)
                    {
                        print("Adding the move to the first slot: " + ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[RandomNumber].learnableMoves.name);
                        ENEMY_Moves_MONSTER.SetValue(ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[RandomNumber].learnableMoves, 0);
                    }
                    else
                    {
                        if (IsBossMonster == true)
                        {
                            print("Adding the move to the first slot: " + ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[RandomNumber].learnableMoves.name);
                            ENEMY_Moves_MONSTER.SetValue(ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[RandomNumber].learnableMoves, 0);
                        }
                        else
                        {
                            print("Non-Boss monsters cannot learn instakill moves.");
                        }
                    }
                } else if (ENEMY_Moves_MONSTER[0] != null && ENEMY_Moves_MONSTER[1] == null)
                {
                    if (ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[RandomNumber].learnableMoves.id != ENEMY_Moves_MONSTER[0].id)
                    {
                        if (ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[RandomNumber].learnableMoves.InstaKill == false)
                        {
                            print("Adding the move to the second slot: " + ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[RandomNumber].learnableMoves.name);
                            ENEMY_Moves_MONSTER.SetValue(ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[RandomNumber].learnableMoves, 1);
                        }
                        else
                        {
                            if (IsBossMonster == true)
                            {
                                print("Adding the move to the second slot: " + ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[RandomNumber].learnableMoves.name);
                                ENEMY_Moves_MONSTER.SetValue(ENEMY_ScriptableObject_MONSTER.MonstersLearnableMoves[RandomNumber].learnableMoves, 1);
                            }
                            else
                            {
                                print("Non-Boss monsters cannot learn instakill moves.");
                            }
                        }
                    } else
                    {
                        print("Move picked is the same as slot 1. Restarting.");
                    }
                } else
                {
                    print("Something broke.");
                }
            }
        }

        ENEMY_HealthText.text = ENEMY_Health_MONSTER.ToString();
        ENEMY_MaxHealthText.text = ENEMY_MaxHealth_MONSTER.ToString();
        ENEMY_NameText.text = ENEMY_Name_MONSTER.ToString();
        ENEMY_SliderComponent.maxValue = ENEMY_MaxHealth_MONSTER;
        ENEMY_BeginningHealthLerp = true;
    }

    public void PreAttack(int MoveNumber)
    {
        StartCoroutine(Attack(MoveNumber));
    }

    private IEnumerator Attack(int MoveNumber)
    {

        //Check if the players move even exists.
        if (currentMonsterScript.monsterMoves[MoveNumber] == null)
        {
            print("The move for slot " + (MoveNumber + 1) + " is null.");
            yield break;
        }

        //Check if IsTurnToBattle is false; if so return.
        if (this.IsTurnToBattle == false)
        {
            print("IsTurnToBattle is false, you are not allowed to attack now.");
            yield break;
        }

        //Making IsTurnToBattle false since you are now attacking. Also setting the moves UI active state to false.
        this.IsTurnToBattle = false;
        BattleAttackMove1Text.transform.parent.transform.parent.gameObject.SetActive(false);

        //Making a variable for dealing with who gets to attack first.
        string WhoIsAttackingFirst = null;

        //Checking all the boosts before you attack.
        float SpeedBoost = 0;
        float AttackBoost = 0;
        float DefenceBoost = 0;
        float HealthBoost = 0;
        if (currentActiveBattleBoosts.Count > 0)
        {
            for (int i = 0; i <= currentActiveBattleBoosts.Count - 1; i++)
            {
                SpeedBoost += currentActiveBattleBoosts[i].SpeedBoost;
                AttackBoost += currentActiveBattleBoosts[i].AttackBoost;
                DefenceBoost += currentActiveBattleBoosts[i].DefenceBoost;
                HealthBoost += currentActiveBattleBoosts[i].HealthBoost;
            }
        }
        else
        {
            print("You have no boosts right now.");
        }
        print("SpeedBoost: " + SpeedBoost);
        print("AttackBoost: " + AttackBoost);
        print("DefenceBoost: " + DefenceBoost);
        print("HealthBoost: " + HealthBoost);
        HealthText.transform.parent.transform.GetChild(7).GetComponent<Text>().text = HealthBoost.ToString();

        //Check speed of the player's and enemies monsters.
        if ((currentMonsterScript.speed + SpeedBoost) > ENEMY_Speed_MONSTER)
        {
            print("Player's speed is > Enemies: PLAYER | " + (currentMonsterScript.speed + SpeedBoost) + " | ENEMY | " + ENEMY_Speed_MONSTER);
            WhoIsAttackingFirst = "Player";
        } else if ((currentMonsterScript.speed + SpeedBoost) < ENEMY_Speed_MONSTER)
        {
            print("Player's speed is < Enemies: PLAYER | " + (currentMonsterScript.speed + SpeedBoost) + " | ENEMY | " + ENEMY_Speed_MONSTER);
            WhoIsAttackingFirst = "Enemy";
        } else if ((currentMonsterScript.speed + SpeedBoost) == ENEMY_Speed_MONSTER)
        {
            print("Player's speed is = Enemies: PLAYER | " + (currentMonsterScript.speed + SpeedBoost) + " | ENEMY | " + ENEMY_Speed_MONSTER);
            if (Random.Range(0, 2) == 0)
            {
                print("Random number chosen, player attacks first.");
                WhoIsAttackingFirst = "Player";
            } else
            {
                print("Random number chosen, enemy attacks first.");
                WhoIsAttackingFirst = "Enemy";
            }
        }

        if (WhoIsAttackingFirst == "Player")
        {
            //Dealing with the player attacking.
            if (Random.Range(1, 101) < currentMonsterScript.monsterMoves[MoveNumber].accuracy)
            {
                int PlayerDamageToEnemy = Mathf.Clamp(Mathf.CeilToInt((float)((currentMonsterScript.attack + AttackBoost) * currentMonsterScript.monsterMoves[MoveNumber].damage) / ENEMY_Defence_MONSTER), 1, int.MaxValue);
                print("PLAYER SHOULD DO THIS DAMAGE: " + PlayerDamageToEnemy);
                ENEMY_Health_MONSTER -= PlayerDamageToEnemy;
                if (ENEMY_Health_MONSTER < 0)
                {
                    ENEMY_Health_MONSTER = 0;
                }
                ENEMY_HealthText.text = ENEMY_Health_MONSTER.ToString();

                for (float i = 0.0f; i <= 1.0f; i += 0.001f)
                {
                    if (ENEMY_SliderComponent.value < ENEMY_Health_MONSTER + 0.01f && ENEMY_SliderComponent.value > ENEMY_Health_MONSTER - 0.01f)
                    {
                        ENEMY_SliderComponent.value = ENEMY_Health_MONSTER;
                        break;
                    }
                    ENEMY_SliderComponent.value = Mathf.Lerp(ENEMY_SliderComponent.value, ENEMY_Health_MONSTER, i);
                    //print("i: " + i.ToString() + " | ENEMY_SliderValue: " + ENEMY_SliderComponent.value);
                    yield return new WaitForSeconds(0.01f);
                }
                print("Done Waiting: PlayerAttackingFirst");

                if (ENEMY_Health_MONSTER <= 0)
                {
                    BattleAmountOfTurns++;
                    currentMonsterScript.GainExp(Mathf.CeilToInt((float)(ENEMY_Level_MONSTER * ENEMY_ScriptableObject_MONSTER.baseExpMultiplier)));
                    this.EndBattle(true);
                    this.EnemyMonsterDrops(ENEMY_ScriptableObject_MONSTER);
                    yield break;
                }
            } else
            {
                print("Attack Missed!");
            }

            yield return new WaitForSeconds(1.0f);

            //Dealing with the enemy attacking.
            //TODO: An actual AI system where the move is picked based on damage (game setting?)
            int EnemyRandomMoveNumber;
            if (ENEMY_Moves_MONSTER[1] != null)
            {
                if (StaticClasses.AIDifficulty == "Easy")
                {
                    print("Difficulty is Easy, choosing random move.");
                    EnemyRandomMoveNumber = Random.Range(0, 2);
                }
                else
                {
                    if (ENEMY_Moves_MONSTER[0].damage > ENEMY_Moves_MONSTER[1].damage)
                    {
                        EnemyRandomMoveNumber = 0;
                    }
                    else
                    {
                        EnemyRandomMoveNumber = 1;
                    }
                }
                print("Has 2 moves.");
            }
            else
            {
                EnemyRandomMoveNumber = 0;
                print("Only has 1 move.");
            }
            print("The random enemy move number is: " + EnemyRandomMoveNumber);
            if (Random.Range(1, 101) < ENEMY_Moves_MONSTER[EnemyRandomMoveNumber].accuracy)
            {
                int EnemyDamageToPlayer = Mathf.Clamp(Mathf.CeilToInt((float)(ENEMY_Attack_MONSTER * ENEMY_Moves_MONSTER[EnemyRandomMoveNumber].damage) / (currentMonsterScript.defence + DefenceBoost)), 1, int.MaxValue);
                print("ENEMY SHOULD DO THIS DAMAGE: " + EnemyDamageToPlayer + " | MOVE USED: " + ENEMY_Moves_MONSTER[EnemyRandomMoveNumber].name);

                //Before updating the health, let's check if the player has any health boosts.
                int LeftoverDamage = 0;
                if (HealthBoost > 0)
                {
                    print("Player has a health boost of: " + HealthBoost.ToString());
                    if (EnemyDamageToPlayer > HealthBoost)
                    {
                        LeftoverDamage = EnemyDamageToPlayer - (int)HealthBoost;
                        print("The leftover damage after the enemy has hit the health boost is: " + LeftoverDamage.ToString());
                        HealthBoost = 0;
                        for (int i = 0; i <= currentActiveBattleBoosts.Count - 1; i++)
                        {
                            currentActiveBattleBoosts[i].HealthBoost = 0;
                        }
                        HealthText.transform.parent.transform.GetChild(7).GetComponent<Text>().text = HealthBoost.ToString();
                    }
                    else
                    {
                        HealthBoost -= EnemyDamageToPlayer;
                        HealthText.transform.parent.transform.GetChild(7).GetComponent<Text>().text = HealthBoost.ToString();
                    }
                }
                else
                {
                    print("HealthBoost is null.");
                }

                if (LeftoverDamage > 0)
                {
                    currentMonsterScript.UpdateHealth(Mathf.Clamp(currentMonsterScript.health - LeftoverDamage, 0, currentMonsterScript.maxHealth));
                }
                else if (LeftoverDamage == 0 && HealthBoost == 0)
                {
                    print("LeftoverDamage is 0, so is HealthBoost. Doing normal damage.");
                    currentMonsterScript.UpdateHealth(Mathf.Clamp(currentMonsterScript.health - EnemyDamageToPlayer, 0, currentMonsterScript.maxHealth));
                }
                HealthText.text = currentMonsterScript.health.ToString();

                for (float i = 0.0f; i <= 1.0f; i += 0.001f)
                {
                    if (SliderComponent.value < currentMonsterScript.health + 0.01f && SliderComponent.value > currentMonsterScript.health - 0.01f)
                    {
                        SliderComponent.value = currentMonsterScript.health;
                        break;
                    }
                    SliderComponent.value = Mathf.Lerp(SliderComponent.value, currentMonsterScript.health, i);
                    //print("i: " + i.ToString() + " | SliderValue: " + SliderComponent.value);
                    yield return new WaitForSeconds(0.01f);
                }
                print("Done Waiting: EnemyAttackingSecond");

                if (currentMonsterScript.health <= 0)
                {
                    currentMonsterScript.UpdateHealth(0);
                    BattleAmountOfTurns++;
                    this.EndBattle(false);
                    yield break;
                }
            } else
            {
                print("Attack Missed!");
            }

        } else if (WhoIsAttackingFirst == "Enemy")
        {
            //Dealing with the enemy attacking.
            //TODO: An actual AI system where the move is picked based on damage (game setting?)
            int EnemyRandomMoveNumber;
            if (ENEMY_Moves_MONSTER[1] != null)
            {
                if (StaticClasses.AIDifficulty == "Easy")
                {
                    print("Difficulty is Easy, choosing random move.");
                    EnemyRandomMoveNumber = Random.Range(0, 2);
                }
                else
                {
                    if (ENEMY_Moves_MONSTER[0].damage > ENEMY_Moves_MONSTER[1].damage)
                    {
                        EnemyRandomMoveNumber = 0;
                    }
                    else
                    {
                        EnemyRandomMoveNumber = 1;
                    }
                }
                print("Has 2 moves.");
            }
            else
            {
                EnemyRandomMoveNumber = 0;
                print("Only has 1 move.");
            }
            print("The random enemy move number is: " + EnemyRandomMoveNumber);
            if (Random.Range(1, 101) < ENEMY_Moves_MONSTER[EnemyRandomMoveNumber].accuracy)
            {
                int EnemyDamageToPlayer = Mathf.Clamp((int)Mathf.Ceil((float)(ENEMY_Attack_MONSTER * ENEMY_Moves_MONSTER[EnemyRandomMoveNumber].damage) / (currentMonsterScript.defence + DefenceBoost)), 1, int.MaxValue);
                print("ENEMY SHOULD DO THIS DAMAGE: " + EnemyDamageToPlayer + " | MOVE USED: " + ENEMY_Moves_MONSTER[EnemyRandomMoveNumber].name);

                //Before updating the health, let's check if the player has any health boosts.
                int LeftoverDamage = 0;
                if (HealthBoost > 0)
                {
                    print("Player has a health boost of: " + HealthBoost.ToString());
                    if (EnemyDamageToPlayer > HealthBoost)
                    {
                        LeftoverDamage = EnemyDamageToPlayer - (int)HealthBoost;
                        print("The leftover damage after the enemy has hit the health boost is: " + LeftoverDamage.ToString());
                        HealthBoost = 0;
                        for (int i = 0; i <= currentActiveBattleBoosts.Count - 1; i++)
                        {
                            currentActiveBattleBoosts[i].HealthBoost = 0;
                        }
                        HealthText.transform.parent.transform.GetChild(7).GetComponent<Text>().text = HealthBoost.ToString();
                    }
                    else
                    {
                        HealthBoost -= EnemyDamageToPlayer;
                        HealthText.transform.parent.transform.GetChild(7).GetComponent<Text>().text = HealthBoost.ToString();
                    }
                }
                else
                {
                    print("HealthBoost is null.");
                }

                if (LeftoverDamage > 0)
                {
                    currentMonsterScript.UpdateHealth(Mathf.Clamp(currentMonsterScript.health - LeftoverDamage, 0, currentMonsterScript.maxHealth));
                }
                else if (LeftoverDamage == 0 && HealthBoost == 0)
                {
                    print("LeftoverDamage is 0, so is HealthBoost. Doing normal damage.");
                    currentMonsterScript.UpdateHealth(Mathf.Clamp(currentMonsterScript.health - EnemyDamageToPlayer, 0, currentMonsterScript.maxHealth));
                }
                HealthText.text = currentMonsterScript.health.ToString();

                for (float i = 0.0f; i <= 1.0f; i += 0.001f)
                {
                    if (SliderComponent.value < currentMonsterScript.health + 0.01f && SliderComponent.value > currentMonsterScript.health - 0.01f)
                    {
                        SliderComponent.value = currentMonsterScript.health;
                        break;
                    }
                    SliderComponent.value = Mathf.Lerp(SliderComponent.value, currentMonsterScript.health, i);
                    //print("i: " + i.ToString() + " | SliderValue: " + SliderComponent.value);
                    yield return new WaitForSeconds(0.01f);
                }
                print("Done Waiting: EnemyAttackingFirst");

                if (currentMonsterScript.health <= 0)
                {
                    currentMonsterScript.UpdateHealth(0);
                    BattleAmountOfTurns++;
                    this.EndBattle(false);
                    yield break;
                }
            } else
            {
                print("Attack Missed!");
            }

            yield return new WaitForSeconds(1.0f);

            //Dealing with the player attacking.
            if (Random.Range(1, 101) < currentMonsterScript.monsterMoves[MoveNumber].accuracy)
            {
                int PlayerDamageToEnemy = Mathf.Clamp((int)Mathf.Ceil((float)((currentMonsterScript.attack + AttackBoost) * currentMonsterScript.monsterMoves[MoveNumber].damage) / ENEMY_Defence_MONSTER), 1, int.MaxValue);
                print("PLAYER SHOULD DO THIS DAMAGE: " + PlayerDamageToEnemy);
                ENEMY_Health_MONSTER -= PlayerDamageToEnemy;
                if (ENEMY_Health_MONSTER < 0)
                {
                    ENEMY_Health_MONSTER = 0;
                }
                ENEMY_HealthText.text = ENEMY_Health_MONSTER.ToString();

                for (float i = 0.0f; i <= 1.0f; i += 0.001f)
                {
                    if (ENEMY_SliderComponent.value < ENEMY_Health_MONSTER + 0.01f && ENEMY_SliderComponent.value > ENEMY_Health_MONSTER - 0.01f)
                    {
                        ENEMY_SliderComponent.value = ENEMY_Health_MONSTER;
                        break;
                    }
                    ENEMY_SliderComponent.value = Mathf.Lerp(ENEMY_SliderComponent.value, ENEMY_Health_MONSTER, i);
                    //print("i: " + i.ToString() + " | ENEMY_SliderValue: " + ENEMY_SliderComponent.value);
                    yield return new WaitForSeconds(0.01f);
                }
                print("Done Waiting: PlayerAttackingSecond");

                if (ENEMY_Health_MONSTER <= 0)
                {
                    BattleAmountOfTurns++;
                    currentMonsterScript.GainExp(Mathf.CeilToInt((float)(ENEMY_Level_MONSTER * ENEMY_ScriptableObject_MONSTER.baseExpMultiplier)));
                    this.EndBattle(true);
                    this.EnemyMonsterDrops(ENEMY_ScriptableObject_MONSTER);
                    yield break;
                }
            } else
            {
                print("Attack Missed!");
            }
        }

        //Recalculating all the values for the boosts.
        SpeedBoost = 0;
        AttackBoost = 0;
        DefenceBoost = 0;
        HealthBoost = 0;
        List<int> BoostRemovalInts = new List<int>();
        if (currentActiveBattleBoosts.Count > 0)
        {
            for (int i = 0; i <= currentActiveBattleBoosts.Count - 1; i++)
            {
                currentActiveBattleBoosts[i].SpeedBoost -= (currentActiveBattleBoosts[i].amount / currentActiveBattleBoosts[i].length);
                if (currentActiveBattleBoosts[i].SpeedBoost < 0)
                {
                    currentActiveBattleBoosts[i].SpeedBoost = 0;
                }
                currentActiveBattleBoosts[i].AttackBoost -= (currentActiveBattleBoosts[i].amount / currentActiveBattleBoosts[i].length);
                if (currentActiveBattleBoosts[i].AttackBoost < 0)
                {
                    currentActiveBattleBoosts[i].AttackBoost = 0;
                }
                currentActiveBattleBoosts[i].DefenceBoost -= (currentActiveBattleBoosts[i].amount / currentActiveBattleBoosts[i].length);
                if (currentActiveBattleBoosts[i].DefenceBoost < 0)
                {
                    currentActiveBattleBoosts[i].DefenceBoost = 0;
                }
                currentActiveBattleBoosts[i].HealthBoost -= (currentActiveBattleBoosts[i].amount / currentActiveBattleBoosts[i].length);
                if (currentActiveBattleBoosts[i].HealthBoost < 0)
                {
                    currentActiveBattleBoosts[i].HealthBoost = 0;
                }

                if (currentActiveBattleBoosts[i].SpeedBoost <= 0 && currentActiveBattleBoosts[i].AttackBoost <= 0 && currentActiveBattleBoosts[i].DefenceBoost <= 0 && currentActiveBattleBoosts[i].HealthBoost <= 0)
                {
                    print("Everything is below or equal to 0, boost being removed.");
                    BoostRemovalInts.Add(i);
                }
                else
                {
                    print("Something is above 0, not removing the current boost.");
                    SpeedBoost += currentActiveBattleBoosts[i].SpeedBoost;
                    AttackBoost += currentActiveBattleBoosts[i].AttackBoost;
                    DefenceBoost += currentActiveBattleBoosts[i].DefenceBoost;
                    HealthBoost += currentActiveBattleBoosts[i].HealthBoost;
                }
            }

            foreach (int numbers in BoostRemovalInts)
            {
                currentActiveBattleBoosts.RemoveAt(numbers);
            }

            BoostRemovalInts.Clear();
        }
        else
        {
            print("You have no boosts right now.");
        }
        print("SpeedBoost: " + SpeedBoost);
        print("AttackBoost: " + AttackBoost);
        print("DefenceBoost: " + DefenceBoost);
        print("HealthBoost: " + HealthBoost);
        HealthText.transform.parent.transform.GetChild(7).GetComponent<Text>().text = HealthBoost.ToString();

        //This is when all the attacking is done and neither of the monsters died, so I have to put the UI back on the screen.
        BattleMenuBottom.SetActive(true);
        BattleMenuBottom.transform.GetChild(1).gameObject.SetActive(true);
        BattleMenuBottom.transform.GetChild(2).gameObject.SetActive(false);
        BattleMenuBottom.transform.GetChild(3).gameObject.SetActive(false);
        BattleMenuBottom.transform.GetChild(4).gameObject.SetActive(false);
        IsTurnToBattle = true;
        BattleAmountOfTurns++;
        yield return null;
    }

    private IEnumerator BattleJustEnemy(System.Action<bool> callback)
    {
        //Dealing with the enemy attacking.
        //TODO: An actual AI system where the move is picked based on damage (game setting?)
        yield return new WaitForSeconds(1.0f);
        int EnemyRandomMoveNumber;
        if (ENEMY_Moves_MONSTER[1] != null)
        {
            if (StaticClasses.AIDifficulty == "Easy")
            {
                print("Difficulty is Easy, choosing random move.");
                EnemyRandomMoveNumber = Random.Range(0, 2);
            } else
            {
                if (ENEMY_Moves_MONSTER[0].damage > ENEMY_Moves_MONSTER[1].damage)
                {
                    EnemyRandomMoveNumber = 0;
                } else
                {
                    EnemyRandomMoveNumber = 1;
                }
            }
            print("Has 2 moves.");
        } else
        {
            EnemyRandomMoveNumber = 0;
            print("Only has 1 move.");
        }
        
        print("The random enemy move number is: " + EnemyRandomMoveNumber);

        //Checking all the boosts before you attack.
        float SpeedBoost = 0;
        float AttackBoost = 0;
        float DefenceBoost = 0;
        float HealthBoost = 0;
        if (currentActiveBattleBoosts.Count > 0)
        {
            print("ACTUALLY HAS 1 THING!");
            for (int i = 0; i <= currentActiveBattleBoosts.Count - 1; i++)
            {
                print("Current Boost is has SpeedBoost of: " + currentActiveBattleBoosts[i].SpeedBoost.ToString() + "; an Attack Boost of: " + currentActiveBattleBoosts[i].AttackBoost.ToString() + "; a Defence Boost of: " + currentActiveBattleBoosts[i].DefenceBoost.ToString() + "; and a Speed Boost of: " + currentActiveBattleBoosts[i].SpeedBoost.ToString());
                SpeedBoost += currentActiveBattleBoosts[i].SpeedBoost;
                AttackBoost += currentActiveBattleBoosts[i].AttackBoost;
                DefenceBoost += currentActiveBattleBoosts[i].DefenceBoost;
                HealthBoost += currentActiveBattleBoosts[i].HealthBoost;
            }
        }
        else
        {
            print("You have no boosts right now.");
        }
        print("SpeedBoost: " + SpeedBoost);
        print("AttackBoost: " + AttackBoost);
        print("DefenceBoost: " + DefenceBoost);
        print("HealthBoost: " + HealthBoost);
        HealthText.transform.parent.transform.GetChild(7).GetComponent<Text>().text = HealthBoost.ToString();

        if (Random.Range(1, 101) < ENEMY_Moves_MONSTER[EnemyRandomMoveNumber].accuracy)
        {
            int EnemyDamageToPlayer = Mathf.Clamp((int)Mathf.Ceil((float)(ENEMY_Attack_MONSTER * ENEMY_Moves_MONSTER[EnemyRandomMoveNumber].damage) / (currentMonsterScript.defence + DefenceBoost)), 1, int.MaxValue);
            print("ENEMY SHOULD DO THIS DAMAGE: " + EnemyDamageToPlayer + " | MOVE USED: " + ENEMY_Moves_MONSTER[EnemyRandomMoveNumber].name);

            //Before updating the health, let's check if the player has any health boosts.
            int LeftoverDamage = 0;
            float LeftoverBoostDamage = 0;
            if (HealthBoost > 0)
            {
                print("Player has a health boost of: " + HealthBoost.ToString());
                if (EnemyDamageToPlayer > HealthBoost)
                {
                    LeftoverDamage = EnemyDamageToPlayer - (int)HealthBoost;
                    print("The leftover damage after the enemy has hit the health boost is: " + LeftoverDamage.ToString());
                    HealthBoost = 0;
                    for (int i = 0; i <= currentActiveBattleBoosts.Count - 1; i++)
                    {
                        currentActiveBattleBoosts[i].HealthBoost = 0;
                    }
                    HealthText.transform.parent.transform.GetChild(7).GetComponent<Text>().text = HealthBoost.ToString();
                } else
                {
                    HealthBoost -= EnemyDamageToPlayer;
                    LeftoverBoostDamage = EnemyDamageToPlayer;
                    for (int i = 0; i <= currentActiveBattleBoosts.Count - 1; i++)
                    {
                        if (currentActiveBattleBoosts[i].HealthBoost - LeftoverBoostDamage > 0)
                        {
                            currentActiveBattleBoosts[i].HealthBoost -= LeftoverBoostDamage;
                            print("LeftoverBoostDamage doesn't break the first health boost. Stopping loop.");
                            break;
                        } else
                        {
                            LeftoverBoostDamage -= currentActiveBattleBoosts[i].HealthBoost;
                            print("LeftoverBoostDamage breaks the first health boost, has: " + LeftoverBoostDamage.ToString() + " left.");
                            currentActiveBattleBoosts[i].HealthBoost = 0;
                        }
                    }
                    HealthText.transform.parent.transform.GetChild(7).GetComponent<Text>().text = HealthBoost.ToString();
                }
            } else
            {
                print("HealthBoost is null.");
            }
            
            if (LeftoverDamage > 0)
            {
                currentMonsterScript.UpdateHealth(Mathf.Clamp(currentMonsterScript.health - LeftoverDamage, 0, currentMonsterScript.maxHealth));
            } else if (LeftoverDamage == 0 && HealthBoost == 0)
            {
                print("LeftoverDamage is 0, so is HealthBoost. Doing normal damage.");
                currentMonsterScript.UpdateHealth(Mathf.Clamp(currentMonsterScript.health - EnemyDamageToPlayer, 0, currentMonsterScript.maxHealth));
            }
            HealthText.text = currentMonsterScript.health.ToString();

            for (float i = 0.0f; i <= 1.0f; i += 0.001f)
            {
                if (SliderComponent.value < currentMonsterScript.health + 0.01f && SliderComponent.value > currentMonsterScript.health - 0.01f)
                {
                    SliderComponent.value = currentMonsterScript.health;
                    break;
                }
                SliderComponent.value = Mathf.Lerp(SliderComponent.value, currentMonsterScript.health, i);
                //print("i: " + i.ToString() + " | SliderValue: " + SliderComponent.value);
                yield return new WaitForSeconds(0.01f);
            }


            print("Done Waiting: EnemyAttackingOnly");
            if (currentMonsterScript.health <= 0)
            {
                currentMonsterScript.UpdateHealth(0);
                BattleAmountOfTurns++;
                this.EndBattle(false);
                yield return null;
                callback(true);
            }
        } else
        {
            print("Attack Missed!");
        }

        //Recalculating all the values for the boosts.
        SpeedBoost = 0;
        AttackBoost = 0;
        DefenceBoost = 0;
        HealthBoost = 0;
        List<int>BoostRemovalInts = new List<int>();
        if (currentActiveBattleBoosts.Count > 0)
        {
            for (int i = 0; i <= currentActiveBattleBoosts.Count - 1; i++)
            {
                currentActiveBattleBoosts[i].SpeedBoost -= (currentActiveBattleBoosts[i].amount / currentActiveBattleBoosts[i].length);
                if (currentActiveBattleBoosts[i].SpeedBoost < 0)
                {
                    currentActiveBattleBoosts[i].SpeedBoost = 0;
                }
                currentActiveBattleBoosts[i].AttackBoost -= (currentActiveBattleBoosts[i].amount / currentActiveBattleBoosts[i].length);
                if (currentActiveBattleBoosts[i].AttackBoost < 0)
                {
                    currentActiveBattleBoosts[i].AttackBoost = 0;
                }
                currentActiveBattleBoosts[i].DefenceBoost -= (currentActiveBattleBoosts[i].amount / currentActiveBattleBoosts[i].length);
                if (currentActiveBattleBoosts[i].DefenceBoost < 0)
                {
                    currentActiveBattleBoosts[i].DefenceBoost = 0;
                }
                currentActiveBattleBoosts[i].HealthBoost -= (currentActiveBattleBoosts[i].amount / currentActiveBattleBoosts[i].length);
                if (currentActiveBattleBoosts[i].HealthBoost < 0)
                {
                    currentActiveBattleBoosts[i].HealthBoost = 0;
                }

                if (currentActiveBattleBoosts[i].SpeedBoost <= 0 && currentActiveBattleBoosts[i].AttackBoost <= 0 && currentActiveBattleBoosts[i].DefenceBoost <= 0 && currentActiveBattleBoosts[i].HealthBoost <= 0)
                {
                    print("Everything is below or equal to 0, boost being removed.");
                    BoostRemovalInts.Add(i);
                } else
                {
                    print("Something is above 0, not removing the current boost.");
                    SpeedBoost += currentActiveBattleBoosts[i].SpeedBoost;
                    AttackBoost += currentActiveBattleBoosts[i].AttackBoost;
                    DefenceBoost += currentActiveBattleBoosts[i].DefenceBoost;
                    HealthBoost += currentActiveBattleBoosts[i].HealthBoost;
                }
            }

            foreach (int numbers in BoostRemovalInts)
            {
                currentActiveBattleBoosts.RemoveAt(numbers);
            }

            BoostRemovalInts.Clear();
        }
        else
        {
            print("You have no boosts right now.");
        }
        print("SpeedBoost: " + SpeedBoost);
        print("AttackBoost: " + AttackBoost);
        print("DefenceBoost: " + DefenceBoost);
        print("HealthBoost: " + HealthBoost);
        HealthText.transform.parent.transform.GetChild(7).GetComponent<Text>().text = HealthBoost.ToString();
        BattleAmountOfTurns++;

        BattleMenuBottom.SetActive(true);
        BattleMenuBottom.transform.GetChild(1).gameObject.SetActive(true);
        BattleMenuBottom.transform.GetChild(2).gameObject.SetActive(false);
        BattleMenuBottom.transform.GetChild(3).gameObject.SetActive(false);
        BattleMenuBottom.transform.GetChild(4).gameObject.SetActive(false);
        IsTurnToBattle = true;
        yield return null;
        callback(false);
    }

    public void RunAway()
    {
        float runningFormulaAnswer = Mathf.Pow((currentMonsterScript.speed * (1.5f) / ENEMY_Speed_MONSTER), 0.7f) * 50;
        print("Run chances: " + runningFormulaAnswer);
        int RandomNumber = Random.Range(1, 101);
        print("Random number: " + RandomNumber);

        if (RandomNumber <= runningFormulaAnswer)
        {
            this.EndBattleRun();
            return;
        } else
        {
            print("Couldn't escape.");
            StartCoroutine(BattleJustEnemy((myReturnValue) => {
                if (myReturnValue == true) {
                    //Ended battle.
                } else
                {
                    BattleMenuBottom.SetActive(true);
                    BattleMenuBottom.transform.GetChild(1).gameObject.SetActive(true);
                    BattleMenuBottom.transform.GetChild(2).gameObject.SetActive(false);
                    BattleMenuBottom.transform.GetChild(3).gameObject.SetActive(false);
                    BattleMenuBottom.transform.GetChild(4).gameObject.SetActive(false);
                }
            }));
        }
    }

    private void EnemyMonsterDrops(Monsters monster)
    {
        //Dealing with the monster dropping crafting materials.
        if (monster.CraftingMaterialsT1[1] > 0)
        {
            int RandomNumber = Random.Range(monster.CraftingMaterialsT1[0], monster.CraftingMaterialsT1[1] + 1);
            print("Random Number for the crafting materials tier 1 is: " + RandomNumber);
            currentMonsterScript.materials[0] += RandomNumber;
        } else
        {
            print("This monster: " + monster.name + " does not drop tier 1 crafting materials.");
        }
        if (monster.CraftingMaterialsT2[1] > 0)
        {
            int RandomNumber = Random.Range(monster.CraftingMaterialsT2[0], monster.CraftingMaterialsT2[1] + 1);
            print("Random Number for the crafting materials tier 2 is: " + RandomNumber);
            currentMonsterScript.materials[1] += RandomNumber;
        }
        else
        {
            print("This monster: " + monster.name + " does not drop tier 2 crafting materials.");
        }
        if (monster.CraftingMaterialsT3[1] > 0)
        {
            int RandomNumber = Random.Range(monster.CraftingMaterialsT3[0], monster.CraftingMaterialsT3[1] + 1);
            print("Random Number for the crafting materials tier 3 is: " + RandomNumber);
            currentMonsterScript.materials[2] += RandomNumber;
        }
        else
        {
            print("This monster: " + monster.name + " does not drop tier 3 crafting materials.");
        }
        if (monster.CraftingMaterialsT4[1] > 0)
        {
            int RandomNumber = Random.Range(monster.CraftingMaterialsT4[0], monster.CraftingMaterialsT4[1] + 1);
            print("Random Number for the crafting materials tier 4 is: " + RandomNumber);
            currentMonsterScript.materials[3] += RandomNumber;
        }
        else
        {
            print("This monster: " + monster.name + " does not drop tier 4 crafting materials.");
        }

        //Dealing with the other drops.
        if (monster.MonstersDropTable.Count > 0)
        {
            for (int i = 0; i < monster.MonstersDropTable.Count; i++)
            {
                int RandomNumber = Random.Range(1, 101);
                if (RandomNumber < monster.MonstersDropTable[i].percentageChance)
                {
                    print("You are getting an item by the name of: " + monster.MonstersDropTable[i].drop.name);
                    for (int v = 0; v < monster.MonstersDropTable[i].amountDropped; v++)
                    {
                        if (monster.MonstersDropTable[i].drop.type == DropDownItem.boost)
                        {
                            if (currentMonsterScript.itemsBoosts.Count < currentMonsterScript.MaxSpace)
                            {
                                print("Adding a boost to your inventory.");
                                currentMonsterScript.itemsBoosts.Add(monster.MonstersDropTable[i].drop);
                            }
                            else
                            {
                                print("Your item inventory is full!");
                            }
                        }
                        else if (monster.MonstersDropTable[i].drop.type == DropDownItem.potion)
                        {
                            if (currentMonsterScript.itemsPotions.Count < currentMonsterScript.MaxSpace)
                            {
                                print("Adding a potion to your inventory.");
                            currentMonsterScript.itemsPotions.Add(monster.MonstersDropTable[i].drop);
                        }
                            else
                            {
                                print("Your item inventory is full!");
                            }
                        }
                        else if (monster.MonstersDropTable[i].drop.type == DropDownItem.item)
                        {
                            if (currentMonsterScript.items.Count < currentMonsterScript.MaxSpace)
                            {
                                print("Adding an item to your inventory.");
                                currentMonsterScript.items.Add(monster.MonstersDropTable[i].drop);
                        }
                            else
                            {
                                print("Your item inventory is full!");
                            }
                        }
                        else
                        {
                        print("Something went wrong when trying to add item: " + monster.MonstersDropTable[i].drop.name + " to your inventory after a battle, item type is: " + monster.MonstersDropTable[i].drop.type);
                    }
                    }
                }
            }
        }
    }

    private void EndBattle(bool won)
    {
        if (won == true)
        {
            StaticClasses.BattleWins++;
            if (StaticClasses.BattleWins == 1)
            {
                playerScript.GainAchievement(1);
            }
            print("You have " + StaticClasses.BattleWins + " wins.");
        } else
        {
            StaticClasses.BattleLosses++;
            print("You have " + StaticClasses.BattleLosses + " losses.");
        }
        this.IsTurnToBattle = false;
        StaticClasses.IsInBattle = false;
        BattleAttackMove1Text.transform.parent.transform.parent.gameObject.SetActive(false);
        BattleAttackMove1Text.transform.parent.transform.parent.transform.parent.gameObject.SetActive(false);
        Battle_UI.SetActive(false);
        ENEMY_Battle_UI.SetActive(false);
        currentActiveBattleBoosts.Clear();
        HealthText.transform.parent.transform.GetChild(7).GetComponent<Text>().text = "0";
        print("This battle took " + BattleAmountOfTurns + " turns to complete.");
        if (BattleAmountOfTurns == 1)
        {
            playerScript.GainAchievement(2);
        }
        Cursor.lockState = CursorLockMode.Locked;
        return;
    }

    private void EndBattleRun()
    {
        this.IsTurnToBattle = false;
        StaticClasses.IsInBattle = false;
        BattleAttackMove1Text.transform.parent.transform.parent.gameObject.SetActive(false);
        BattleAttackMove1Text.transform.parent.transform.parent.transform.parent.gameObject.SetActive(false);
        Battle_UI.SetActive(false);
        ENEMY_Battle_UI.SetActive(false);
        currentActiveBattleBoosts.Clear();
        HealthText.transform.parent.transform.GetChild(7).GetComponent<Text>().text = "0";
        Cursor.lockState = CursorLockMode.Locked;
        return;
    }
}