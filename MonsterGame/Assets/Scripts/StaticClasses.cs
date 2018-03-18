using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticClasses {

    //Game Settings
    public static string AIDifficulty = "Easy";
    public static KeyCode PauseButton = KeyCode.Escape;
    public static KeyCode InventoryButton = KeyCode.E;
    public static KeyCode InteractableButton = KeyCode.F;
    public static KeyCode SprintButton = KeyCode.LeftShift;
    public static KeyCode JumpButton = KeyCode.Space;

    //Others
    public static bool IsInBattle = false;
    public static bool IsInDialogue = false;
    public static bool IsInteractable = false;
    public static bool IsGamePaused = false;
    public static string WhatInteractableItem = null;
    public static GameObject WhatInteractableItemGO = null;
    public static int BattleWins = 0;
    public static int BattleLosses = 0;
    public static int BossChance = 7;
    public static string WhoAreYouInDialogueWith = null;
    public static GameObject WhoAreYouInDialogueWithGO = null;
    public static bool CharControllerVelocity = true;
    public static bool UIIsOpen = false;
    public static bool IsInCutscene = false;
}
