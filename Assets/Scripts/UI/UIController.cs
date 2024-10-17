
using Controls;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    
    [Header("Player")]
    [SerializeField] private PlayerController player;
    private PlayerPanicControl playerPanicControl;

    [Header("Page Opening")] 
    private bool pageOpen;
    private int pageIndex;
    
    [Header("Pages")]
    public GameObject[] interactionElements;
    public GameObject panicMetreDisplay;
    public GameObject notebookDisplay;
    public GameObject gameIconDisplay;
    public GameObject pauseMenuDisplay;
    public GameObject checklistDisplay;
    public GameObject mapDisplay;
    public GameObject tutorialDisplay;
    
    [Header("Element Scripts")]
    public InteractUI interactUI;
    public BasicInteractionUI basicIntUI;
    public NotebookUI notebookUI;
    public ChecklistUI checklistUI;
    public TutorialController tutorialUI;

    [Header("Animation")] 
    private readonly LeanTweenType leanTypeIn = LeanTweenType.easeInCubic;
    private readonly LeanTweenType leanTypeOut = LeanTweenType.easeOutCubic;
    
    [Header("UI Transforms")]
    [SerializeField] private Transform[] panicMetrePositions;
    [SerializeField] private Transform[] gameIconPositions;
    [SerializeField] private Transform[] notebookPositions;
    [SerializeField] private Transform[] pauseMenuPositions;
    [SerializeField] private Transform[] dialoguePositions;
    [SerializeField] private Transform[] interactPositions;
    private readonly float transitionTime = 0.1f;
    
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerPanicControl = PlayerPanicControl.instance;
        HideInteractionElements();
        HideNotebook();
        HidePauseMenu();
        HideChecklist();
        HideTutorial();
        MoveElementsIn();
    }
    
    /// <summary>
    /// When keys are pressed open and close the UI based on the index
    /// </summary>
    public void OpenCloseUI()
    {
        if (pageOpen)
        {
            pageOpen = false;
            switch (pageIndex)
            {
                case 0:
                    HidePauseMenu();
                    break;
                case 1:
                    HideNotebook();
                    break;
                case 2:
                    HideChecklist();
                    break;
                case 3: 
                    HideMap();
                    break;
            }
            
            if(!player._closestNpc) return;
            
            if (player._closestNpc.mainContents != null)
            {
                ShowInteract(player._closestNpc.mainContents);
            }
            else
            {
                ShowInteract(player._closestNpc.contents);
            }
        }
        else
        {
            HideInteract();
            pageOpen = true;
            switch (pageIndex)
            {
                case 0:
                    ShowPauseMenu();
                    break;
                case 1:
                    ShowNotebook();
                    break;
                case 2:
                    ShowChecklist();
                    break;
                case 3: 
                    ShowMap();
                    break;
            }
        }
    }

    public void SetPageIndex(int index)
    {
        if(pageOpen)
            return;
        
        pageIndex = index;
    }
    public void DisplayHidePage(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OpenCloseUI();
        }
    }

    public void ShowTutorial()
    {
        tutorialDisplay.SetActive(true);
        tutorialUI.StartTutorial();
        tutorialDisplay.transform.position = notebookPositions[0].transform.position;
        LeanTween.moveY(tutorialDisplay, notebookPositions[1].position.y, transitionTime);
    }

    public void HideTutorial()
    {
        tutorialUI.EndTutorial();
        tutorialDisplay.SetActive(false);
    }
    #region Interactions
    /// <summary>
    /// Show the interact pop up
    /// </summary>
    /// <param name="npc"></param>
    public void ShowInteract(NPCContents npc)
    {
        if(pageOpen) return;
        
        if (!interactionElements[1].activeInHierarchy)
        {
            interactionElements[0].SetActive(true);
        }
        interactionElements[0].transform.position = interactPositions[0].transform.position;
        LeanTween.moveY(interactionElements[0], interactPositions[1].position.y, transitionTime);
        interactUI.npcName.text = npc.npcName;
        interactUI.interactKey.text = "[E]";
    }
    
    /// <summary>
    /// Overload show option for if it is a main NPC
    /// </summary>
    /// <param name="npc"></param>
    public void ShowInteract(MainNpcSO npc)
    {
        if (pageOpen) return;
        
        if (!interactionElements[1].activeInHierarchy)
        {
            interactionElements[0].SetActive(true);
        }
        interactionElements[0].transform.position = interactPositions[0].transform.position;
        LeanTween.moveY(interactionElements[0], interactPositions[1].position.y, transitionTime);
        interactUI.npcName.text = npc.npcName;
        interactUI.interactKey.text = "[E]";
    }
    /// <summary>
    /// Hides the interact pop up
    /// </summary>
    public void HideInteract()
    {
        interactionElements[0].SetActive(false);
    }
    
    /// <summary>
    /// Hide all elements of the UI
    /// </summary>
    private void HideInteractionElements()
    {
        foreach (GameObject page in interactionElements)
        {
            page.SetActive(false);
        }
        notebookDisplay.SetActive(false);
        pauseMenuDisplay.SetActive(false);
    }
    #endregion

    #region Dialogue
    
    /// <summary>
    /// Show the basic dialogue pop up
    /// </summary>
    /// <param name="npc"></param>
    public void ShowBasicDialogue(NPCContents npc)
    {
        playerPanicControl.isPaused = true;
        HideInteract();
        MoveElementsOut();
        interactionElements[1].SetActive(true);
        basicIntUI.SetBasicNpcSO(npc);
        interactionElements[1].transform.position = dialoguePositions[0].transform.position;
        LeanTween.moveY(interactionElements[1], dialoguePositions[1].position.y, transitionTime);
    }
    
    /// <summary>
    /// Hide the basic dialogue pop up
    /// </summary>
    public void HideBasicDialogue()
    {
        playerPanicControl.isPaused = false;
        interactionElements[1].SetActive(false);
        MoveElementsIn();
        player.ResumeMovement();
        if(player.ClosestNpc != null)
        {
            player.ClosestNpc.talking = false;
        }
        interactionElements[0].SetActive(true);
    }
    
    /// <summary>
    /// Start dialogue with a main NPC
    /// </summary>
    /// <param name="npc">NPC to interact with</param>
    public void ShowAdvancedDialogue(MainNpcSO npc)
    {
        playerPanicControl.isPaused = true;
        HideInteract();
        MoveElementsOut();
        basicIntUI.SetMainNpcSO(npc);
        interactionElements[1].SetActive(true);
        interactionElements[1].transform.position = dialoguePositions[0].transform.position;
        LeanTween.moveY(interactionElements[1], dialoguePositions[1].position.y, transitionTime);
    }
    
    #endregion

    #region Pop Ups
    /// <summary>
    /// Show the panic metre and hide the notebook
    /// </summary>
    public void HideNotebook()
    {
        playerPanicControl.isPaused = false;
        MoveElementsIn();
        notebookDisplay.SetActive(false);
    }
    /// <summary>
    /// Show the notebook and hide all other UI elements
    /// </summary>
    public void ShowNotebook()
    {
        notebookUI.newEntry = false;
        playerPanicControl.isPaused = true;
        MoveElementsOut();
        notebookDisplay.SetActive(true);
        notebookDisplay.transform.position = notebookPositions[0].transform.position;
        LeanTween.moveY(notebookDisplay, notebookPositions[1].position.y, transitionTime);
        notebookUI.SetNotebookEntries();
    }
    
    /// <summary>
    /// Hide the pause menu
    /// </summary>
    public void HidePauseMenu()
    {
        playerPanicControl.isPaused = false;
        pauseMenuDisplay.SetActive(false);
        MoveElementsIn();
    }
    
    /// <summary>
    /// Move top elements into the scene and pop up pause menu
    /// </summary>
    public void ShowPauseMenu()
    {
        playerPanicControl.isPaused = true;
        MoveElementsOut();
        pauseMenuDisplay.SetActive(true);
        pauseMenuDisplay.transform.position = pauseMenuPositions[0].transform.position;
        LeanTween.moveY(pauseMenuDisplay, pauseMenuPositions[1].position.y, transitionTime);
    }
    
    /// <summary>
    /// Show the checklist UI
    /// </summary>
    public void ShowChecklist()
    {
        playerPanicControl.isPaused = true;
        MoveElementsOut();
        checklistDisplay.SetActive(true);
        checklistDisplay.transform.position = pauseMenuPositions[0].transform.position;
        LeanTween.moveY(checklistDisplay, pauseMenuPositions[1].position.y, transitionTime);
    }
    
    /// <summary>
    /// Hide the checklist UI
    /// </summary>
    public void HideChecklist()
    {
        playerPanicControl.isPaused = false;
        checklistDisplay.SetActive(false);
        MoveElementsIn();
    }
    
    /// <summary>
    /// Display the Map
    /// </summary>
    public void ShowMap()
    {
        playerPanicControl.isPaused = true;
        MoveElementsOut();
        mapDisplay.SetActive(true);
        mapDisplay.transform.position = pauseMenuPositions[0].transform.position;
        LeanTween.moveY(mapDisplay, pauseMenuPositions[1].position.y, transitionTime);
    }
    
    /// <summary>
    /// Hide the Map display
    /// </summary>
    public void HideMap()
    {
        playerPanicControl.isPaused = false;
        mapDisplay.SetActive(false);
        MoveElementsIn();
    }
    #endregion
    
    #region Animation
    /// <summary>
    /// Push the top UI elements out of the scene
    /// </summary>
    private void MoveElementsOut()
    {
        LeanTween.moveX(panicMetreDisplay, panicMetrePositions[1].position.x, transitionTime).setEase(leanTypeOut);
        LeanTween.moveX(gameIconDisplay, gameIconPositions[1].position.x, transitionTime).setEase(leanTypeOut);
    }
    
    /// <summary>
    /// Bring the elements into the scene
    /// </summary>
    private void MoveElementsIn()
    {
        LeanTween.moveX(panicMetreDisplay, panicMetrePositions[0].position.x, transitionTime).setEase(leanTypeIn);
        LeanTween.moveX(gameIconDisplay, gameIconPositions[0].position.x, transitionTime).setEase(leanTypeIn);
    }
    #endregion
    
}
