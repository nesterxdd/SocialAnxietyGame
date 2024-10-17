
using System;
using System.Collections;
using Controls;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BasicInteractionUI : MonoBehaviour
{
    [Header("Text")]
    public TMP_Text npcName;
    public TMP_Text npcDialogue;

    [Header("Dialogue Settings")] 
    private DialogueStage _dialogueStage;
    private bool waitingForChoice;
    private int choiceIndex;
    private MainNpcSO _mainNpc;
    private NPCContents _basicNpc;
    private int dialogueIndex;
    
    [Header("Options")]
    public GameObject choicesBox;
    public GameObject nextButton;
    public GameObject endDialogue;
    public TMP_Text optionOne;
    public TMP_Text optionTwo;
    
    [Header("Typewriter Functionality")]
    private int currentVisibleCharIndex;
    private Coroutine typewriterCoroutine;
    private WaitForSeconds simpleDelay;
    private WaitForSeconds interpunctuationDelay;

    [Header("Typewriter Settings")] 
    [SerializeField] private float charactersPerSecond ;
    [SerializeField] private float interpunctuation;

    [Header("Script References")] private UIController uiController;
    private GameProgression gameProgression;
    private PlayerPanicControl playerPanicControl;
    private void Start()
    {
        simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
        interpunctuationDelay = new WaitForSeconds(interpunctuation);
        
        uiController = UIController.instance;
        gameProgression = GameProgression.instance;
        playerPanicControl = PlayerPanicControl.instance;
        
        HideOptions();
    }
    private void Update()
    {
        if (waitingForChoice)
        {
            DisplayOptions();
        }
    }
    #region Button Functions
    /// <summary>
    /// Display the relevant dialogue progression option based on the dialogue stage enum
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void DisplayOptions()
    {
        switch (_dialogueStage)
        {
            case DialogueStage.Single:
                endDialogue.SetActive(true);
                break;
            case DialogueStage.Choice:
                choicesBox.SetActive(true);
                break;
            case DialogueStage.Main:
                nextButton.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    /// <summary>
    /// Hide the dialogue progression options
    /// </summary>
    private void HideOptions()
    {
        choicesBox.SetActive(false);
        endDialogue.SetActive(false);
        nextButton.SetActive(false);
    }
    /// <summary>
    /// Set the text in the UI to represent the choices in the array
    /// </summary>
    /// <param name="texts">Array of two choices</param>
    private void SetChoices(string[] texts)
    {
        optionOne.text = texts[0];
        optionTwo.text = texts[1];
    }
    /// <summary>
    /// Set the first dialogue choice
    /// </summary>
    public void ChoiceOne()
    {
        choiceIndex = 0;
        choicesBox.SetActive(false);
        waitingForChoice = false;
        dialogueIndex++;
        NextDialogueOption();
    }
    
    /// <summary>
    /// Set the second dialogue choice
    /// </summary>
    public void ChoiceTwo()
    {
        choiceIndex = 1;
        waitingForChoice = false;
        choicesBox.SetActive(false);
        dialogueIndex++;
        NextDialogueOption();
    }
    
    /// <summary>
    /// In non-choice dialogue proceed to the next section
    /// </summary>
    public void NextSection()
    {
        waitingForChoice = false;
        dialogueIndex++;
        NextDialogueOption();
    }
    
    /// <summary>
    /// Skips the typewriter effect
    /// </summary>
    public void Skip()
    {
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }
        npcDialogue.maxVisibleCharacters = npcDialogue.textInfo.characterCount;
        waitingForChoice = true;
    }
    #endregion

    #region Dialogue Functions
    /// <summary>
    /// Set the main npc for the next conversation 
    /// </summary>
    /// <param name="npc">Main NPCs SO</param>
    public void SetMainNpcSO(MainNpcSO npc)
    {
        _mainNpc = npc;
        npcName.text = npc.npcName;
        
        //If the NPCs branch has already been completed
        if (npc.completed)
        {
            _dialogueStage = DialogueStage.Single;
            SetDialogue(npc.onComplete);
        }
        else
        {
            dialogueIndex = 0;
            NextDialogueOption();
        }
    }
    /// <summary>
    /// If the NPC is a basic NPC start the basic conversation
    /// </summary>
    /// <param name="npc">Basic NPC SO</param>
    public void SetBasicNpcSO(NPCContents npc)
    {
        var length = npc.dialogue.Length;
        npcName.text = npc.npcName;
        switch (npc.conversationType)
        {
            case NPCContents.BasicConversationType.Single:
                
                if (length == 1)
                {
                    SetDialogue(npc.dialogue[0]);
                }
                else
                {
                    var rng = Random.Range(0, npc.dialogue.Length);
                    SetDialogue(npc.dialogue[rng]);
                }
                break;
            
            case NPCContents.BasicConversationType.Choice:

                _basicNpc = npc;
                if (npc.completed)
                {
                    SetDialogue(npc.onComplete);
                }
                else
                {
                    dialogueIndex = 0;
                    NextDialogueOption();
                }
                break;
        }
        
    }
    /// <summary>
    /// Used to determine what type of dialogue section is happening
    /// </summary>
    private enum DialogueStage
    {
        Single,
        Main,
        Choice,
    }
    /// <summary>
    /// Sets the current text to be played out in the dialogue box
    /// </summary>
    private void SetDialogue(string text)
    {
        waitingForChoice = false;
        npcDialogue.text = text;
        ResetDialogueBox();
        
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }
        typewriterCoroutine = StartCoroutine(Typewriter());
    }
    /// <summary>
    /// Set the dialogue box to its defaults
    /// </summary>
    private void ResetDialogueBox()
    {
        nextButton.SetActive(false);
        endDialogue.SetActive(false);
        npcDialogue.maxVisibleCharacters = 0;
        currentVisibleCharIndex = 0;
    }
    /// <summary>
    /// Creates the typewriter effect in the text boxes
    /// </summary>
    /// <returns>Next character in text</returns>
    private IEnumerator Typewriter()
    {
        yield return null;
        var textInfo = npcDialogue.textInfo;
        
        if (textInfo == null)
        {
            npcDialogue.ForceMeshUpdate();
            yield return null;
            textInfo = npcDialogue.textInfo;
        }
        while (currentVisibleCharIndex < textInfo.characterCount)
        {
            var character = textInfo.characterInfo[currentVisibleCharIndex].character;
            npcDialogue.maxVisibleCharacters++;

            if (character is '?' or '.' or ',' or ':' or ';' or '!' or '-')
            {
                yield return interpunctuationDelay;
            }
            else
            {
                yield return simpleDelay;
            }

            currentVisibleCharIndex++;
        }
        waitingForChoice = true;
    }
    
    /// <summary>
    /// Uses the dialogue index to see which dialogue option goes next and sets it
    /// </summary>
    private void NextDialogueOption()
    {
        if (_mainNpc != null)
        {
            switch (dialogueIndex)
            {
                case 0:
                    _dialogueStage = DialogueStage.Choice;
                    SetChoices(_mainNpc.playerIntros);
                    SetDialogue(_mainNpc.opening);
                    break;
                case 1:
                    _dialogueStage = DialogueStage.Main;
                    SetDialogue(_mainNpc.introResponses[choiceIndex]);
                    break;
                case 2:
                    SetDialogue(_mainNpc.mainDialogue);
                    break;
                case 3:
                    _dialogueStage = DialogueStage.Choice;
                    SetChoices(_mainNpc.playerResponses);
                    SetDialogue(_mainNpc.choiceDialogue);
                    break;
                case 4:
                    _dialogueStage = DialogueStage.Main;
                    SetDialogue(_mainNpc.npcResponses[choiceIndex]);
                    break;
                case 5:
                    _dialogueStage = DialogueStage.Single;
                    playerPanicControl.panicLevel += 10;
                    SetDialogue(_mainNpc.goodbye);
                
                    //Progress the game with connected scripts
                    _mainNpc.completed = true;
                    break;
                default:
                    Debug.Log("Out of bounds");
                    break;  
            }
        }
        else
        {
            switch (dialogueIndex)
            {
                case 0:
                    _dialogueStage = DialogueStage.Choice;
                    SetChoices(_basicNpc.choices);
                    SetDialogue(_basicNpc.opening);
                    break;
                case 1:
                    _dialogueStage = DialogueStage.Main;
                    SetDialogue(_basicNpc.responses[choiceIndex]);
                    break;
                case 2:
                    _dialogueStage = DialogueStage.Single;
                    SetDialogue(_basicNpc.ending);
                    _basicNpc.completed = true;
                    playerPanicControl.panicLevel += 5;
                    break;
            } 
        }
        
    }
    
    /// <summary>
    /// All the game progression functions in their scripts
    /// </summary>
    public void ProgressGame()
    {
        if (_basicNpc != null && _basicNpc.completed)
        {
            _basicNpc = null;
            playerPanicControl.MinorImprovePanicDurability();
            return;
        }
        
        if (_mainNpc == null || !_mainNpc.completed) return;
        gameProgression.EndInteraction();
        gameProgression.SetNextInteraction();
        uiController.checklistUI.UpdateChecklist(gameProgression.progressionIndex);
        uiController.notebookUI.newEntry = true;
        if (uiController.notebookUI.entryIndex < 6)
        {
            uiController.notebookUI.entryIndex++;
        }
        playerPanicControl.MainImprovePanicDurability();
        _mainNpc = null;
    }
    #endregion
}
