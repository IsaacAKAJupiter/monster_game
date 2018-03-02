using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private GameObject player;
    private Animator Animator;
    public Rigidbody playerRigidbody;
    public Sprite playerSprite;
    private CharacterController charController;

    public float speed = 6.0f;
    public float sprint = 1.05f;
    public float jumpMultiplier = 25f;
    public float gravity = 50f;
    private Vector3 moveDirection = Vector3.zero;
    private bool AssigningKey = false;
    private string WhichControl = null;
    private bool IsShopShowingBuying = true;

    public List<Moves> moves = new List<Moves>();
    public List<Items> items = new List<Items>();
    public List<Achievements> achievements = new List<Achievements>();
    public List<Quests> quests = new List<Quests>();

    private CurrentMonsterOnPlayer playerInventory;
    private PlayerStats playerStats;

    public GameObject InteractableUI;
    public GameObject MoveConstructingUI;
    public GameObject PlayerInventoryUI;
    public GameObject PauseMenuUI;
    public GameObject ShopUI;
    public GameObject DialogueUI;

    public Button ExitMoveConstructingUIButton;
    public Button ExitPlayerInventoryUIButton;

    public GameObject PlayerInventoryRightBackground;
    public GameObject PlayerInventoryLeftButtons;
    public GameObject PlayerInventoryRightPlaceholders;

    public Image PlayerInventoryMonsterSprite;
    public Text PlayerInventoryMonsterAttack;
    public Text PlayerInventoryMonsterID;
    public Text PlayerInventoryMonsterName;
    public Text PlayerInventoryMonsterDescription;
    public Text PlayerInventoryMonsterDefence;
    public Text PlayerInventoryMonsterSpeed;
    public Text PlayerInventoryMonsterHealth;
    public Text PlayerInventoryMonsterMaxHealth;
    public Text PlayerInventoryMonsterLevel;
    public Button PlayerInventoryMonsterMove1;
    public Button PlayerInventoryMonsterMove2;
    public GameObject ReplacingPlayerMonsterMoveUIInventory;
    public GameObject ReplacingPlayerMonsterMoveUIInventoryLeft;

    //Move Contructing UI
    public GameObject BackgroundRightPart;
    public Text DamageValueUI;
    public Text AccuracyValueUI;
    public Text MaterialsT1ValueUI;
    public Text MaterialsT2ValueUI;
    public Text MaterialsT3ValueUI;
    public Text MaterialsT4ValueUI;
    public Text MoveNameUI;
    public Image MoveSpriteUI;
    public Text MoveDescriptionUI;
    public Button ConstructButton;
    public int MoveID;

    //Chemistry UI
    public GameObject ChemistryUI;
    public GameObject ChemistryBackgroundRightPart;
    public Text ChemistryHealthValueUI;
    public Text ChemistryBoostValueUI;
    public Text ChemistryMaterialsT1ValueUI;
    public Text ChemistryMaterialsT2ValueUI;
    public Text ChemistryMaterialsT3ValueUI;
    public Text ChemistryMaterialsT4ValueUI;
    public Text ChemistryItemNameUI;
    public Image ChemistryItemSpriteUI;
    public Text ChemistryItemDescriptionUI;
    public Button ChemistryUIExitButton;
    public Button ChemistryMixButton;
    public int ItemID;

    // Use this for initialization
    void Start()
    {
        //TEMP WHILE I DON'T HAVE SAVES!
        this.ResetQuests();
        this.ResetAchievements();

        //Getting Components; in children or not.
        player = GetComponentInChildren<Transform>().gameObject;
        Animator = GetComponentInChildren<Animator>();
        charController = GetComponentInChildren<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
        playerInventory = GetComponent<CurrentMonsterOnPlayer>();

        //Making the cursor in a locked state when the game starts.
        Cursor.lockState = CursorLockMode.Locked;

        //Setting all the UI's active property to false so that you don't get random UI's in the beginning of the game.
        InteractableUI.SetActive(false);
        MoveConstructingUI.SetActive(false);
        PlayerInventoryUI.SetActive(false);
        PauseMenuUI.SetActive(false);
        ShopUI.SetActive(false);
        DialogueUI.SetActive(false);
      
        //Adding Listeners to Buttons.
        ExitMoveConstructingUIButton.onClick.AddListener(UIButtonExiterChangeToLockedCursor);
        ExitPlayerInventoryUIButton.onClick.AddListener(UIButtonExiterChangeToLockedCursor);
        ChemistryUIExitButton.onClick.AddListener(UIButtonExiterChangeToLockedCursor);
        PlayerInventoryMonsterMove1.onClick.AddListener(delegate { ChoosingMove(1); });
        PlayerInventoryMonsterMove2.onClick.AddListener(delegate { ChoosingMove(2); });
        ChemistryMixButton.onClick.AddListener(MixButtonPressed);
        ConstructButton.onClick.AddListener(ConstructButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {
        //Dealing with the pressing of a keyboard key when inside an Interactable.
        if (StaticClasses.IsInteractable == true && StaticClasses.IsInBattle == false && StaticClasses.IsGamePaused == false && StaticClasses.IsInDialogue == false)
        {
            if (Input.GetKeyDown(StaticClasses.InteractableButton))
            {
                print("Pressed " + StaticClasses.InteractableButton + " on Interactable");
                if (StaticClasses.WhatInteractableItem == "MoveConstructingUITrigger")
                {
                    this.MoveConstructingUITrigger();
                }
                else if (StaticClasses.WhatInteractableItem == "ChemistryUITrigger")
                {
                    this.ChemistryUITrigger();
                } else if (StaticClasses.WhatInteractableItem == "ShopUITrigger")
                {
                    this.ShopUITrigger();
                } else if (StaticClasses.WhatInteractableItem == "DialogueTrigger")
                {
                    StaticClasses.WhatInteractableItemGO.transform.parent.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
                    StaticClasses.IsInDialogue = true;
                    DialogueUI.SetActive(true);
                    this.ChangeMouseCursorMode(CursorLockMode.None);
                    InteractableUI.SetActive(false);
                }
            }
        }

        //Dealing with the pressing of the inventory key.
        if (Input.GetKeyDown(StaticClasses.InventoryButton))
        {
            //Checking if any other UI is open, game is paused, in dialogue, or in battle.
            if (StaticClasses.IsInBattle == true || StaticClasses.IsGamePaused == true || StaticClasses.IsInDialogue == true || StaticClasses.UIIsOpen == true)
            {
                return;
            }
            if (PlayerInventoryUI.activeSelf == true)
            {
                PlayerInventoryUI.SetActive(false);
                print("Pressed " + StaticClasses.InventoryButton + "; closing Inventory.");
                return;
            }
            print("Pressed " + StaticClasses.InventoryButton + "; opening Inventory.");
            this.ChangeMouseCursorMode(CursorLockMode.None);
            StaticClasses.UIIsOpen = true;
            PlayerInventoryUI.SetActive(true);

            //Setting all of the right UI's active state to false, except inventory; which is true.
            PlayerInventoryMonsterAttack.transform.parent.transform.parent.transform.GetChild(0).gameObject.SetActive(true);
            PlayerInventoryMonsterAttack.transform.parent.transform.parent.transform.GetChild(1).gameObject.SetActive(false);
            PlayerInventoryMonsterAttack.transform.parent.transform.parent.transform.GetChild(2).gameObject.SetActive(false);
            PlayerInventoryMonsterAttack.transform.parent.transform.parent.transform.GetChild(3).gameObject.SetActive(false);
            PlayerInventoryLeftButtons.SetActive(true);
            ReplacingPlayerMonsterMoveUIInventory.SetActive(false);
            ReplacingPlayerMonsterMoveUIInventoryLeft.SetActive(false);

            this.PlayerInventoryUpdate("Inventory");
        }

        //Dealing with the movement of the character controller.
        if (StaticClasses.IsInBattle == false && StaticClasses.IsGamePaused == false && StaticClasses.IsInDialogue == false && StaticClasses.UIIsOpen == false)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.GetChild(0).transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetKeyDown(StaticClasses.JumpButton) && charController.isGrounded)
            {
                moveDirection.y = jumpMultiplier;
            }

            if (Input.GetKey(StaticClasses.SprintButton))
            {
                moveDirection *= sprint;
                moveDirection.y -= gravity * Time.deltaTime;
                charController.Move(moveDirection * Time.deltaTime);
                Animator.SetFloat("SpeedPercent", 1.7f, 0.1f, Time.deltaTime);
            }

            moveDirection.y -= gravity * Time.deltaTime;
            charController.Move(moveDirection * Time.deltaTime);
            Animator.SetFloat("SpeedPercent", 0.5f, 0.1f, Time.deltaTime);
        }

        if (charController.velocity.magnitude > 0) {
            StaticClasses.CharControllerVelocity = true;
        } else
        {
            StaticClasses.CharControllerVelocity = false;
        }

        if (charController.velocity == Vector3.zero)
        {
            Animator.SetFloat("SpeedPercent", -0.7f, 0.1f, Time.deltaTime);
        }

        //Dealing with the user pressing escape while playing.
        if (Input.GetKeyDown(StaticClasses.PauseButton) && StaticClasses.IsInBattle == false)
        {
            if (StaticClasses.IsGamePaused == false)
            {
                StaticClasses.IsGamePaused = true;
                this.ChangeMouseCursorMode(CursorLockMode.None);
                PauseMenuUI.SetActive(true);

                //Setting all PauseMenuUI actives to the original/default state.
                foreach (Transform button in PauseMenuUI.transform.GetChild(1).GetComponentInChildren<Transform>())
                {
                    button.gameObject.SetActive(true);
                }
                PauseMenuUI.transform.GetChild(1).gameObject.SetActive(true);
                PauseMenuUI.transform.GetChild(2).gameObject.SetActive(false);
                PauseMenuUI.transform.GetChild(3).gameObject.SetActive(false);
                PauseMenuUI.transform.GetChild(4).gameObject.SetActive(false);
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void OnGUI()
    {
        //Changing controls.
        if (Event.current.isKey && AssigningKey == true && WhichControl != null)
        {
            if (Event.current.keyCode != KeyCode.Escape)
            {
                if (StaticClasses.InteractableButton == Event.current.keyCode)
                {
                    StaticClasses.InteractableButton = KeyCode.None;
                } else if(StaticClasses.InventoryButton == Event.current.keyCode)
                {
                    StaticClasses.InventoryButton = KeyCode.None;
                } else if (StaticClasses.SprintButton == Event.current.keyCode)
                {
                    StaticClasses.SprintButton = KeyCode.None;
                } else if (StaticClasses.JumpButton == Event.current.keyCode)
                {
                    StaticClasses.JumpButton = KeyCode.None;
                }
                ActuallyChangingControl(WhichControl, Event.current.keyCode);
            }
        }
    }

    //This is for when you press a key while setting a key.
    private void ActuallyChangingControl(string control, KeyCode keyCode)
    {
        if (control == "Sprint")
        {
            StaticClasses.SprintButton = keyCode;
            WhichControl = null;
            AssigningKey = false;
            LoadControlsUI();
        } else if (control == "Interactable")
        {
            StaticClasses.InteractableButton = keyCode;
            WhichControl = null;
            AssigningKey = false;
            LoadControlsUI();
        } else if (control == "Inventory")
        {
            StaticClasses.InventoryButton = keyCode;
            WhichControl = null;
            AssigningKey = false;
            LoadControlsUI();
        } else if (control == "Jump")
        {
            StaticClasses.JumpButton = keyCode;
            WhichControl = null;
            AssigningKey = false;
            LoadControlsUI();
        }
    }

    //This is to set the AssigningKey to true and WhichControl to the control that is being switched.
    public void StartChangingControl(GameObject control)
    {
        if (AssigningKey == false && WhichControl == null)
        {
            control.transform.GetChild(2).GetComponent<Text>().text = "Press a Button";
            AssigningKey = true;
            WhichControl = control.name;
        }
    }

    public void ResumeGame()
    {
        if (AssigningKey == false && WhichControl == null)
        {
            StaticClasses.IsGamePaused = false;
            this.ChangeMouseCursorMode(CursorLockMode.Locked);
            PauseMenuUI.SetActive(false);
        }
    }
    
    public void QuitGame()
    {
        //TODO: When I add save, make it save before quitting.
        Application.Quit();
    }
    
    public void SaveGame()
    {
        //TODO: Add saving lmao.
        print("You pressed the save button; DOESN'T WORK.");
    }

    public void ChangeDifficulty(int number)
    {
        if (number == 0)
        {
            StaticClasses.AIDifficulty = "Easy";
            print("AIDifficulty is now: " + StaticClasses.AIDifficulty);
        } else if (number == 1)
        {
            StaticClasses.AIDifficulty = "Hard";
            print("AIDifficulty is now: " + StaticClasses.AIDifficulty);
        }  else
        {
            print("Something went wrong when changing the difficulty.");
        }
    }

    public void LoadControlsUI()
    {
        for (int i = 4; i <= PauseMenuUI.transform.GetChild(3).childCount - 1; i++)
        {
            if (PauseMenuUI.transform.GetChild(3).GetChild(i).gameObject.name == "Interactable")
            {
                PauseMenuUI.transform.GetChild(3).GetChild(i).GetChild(2).GetComponent<Text>().text = StaticClasses.InteractableButton.ToString();
            } else if (PauseMenuUI.transform.GetChild(3).GetChild(i).gameObject.name == "Inventory")
            {
                PauseMenuUI.transform.GetChild(3).GetChild(i).GetChild(2).GetComponent<Text>().text = StaticClasses.InventoryButton.ToString();
            } else if (PauseMenuUI.transform.GetChild(3).GetChild(i).gameObject.name == "Jump")
            {
                PauseMenuUI.transform.GetChild(3).GetChild(i).GetChild(2).GetComponent<Text>().text = StaticClasses.JumpButton.ToString();
            } else if (PauseMenuUI.transform.GetChild(3).GetChild(i).gameObject.name == "Sprint")
            {
                PauseMenuUI.transform.GetChild(3).GetChild(i).GetChild(2).GetComponent<Text>().text = StaticClasses.SprintButton.ToString();
            }
        }
    }

    //This function is TEMP while I don't have saving the game.
    private void ResetQuests()
    {
        foreach (Quests quest in quests) {
            quest.CurrentPart = 0;
            quest.IsCompleted = false;
            quest.IsStarted = false;
        }
    }

    //This function is TEMP while I don't have saving the game.
    private void ResetAchievements()
    {
        foreach (Achievements achievement in achievements)
        {
            achievement.completed = false;
        }
    }

    //This function will deal with starting quests.
    public void StartQuest(int id)
    {
        if (quests[id - 1].MainQuest == true)
        {
            foreach (Quests quest in quests)
            {
                if (quest.MainQuest == true && quest.IsStarted == true && quest.IsCompleted == false)
                {
                    print("You are already doing a main quest!");
                    return;
                }
            }
        }
        print("Starting Quest with ID: " + id);
        quests[id - 1].IsStarted = true;
        //Make sure that IsCompleted isn't true and you are on step 1.
        quests[id - 1].IsCompleted = false;
        quests[id - 1].CurrentPart = 1;
    }

    //This function will deal with advancing to the next part of a quest.
    public void AdvanceQuest(int id)
    {
        print("Advancing a step in Quest with ID: " + id + " | Current step = " + quests[id - 1].CurrentPart + " | Going to step: " + (quests[id - 1].CurrentPart + 1));
        //Make sure that you are still starting and not completing the quest. Then advance.
        quests[id - 1].IsStarted = true;
        quests[id - 1].IsCompleted = false;
        quests[id - 1].CurrentPart++;
    }

    //This function will deal with completing quests.
    public void CompleteQuest(int id)
    {
        //Increase step by 1 for the final amount of steps. Make sure IsStarted is still true. Then make IsCompleted true and give rewards through the PlayerScript.
        quests[id - 1].CurrentPart++;
        quests[id - 1].IsStarted = true;
        quests[id - 1].IsCompleted = true;
        print("Completing Quest with ID: " + id + " | This quest took " + quests[id - 1].CurrentPart + " parts to complete.");
        this.GainQuestRewards(id - 1);
    }

    //This function is used to change the ItemID variable.
    public void ChangeItemID(int ItemID)
    {
        this.ItemID = ItemID;
    }

    //This function is used to change the MoveID variable.
    public void ChangeMoveID(int MoveID)
    {
        this.MoveID = MoveID;
    }

    //This function is used for when the user is exiting a UI; making their mouse back into a locked state. This also sets the UIIsOpen variable to become false.
    public void UIButtonExiterChangeToLockedCursor()
    {
        this.ChangeMouseCursorMode(CursorLockMode.Locked);
        StaticClasses.UIIsOpen = false;
    }

    //This function is for when the user wants to change their cursor's lockstate.
    public void ChangeMouseCursorMode(CursorLockMode cursor)
    {
        Cursor.lockState = cursor;
    }

    //This function is for when the ConstructButton is pressed.
    private void ConstructButtonPressed()
    {
        this.MoveConstructingButtonPurchaseUITrigger(this.MoveID);
    }

    //This function is for when the MixButton is pressed.
    private void MixButtonPressed()
    {
        this.ChemistryButtonPurchaseUITrigger(this.ItemID);
    }

    //This function is for setting up the first part of choosing a more to replace.
    public void ChoosingMove(int WhichMove)
    {
        for (int i = 0; i < playerInventory.MaxSpace; i++)
        {
            ReplacingPlayerMonsterMoveUIInventory.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = null;
        }
        for (int i = 0; i < playerInventory.MaxSpace; i++)
        {
            if (playerInventory.moveCards.Count > i)
            {
                ReplacingPlayerMonsterMoveUIInventory.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = playerInventory.moveCards[i].sprite;
            }
        }
        for (int i = 0; i < playerInventory.MaxSpace; i++)
        {
            int copy = i;
            ReplacingPlayerMonsterMoveUIInventory.transform.GetChild(copy).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        for (int i = 0; i < playerInventory.MaxSpace; i++)
        {
            if (playerInventory.moveCards.Count > i)
            {
                int copy = i;
                ReplacingPlayerMonsterMoveUIInventory.transform.GetChild(copy).gameObject.GetComponent<Button>().onClick.AddListener(delegate { ChoosingMovePart2(playerInventory.moveCards[copy].id); });
            }
        }

        if (WhichMove == 1)
        {
            ReplacingPlayerMonsterMoveUIInventory.transform.GetChild(12).gameObject.tag = "Move1ForChoosingMove";
        }
        else if (WhichMove == 2)
        {
            ReplacingPlayerMonsterMoveUIInventory.transform.GetChild(12).gameObject.tag = "Move2ForChoosingMove";
        }
    }

    //This function is for setting up the second part of choosing a more to replace.
    public void ChoosingMovePart2(int moveID)
    {
        string GetReplacingMove1 = ReplacingPlayerMonsterMoveUIInventory.transform.GetChild(12).gameObject.tag.Replace("Move", "");
        string GetReplacingMove2 = GetReplacingMove1.Replace("ForChoosing", "");
        int GetReplacingMove3 = 0;
        if (Int32.TryParse(GetReplacingMove2, out GetReplacingMove3))
        {
            print("Parsing worked!");
            playerInventory.AddMoveToMonster(this.moves[moveID - 1], moveID, GetReplacingMove3 - 1);

            this.ChangeMouseCursorMode(CursorLockMode.Locked);
            PlayerInventoryUI.SetActive(false);
            PlayerInventoryMonsterAttack.transform.parent.transform.parent.transform.GetChild(0).gameObject.SetActive(false);
            PlayerInventoryMonsterAttack.transform.parent.transform.parent.transform.GetChild(1).gameObject.SetActive(false);
            PlayerInventoryMonsterAttack.transform.parent.transform.parent.transform.GetChild(2).gameObject.SetActive(false);
            PlayerInventoryMonsterAttack.transform.parent.transform.parent.transform.GetChild(3).gameObject.SetActive(false);
            ReplacingPlayerMonsterMoveUIInventory.SetActive(false);
            ReplacingPlayerMonsterMoveUIInventoryLeft.SetActive(false);
            this.PlayerInventoryUpdate("Inventory");
            StaticClasses.UIIsOpen = false;
        }
    }

    //This function will deal with when a ShopUITrigger is activated.
    private void ShopUITrigger()
    {
        ShopUI.SetActive(true);
        InteractableUI.SetActive(false);
        StaticClasses.UIIsOpen = true;
        Cursor.lockState = CursorLockMode.None;
        GameObject PageButtons = ShopUI.transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        PageButtons.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Page: 1";
        PageButtons.transform.GetChild(2).gameObject.SetActive(false);
        PageButtons.transform.GetChild(1).gameObject.SetActive(true);
        //Get all selling and buying IDs just incase the WhatInteractableItemGO changes for some odd reason.
        List<int> ItemsSellingIDs = new List<int>();
        ItemsSellingIDs.AddRange(StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsSellingIDs);
        List<int> ItemsBuyingIDs = new List<int>();
        ItemsBuyingIDs.AddRange(StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsBuyingIDs);

        //Check the shop is selling more than 12 objects.
        if (ItemsSellingIDs.Count > 12) {
            int MaxPages = (ItemsSellingIDs.Count / 12);
            int BeginningMaxPageItemsAmount = MaxPages * 12;
            if (BeginningMaxPageItemsAmount - ItemsSellingIDs.Count > 0)
            {
                MaxPages++;
            }
            print("Max Amount of pages is: " + MaxPages);
            ShopUI.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(true);
            for (int i = 0; i < 12; i++)
            {
                ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = null;
                ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            }
            for (int i = 0; i < 12; i++)
            {
                ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = items[ItemsSellingIDs[i] - 1].sprite;
                int ItemID2 = items[ItemsSellingIDs[i] - 1].id;
                ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(delegate { BuyItemFromShop(ItemID2); });
            }
        } else
        {
            for (int i = 0; i < 12; i++)
            {
                ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = null;
                ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            }
                for (int i = 0; i <= ItemsSellingIDs.Count - 1; i++)
            {
                ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = items[ItemsSellingIDs[i] - 1].sprite;
                int ItemID2 = items[ItemsSellingIDs[i] - 1].id;
                ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(delegate { BuyItemFromShop(ItemID2); });
            }

        }
    }

    public void ShopUIChangeShopSellOrBuy(bool IsBuy)
    {
        //Get all selling and buying IDs just incase the WhatInteractableItemGO changes for some odd reason.
        List<int> ItemsSellingIDs = new List<int>();
        ItemsSellingIDs.AddRange(StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsSellingIDs);
        List<int> ItemsBuyingIDs = new List<int>();
        ItemsBuyingIDs.AddRange(StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsBuyingIDs);
        GameObject PageButtons = ShopUI.transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        PageButtons.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Page: 1";
        PageButtons.transform.GetChild(2).gameObject.SetActive(false);
        PageButtons.transform.GetChild(1).gameObject.SetActive(true);

        if (IsBuy == true)
        {
            this.IsShopShowingBuying = true;
            //Check the shop is selling more than 12 objects.
            if (ItemsSellingIDs.Count > 12)
            {
                int MaxPages = (ItemsSellingIDs.Count / 12);
                int BeginningMaxPageItemsAmount = MaxPages * 12;
                if (BeginningMaxPageItemsAmount - ItemsSellingIDs.Count < 0)
                {
                    MaxPages++;
                }
                print("Max Amount of pages is: " + MaxPages);
                ShopUI.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(true);
                for (int i = 0; i < 12; i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = null;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                }
                for (int i = 0; i < 12; i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = items[ItemsSellingIDs[i] - 1].sprite;
                    int ItemID2 = items[ItemsSellingIDs[i] - 1].id;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(delegate { BuyItemFromShop(ItemID2); });
                }
            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = null;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                }
                for (int i = 0; i <= ItemsSellingIDs.Count - 1; i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = items[ItemsSellingIDs[i] - 1].sprite;
                    int ItemID2 = items[ItemsSellingIDs[i] - 1].id;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(delegate { BuyItemFromShop(ItemID2); });
                }

            }
        } else
        {
            this.IsShopShowingBuying = false;
            //Check the shop is selling more than 12 objects.
            if (ItemsBuyingIDs.Count > 12)
            {
                int MaxPages = (ItemsBuyingIDs.Count / 12);
                int BeginningMaxPageItemsAmount = MaxPages * 12;
                if (BeginningMaxPageItemsAmount - ItemsBuyingIDs.Count < 0)
                {
                    MaxPages++;
                }
                print("Max Amount of pages is: " + MaxPages);
                ShopUI.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(true);
                for (int i = 0; i < 12; i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = null;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                }
                for (int i = 0; i < 12; i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = items[ItemsBuyingIDs[i] - 1].sprite;
                    int ItemID2 = items[ItemsBuyingIDs[i] - 1].id;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(delegate { SellItemToShop(ItemID2); });
                }
            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = null;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                }
                for (int i = 0; i < ItemsBuyingIDs.Count; i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = items[ItemsBuyingIDs[i] - 1].sprite;
                    int ItemID2 = items[ItemsBuyingIDs[i] - 1].id;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(delegate { SellItemToShop(ItemID2); });
                }

            }
        }
    }

    public void ShopUIChangePage(bool NextPage)
    {
        GameObject PageButtons = ShopUI.transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        string PageNumberString = PageButtons.transform.GetChild(0).GetChild(0).GetComponent<Text>().text.Replace("Page: ", "");
        int MaxPages = 0;
        if (this.IsShopShowingBuying == true)
        {
            MaxPages = (StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsSellingIDs.Count / 12);
            int BeginningMaxPageItemsAmount = MaxPages * 12;
            if (BeginningMaxPageItemsAmount - StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsSellingIDs.Count < 0)
            {
                MaxPages++;
            }
        } else
        {
            MaxPages = (StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsBuyingIDs.Count / 12);
            int BeginningMaxPageItemsAmount = MaxPages * 12;
            if (BeginningMaxPageItemsAmount - StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsBuyingIDs.Count < 0)
            {
                MaxPages++;
            }
        }
        
        int PageNumber = 0;
        if (Int32.TryParse(PageNumberString, out PageNumber))
        {
            if (NextPage == true)
            {
                PageNumber++;
            } else
            {
                PageNumber--;
            }
        }
        //Set these to be seen to make sure that they show up when necessary.
        PageButtons.transform.GetChild(2).gameObject.SetActive(true);
        PageButtons.transform.GetChild(1).gameObject.SetActive(true);
        if (PageNumber <= 1) 
        {
            PageNumber = 1;
            PageButtons.transform.GetChild(2).gameObject.SetActive(false);
        }
        if (PageNumber >= MaxPages)
        {
            PageNumber = MaxPages;
            PageButtons.transform.GetChild(1).gameObject.SetActive(false);
        }
        PageButtons.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Page: " + PageNumber.ToString();
        if (this.IsShopShowingBuying == true)
        {
            if (PageNumber == MaxPages)
            {
                for (int i = 0; i < 12; i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = null;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                }
                int MaxPageItemsAmount = MaxPages * 12;
                int EmptyItemSlotsAfterAmount = MaxPageItemsAmount - StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsSellingIDs.Count;
                for (int i = 0; i < (12 - EmptyItemSlotsAfterAmount); i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = items[StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsSellingIDs[i + 1 * (12 * (PageNumber - 1))] - 1].sprite;
                    int ItemID2 = items[StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsSellingIDs[i + 1 * (12 * (PageNumber - 1))] - 1].id;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(delegate { BuyItemFromShop(ItemID2); });
                }
            } else
            {
                for (int i = 0; i < 12; i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = null;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                }
                for (int i = 0; i < 12; i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = items[StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsSellingIDs[i + 1 * (12 * (PageNumber - 1))] - 1].sprite;
                    int ItemID2 = items[StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsSellingIDs[i + 1 * (12 * (PageNumber - 1))] - 1].id;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(delegate { BuyItemFromShop(ItemID2); });
                }
            }
        } else
        {
            if (PageNumber == MaxPages)
            {
                for (int i = 0; i < 12; i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = null;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                }
                int MaxPageItemsAmount = MaxPages * 12;
                int EmptyItemSlotsAfterAmount = MaxPageItemsAmount - StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsBuyingIDs.Count;
                for (int i = 0; i < (12 - EmptyItemSlotsAfterAmount); i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = items[StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsBuyingIDs[i + 1 * (12 * (PageNumber - 1))] - 1].sprite;
                    int ItemID2 = items[StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsBuyingIDs[i + 1 * (12 * (PageNumber - 1))] - 1].id;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(delegate { SellItemToShop(ItemID2); });
                }
            } else
            {
                for (int i = 0; i < 12; i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = null;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                }
                for (int i = 0; i < 12; i++)
                {
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = items[StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsBuyingIDs[i + 1 * (12 * (PageNumber - 1))] - 1].sprite;
                    int ItemID2 = items[StaticClasses.WhatInteractableItemGO.GetComponent<ShopItems>().ItemsBuyingIDs[i + 1 * (12 * (PageNumber - 1))] - 1].id;
                    ShopUI.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(delegate { SellItemToShop(ItemID2); });
                }
            } 
        }
    }

    public void BuyItemFromShop(int ItemID)
    {
        if (playerInventory.Monsto >= items[ItemID - 1].buyPrice)
        {
            if (items[ItemID - 1].type == DropDownItem.boost)
            {
                for (int i = 0; i < playerInventory.itemsBoosts.Count; i++)
                {
                    if (playerInventory.itemsBoosts[i].id == ItemID && playerInventory.itemsBoosts.Count < playerInventory.MaxSpace)
                    {
                        playerInventory.Monsto -= playerInventory.itemsBoosts[i].buyPrice;
                        playerInventory.itemsBoosts.Add(playerInventory.itemsBoosts[i]);
                        print("You now have: " + playerInventory.Monsto + " Monsto.");
                        break;
                    }
                }
            }
            else if (items[ItemID - 1].type == DropDownItem.potion)
            {
                for (int i = 0; i < playerInventory.itemsPotions.Count; i++)
                {
                    if (playerInventory.itemsPotions[i].id == ItemID && playerInventory.itemsPotions.Count < playerInventory.MaxSpace)
                    {
                        playerInventory.Monsto -= playerInventory.itemsPotions[i].buyPrice;
                        playerInventory.itemsPotions.Add(playerInventory.itemsPotions[i]);
                        print("You now have: " + playerInventory.Monsto + " Monsto.");
                        break;
                    }
                }
            }
            else if (items[ItemID - 1].type == DropDownItem.item)
            {
                for (int i = 0; i < playerInventory.items.Count; i++)
                {
                    if (playerInventory.items[i].id == ItemID && playerInventory.items.Count < playerInventory.MaxSpace)
                    {
                        playerInventory.Monsto -= playerInventory.items[i].buyPrice;
                        playerInventory.items.Add(playerInventory.items[i]);
                        print("You now have: " + playerInventory.Monsto + " Monsto.");
                        break;
                    }
                }
            }
        } else
        {
            print("You do not have enough Monsta to purchase this. AMOUNT NEEDED: " + items[ItemID - 1].buyPrice + " | AMOUNT HAVE: " + playerInventory.Monsto);
        }
    }

    public void SellItemToShop(int ItemID)
    {
        if (items[ItemID - 1].type == DropDownItem.boost)
        {
            for (int i = 0; i < playerInventory.itemsBoosts.Count; i++)
            {
                if (playerInventory.itemsBoosts[i].id == ItemID)
                {
                    playerInventory.Monsto += playerInventory.itemsBoosts[i].sellPrice;
                    playerInventory.itemsBoosts.Remove(playerInventory.itemsBoosts[i]);
                    print("You now have: " + playerInventory.Monsto + " Monsto.");
                    break;
                }
            }
        } else if (items[ItemID - 1].type == DropDownItem.potion)
        {
            for (int i = 0; i < playerInventory.itemsPotions.Count; i++)
            {
                if (playerInventory.itemsPotions[i].id == ItemID)
                {
                    playerInventory.Monsto += playerInventory.itemsPotions[i].sellPrice;
                    playerInventory.itemsPotions.Remove(playerInventory.itemsPotions[i]);
                    print("You now have: " + playerInventory.Monsto + " Monsto.");
                    break;
                }
            }
        } else if (items[ItemID - 1].type == DropDownItem.item)
        {
            for (int i = 0; i < playerInventory.items.Count; i++)
            {
                if (playerInventory.items[i].id == ItemID)
                {
                    playerInventory.Monsto += playerInventory.items[i].sellPrice;
                    playerInventory.items.Remove(playerInventory.items[i]);
                    print("You now have: " + playerInventory.Monsto + " Monsto.");
                    break;
                }
            }
        }
    }

    //This function will deal with when a MoveConstructingUITrigger is activated.
    private void MoveConstructingUITrigger()
    {
        if (StaticClasses.UIIsOpen == true)
        {
            return;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        print("MoveConstructingUITrigger");
        InteractableUI.SetActive(false);
        MoveConstructingUI.SetActive(true);
        BackgroundRightPart.SetActive(false);
        StaticClasses.UIIsOpen = true;
    }

    //This function will deal with when a ChemistryUITrigger is activated.
    private void ChemistryUITrigger()
    {
        if (StaticClasses.UIIsOpen == true)
        {
            return;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        print("ChemistryUITrigger");
        InteractableUI.SetActive(false);
        ChemistryUI.SetActive(true);
        ChemistryBackgroundRightPart.SetActive(false);
        StaticClasses.UIIsOpen = true;
    }

    //This function will deal with when a move on the MoveConstructingUI is pressed and set all the values accordingly.
    public void MoveConstructingButtonUITrigger(int MoveID)
    {
        Moves move = moves[MoveID - 1];
        DamageValueUI.text = move.damage.ToString();
        AccuracyValueUI.text = move.accuracy.ToString() + "%";
        MaterialsT1ValueUI.text = " : " + move.materials[0].ToString();
        MaterialsT2ValueUI.text = " : " + move.materials[1].ToString();
        MaterialsT3ValueUI.text = " : " + move.materials[2].ToString();
        MaterialsT4ValueUI.text = " : " + move.materials[3].ToString();
        MoveNameUI.text = move.name;
        MoveSpriteUI.sprite = move.sprite;
        MoveDescriptionUI.text = move.description;
    }

    //This function will deal with when a move on the ChemistryUI is pressed and set all the values accordingly.
    public void ChemistryButtonUITrigger(int ItemID)
    {
        Items item = items[ItemID - 1];
        if (item.type == DropDownItem.potion)
        {
            ChemistryBoostValueUI.transform.parent.gameObject.SetActive(false);
            ChemistryHealthValueUI.transform.parent.gameObject.SetActive(true);
            ChemistryHealthValueUI.text = item.healAmount.ToString();
        }
        else if (item.type == DropDownItem.boost)
        {
            ChemistryHealthValueUI.transform.parent.gameObject.SetActive(false);
            ChemistryBoostValueUI.transform.parent.gameObject.SetActive(true);
            ChemistryBoostValueUI.text = item.boostAmount.ToString();
        }
        ChemistryMaterialsT1ValueUI.text = " : " + item.materials[0].ToString();
        ChemistryMaterialsT2ValueUI.text = " : " + item.materials[1].ToString();
        ChemistryMaterialsT3ValueUI.text = " : " + item.materials[2].ToString();
        ChemistryMaterialsT4ValueUI.text = " : " + item.materials[3].ToString();
        ChemistryItemNameUI.text = item.name;
        ChemistryItemSpriteUI.sprite = item.sprite;
        ChemistryItemDescriptionUI.text = item.description;
    }

    //This function will deal with when a user is attempting to create a move.
    public void MoveConstructingButtonPurchaseUITrigger(int MoveID)
    {
        Moves move = moves[MoveID - 1];
        if (playerInventory.materials[0] >= move.materials[0] && playerInventory.materials[1] >= move.materials[1] && playerInventory.materials[2] >= move.materials[2] && playerInventory.materials[3] >= move.materials[3] && playerInventory.moveCards.Count < playerInventory.MaxSpace)
        {
            print("Can Buy");
            playerInventory.materials[0] -= move.materials[0];
            playerInventory.materials[1] -= move.materials[1];
            playerInventory.materials[2] -= move.materials[2];
            playerInventory.materials[3] -= move.materials[3];

            playerStats.GainMoveConstructingExp(move.expGiven);
            playerInventory.moveCards.Add(move);
        }
    }

    //This function will deal with when a user is attempting to create a move.
    public void ChemistryButtonPurchaseUITrigger(int ItemID)
    {
        Items item = items[ItemID - 1];
        if (playerInventory.materials[0] >= item.materials[0] && playerInventory.materials[1] >= item.materials[1] && playerInventory.materials[2] >= item.materials[2] && playerInventory.materials[3] >= item.materials[3])
        {
            if (item.type == DropDownItem.potion)
            {
                if (playerInventory.itemsPotions.Count >= playerInventory.MaxSpace)
                {
                    return;
                }
                else
                {
                    print("Can Buy");
                    playerInventory.materials[0] -= item.materials[0];
                    playerInventory.materials[1] -= item.materials[1];
                    playerInventory.materials[2] -= item.materials[2];
                    playerInventory.materials[3] -= item.materials[3];

                    playerStats.GainChemistryExp(item.expGiven);
                    playerInventory.itemsPotions.Add(item);
                }
            }
            else if (item.type == DropDownItem.boost)
            {
                if (playerInventory.itemsBoosts.Count >= playerInventory.MaxSpace)
                {
                    return;
                }
                else
                {
                    print("Can Buy");
                    playerInventory.materials[0] -= item.materials[0];
                    playerInventory.materials[1] -= item.materials[1];
                    playerInventory.materials[2] -= item.materials[2];
                    playerInventory.materials[3] -= item.materials[3];

                    playerStats.GainChemistryExp(item.expGiven);
                    playerInventory.itemsBoosts.Add(item);
                }
            }
            else if (item.type == DropDownItem.item)
            {
                if (playerInventory.items.Count >= playerInventory.MaxSpace)
                {
                    return;
                }
                else
                {
                    print("Can Buy");
                    playerInventory.materials[0] -= item.materials[0];
                    playerInventory.materials[1] -= item.materials[1];
                    playerInventory.materials[2] -= item.materials[2];
                    playerInventory.materials[3] -= item.materials[3];

                    playerStats.GainChemistryExp(item.expGiven);
                    playerInventory.items.Add(item);
                }
            }
        }
    }

    public void PlayerInventoryUpdateInventoryTopButtons(string WhatTopButton)
    {
        if (WhatTopButton == "Boosts")
        {
            for (int i = 0; i < playerInventory.MaxSpace; i++)
            {
                PlayerInventoryRightPlaceholders.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = null;
            }
            for (int i = 0; i < playerInventory.MaxSpace; i++)
            {
                if (playerInventory.itemsBoosts.Count > i)
                {
                    PlayerInventoryRightPlaceholders.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = playerInventory.itemsBoosts[i].sprite;
                }
            }
        } else if (WhatTopButton == "Potions")
        {
            for (int i = 0; i < playerInventory.MaxSpace; i++)
            {
                PlayerInventoryRightPlaceholders.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = null;
            }
            for (int i = 0; i < playerInventory.MaxSpace; i++)
            {
                if (playerInventory.itemsPotions.Count > i)
                {
                    PlayerInventoryRightPlaceholders.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = playerInventory.itemsPotions[i].sprite;
                }
            }
        } else if (WhatTopButton == "Items")
        {
            for (int i = 0; i < playerInventory.MaxSpace; i++)
            {
                PlayerInventoryRightPlaceholders.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = null;
            }
            for (int i = 0; i < playerInventory.MaxSpace; i++)
            {
                if (playerInventory.items.Count > i)
                {
                    PlayerInventoryRightPlaceholders.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = playerInventory.items[i].sprite;
                }
            }
        } else if (WhatTopButton == "MoveCards")
        {
            for (int i = 0; i < playerInventory.MaxSpace; i++)
            {
                PlayerInventoryRightPlaceholders.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = null;
            }
            for (int i = 0; i < playerInventory.MaxSpace; i++)
            {
                if (playerInventory.moveCards.Count > i)
                {
                    PlayerInventoryRightPlaceholders.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = playerInventory.moveCards[i].sprite;
                }
            }
        }
    }

    public void PlayerInventoryUpdate(string WhatPartOfInventory)
    {
        if (WhatPartOfInventory == "Inventory")
        {
            for (int i = 0; i < playerInventory.MaxSpace; i++)
            {
                PlayerInventoryRightPlaceholders.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = null;
            }
            for (int i = 0; i < playerInventory.MaxSpace; i++)
            {
                if (playerInventory.moveCards.Count > i)
                {
                    PlayerInventoryRightPlaceholders.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = playerInventory.moveCards[i].sprite;
                }
            }
        }
        else if (WhatPartOfInventory == "Monster")
        {
            PlayerInventoryMonsterSprite.sprite = playerInventory.sprite;
            PlayerInventoryMonsterID.text = playerInventory.id.ToString();
            PlayerInventoryMonsterName.text = playerInventory.name;
            PlayerInventoryMonsterDescription.text = playerInventory.description;
            PlayerInventoryMonsterDefence.text = "Defence: " + playerInventory.defence.ToString();
            PlayerInventoryMonsterAttack.text = "Attack: " + playerInventory.attack.ToString();
            PlayerInventoryMonsterSpeed.text = "Speed: " + playerInventory.speed.ToString();
            PlayerInventoryMonsterHealth.text = "Health: " + playerInventory.health.ToString();
            PlayerInventoryMonsterMaxHealth.text = "Max Health: " + playerInventory.maxHealth.ToString();
            PlayerInventoryMonsterLevel.text = "Level: " + playerInventory.level.ToString();
            if (playerInventory.monsterMoves[0] != null)
            {
                PlayerInventoryMonsterMove1.transform.GetComponentInChildren<Text>().text = "Move 1: " + playerInventory.monsterMoves[0].name;
            }
            else
            {
                PlayerInventoryMonsterMove1.transform.GetComponentInChildren<Text>().text = "Move 1: null";
            }
            if (playerInventory.monsterMoves[1] != null)
            {
                PlayerInventoryMonsterMove2.transform.GetComponentInChildren<Text>().text = "Move 2: " + playerInventory.monsterMoves[1].name;
            }
            else
            {
                PlayerInventoryMonsterMove2.transform.GetComponentInChildren<Text>().text = "Move 2: null";
            }


        }
        else if (WhatPartOfInventory == "PlayerStats")
        {


        }
        else if (WhatPartOfInventory == "GameStats")
        {


        }
    }

    public void GainQuestRewards(int id)
    {
        print("Work on gaining rewards.");
    }

    public void GainAchievement(int id)
    {
        if (achievements[id - 1].completed == false)
        {
            for (int i = 0; i < achievements[id - 1].ItemAwards.Count; i++)
            {
                if (achievements[id - 1].ItemAwards[i].type == DropDownItem.boost)
                {
                    if (playerInventory.itemsBoosts.Count < playerInventory.MaxSpace)
                    {
                        print("Adding a boost to your inventory.");
                        playerInventory.itemsBoosts.Add(achievements[id - 1].ItemAwards[i]);
                    }
                    else
                    {
                        print("Your item inventory is full!");
                    }
                }
                else if (achievements[id - 1].ItemAwards[i].type == DropDownItem.potion)
                {
                    if (playerInventory.itemsPotions.Count < playerInventory.MaxSpace)
                    {
                        print("Adding a potion to your inventory.");
                        playerInventory.itemsPotions.Add(achievements[id - 1].ItemAwards[i]);
                    }
                    else
                    {
                        print("Your item inventory is full!");
                    }
                }
                else if (achievements[id - 1].ItemAwards[i].type == DropDownItem.item)
                {  
                    if (playerInventory.items.Count < playerInventory.MaxSpace)
                    {
                        print("Adding an item to your inventory.");
                        playerInventory.items.Add(achievements[id - 1].ItemAwards[i]);
                    } else
                    {
                        print("Your item inventory is full!");
                    }
                }
                else
                {
                    print("Something went wrong when trying to add item: " + achievements[id - 1].ItemAwards[i].name + " to your inventory after a battle, item type is: " + achievements[id - 1].ItemAwards[i].type);
                }
            }

            playerStats.GainMoveConstructingExp(achievements[id - 1].ExpAwards[0]);
            playerStats.GainChemistryExp(achievements[id - 1].ExpAwards[1]);

            playerInventory.materials[0] += achievements[id - 1].MaterialAwards[0];
            playerInventory.materials[1] += achievements[id - 1].MaterialAwards[1];
            playerInventory.materials[2] += achievements[id - 1].MaterialAwards[2];
            playerInventory.materials[3] += achievements[id - 1].MaterialAwards[3];

            achievements[id - 1].completed = true;
        }
    }
}