using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    private Queue<DialogueAndCheckIfPlayer> sentences;
    public GameObject DialogueUI;
    public GameObject DialogueChoicesUI;
    private Text DialogueText;
    private Image DialogueImage;
    private Sprite playerSprite;
    private PlayerScript playerScript;

	// Use this for initialization
	void Start () {
        sentences = new Queue<DialogueAndCheckIfPlayer>();
        DialogueText = DialogueUI.transform.GetChild(1).GetComponent<Text>();
        DialogueImage = DialogueUI.transform.GetChild(2).GetComponent<Image>();
        playerScript = GetComponent<PlayerScript>();
        playerSprite = playerScript.playerSprite;
    }

    public void StartDialogue(Dialogue dialogue, GameObject dialogueObject)
    {
        print("Starting convo with: " + dialogue.name);
        DialogueChoicesUI.SetActive(false);
        DialogueUI.SetActive(true);
        StaticClasses.WhoAreYouInDialogueWith = dialogue.name;
        StaticClasses.WhoAreYouInDialogueWithGO = dialogueObject;

        sentences.Clear();

        foreach (DialogueAndCheckIfPlayer sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        if (StaticClasses.WhoAreYouInDialogueWithGO.transform.childCount > 1)
        {
            Animator OtherAnimator = StaticClasses.WhoAreYouInDialogueWithGO.transform.GetChild(1).gameObject.GetComponent<Animator>();
                OtherAnimator.SetBool("IsTalking", true);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueAndCheckIfPlayer sentence = sentences.Dequeue();
        if (sentence.IsPlayer == true)
        {
            if (sentence.sentence.Contains("QUEST_START_"))
            {
                string QuestID = sentence.sentence.Replace("QUEST_START_", "");
                int ActualQuestID = 0;
                if (Int32.TryParse(QuestID, out ActualQuestID))
                {
                    playerScript.StartQuest(ActualQuestID);
                    EndDialogue();
                    return;
                }
            } else if (sentence.sentence.Contains("QUEST_ADVANCE_"))
            {
                string QuestID = sentence.sentence.Replace("QUEST_ADVANCE_", "");
                int ActualQuestID = 0;
                if (Int32.TryParse(QuestID, out ActualQuestID))
                {
                    playerScript.AdvanceQuest(ActualQuestID);
                    EndDialogue();
                    return;
                }
            } else if (sentence.sentence.Contains("QUEST_COMPLETE_"))
            {
                string QuestID = sentence.sentence.Replace("QUEST_COMPLETE_", "");
                int ActualQuestID = 0;
                if (Int32.TryParse(QuestID, out ActualQuestID))
                {
                    playerScript.CompleteQuest(ActualQuestID);
                    EndDialogue();
                    return;
                }
            } else if (sentence.sentence.Contains("DIALOGUE_CHOICES_")) {
                string Choice1;
                string Choice2;
                string ChoiceName = sentence.sentence.Replace("DIALOGUE_CHOICES_", "");
                string FirstChoiceNumber;
                string SecondChoiceNumber;
                DialogueAndCheckIfPlayer Choice1Sentence = sentences.Dequeue();
                Choice1 = Choice1Sentence.sentence;
                DialogueAndCheckIfPlayer Choice2Sentence = sentences.Dequeue();
                Choice2 = Choice2Sentence.sentence;
                DialogueAndCheckIfPlayer Choice3Sentence = sentences.Dequeue();
                FirstChoiceNumber = Choice3Sentence.sentence;
                DialogueAndCheckIfPlayer Choice4Sentence = sentences.Dequeue();
                SecondChoiceNumber = Choice4Sentence.sentence;

                //Set button onClick function.
                DialogueChoicesUI.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate { StaticClasses.WhoAreYouInDialogueWithGO.GetComponent<DialogueTrigger>().TriggerDialogueChoices(ChoiceName + "Choice" + FirstChoiceNumber); });
                DialogueChoicesUI.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(delegate { StaticClasses.WhoAreYouInDialogueWithGO.GetComponent<DialogueTrigger>().TriggerDialogueChoices(ChoiceName + "Choice" + SecondChoiceNumber); });

                //Set actives and texts for choices.
                DialogueChoicesUI.SetActive(true);
                DialogueUI.SetActive(false);
                DialogueChoicesUI.transform.GetChild(4).gameObject.GetComponent<Text>().text = Choice1;
                DialogueChoicesUI.transform.GetChild(5).gameObject.GetComponent<Text>().text = Choice2;
            } else
            {
                DialogueImage.sprite = playerSprite;
            }
        } else
        {
            if (sentence.sprite)
            {
                DialogueImage.sprite = sentence.sprite;
            } else
            {
                DialogueImage.sprite = null;
            }          
        }
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(DialogueAndCheckIfPlayer sentence)
    {
        if (sentence.IsPlayer == true)
        {
            DialogueText.text = "Player: ";
        }
        else
        {
            DialogueText.text = StaticClasses.WhoAreYouInDialogueWith + ": " ;
        }
        foreach (char letter in sentence.sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        DialogueUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        if (StaticClasses.WhoAreYouInDialogueWithGO.transform.childCount > 1)
        {
            Animator OtherAnimator = StaticClasses.WhoAreYouInDialogueWithGO.transform.GetChild(1).gameObject.GetComponent<Animator>();
            OtherAnimator.SetBool("IsTalking", false);
        }
        StaticClasses.WhoAreYouInDialogueWith = null;
        StaticClasses.WhoAreYouInDialogueWithGO = null;
        StaticClasses.IsInDialogue = false;
    }
}
