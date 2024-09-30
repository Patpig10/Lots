using UnityEngine;

namespace DS
{
    using ScriptableObjects;

    public class DSDialogue : MonoBehaviour
    {
        /* Dialogue Scriptable Objects */
        [SerializeField] private DSDialogueContainerSO dialogueContainer;
        [SerializeField] private DSDialogueGroupSO dialogueGroup;
        [SerializeField] private DSDialogueSO dialogue;
        [SerializeField] private DSDialogueSO name;



        /* Filters */
        [SerializeField] private bool groupedDialogues;
        [SerializeField] private bool startingDialoguesOnly;

        /* Indexes */
        [SerializeField] private int selectedDialogueGroupIndex;
        [SerializeField] private int selectedDialogueIndex;
    }
}