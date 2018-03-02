using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public GameObject Player;
    public Dialogue DefaultDialogue;
    public Dialogue DefaultDialogueChoice1;
    public Dialogue DefaultDialogueChoice2;
    public Dialogue DefaultDialogueChoice3;
    public Dialogue TestQuest_1;
    public Dialogue TestQuest_2;

    public void TriggerDialogueChoices(string DialogueTable)
    {
        Dialogue DialogueChoice = (Dialogue)this.GetType().GetField(DialogueTable).GetValue(this);
        DialogueManager dialogueManager = Player.GetComponent<DialogueManager>();
        dialogueManager.StartDialogue(DialogueChoice, this.gameObject);
    }

    public void TriggerDialogue()
    {
        PlayerScript playerScript = Player.GetComponent<PlayerScript>();
        DialogueManager dialogueManager = Player.GetComponent<DialogueManager>();
        if (playerScript.quests[0].IsStarted == true && playerScript.quests[0].IsCompleted == false && playerScript.quests[0].CurrentPart == 1 && TestQuest_1.sentences.Count > 0)
        {
            dialogueManager.StartDialogue(TestQuest_1, this.gameObject);
        } else if (playerScript.quests[0].IsStarted == true && playerScript.quests[0].IsCompleted == false && playerScript.quests[0].CurrentPart == 2 && TestQuest_2.sentences.Count > 0)
        {
            dialogueManager.StartDialogue(TestQuest_2, this.gameObject);
        } else
        {
            dialogueManager.StartDialogue(DefaultDialogue, this.gameObject);
        }

    }
}
