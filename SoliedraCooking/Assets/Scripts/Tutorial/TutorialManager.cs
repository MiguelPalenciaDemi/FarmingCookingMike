using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager _instance;
    public static TutorialManager Instance => _instance;
    private Queue<TutorialBox> _tutorialBoxesQueue;
    [SerializeField] private TutorialBox[] tutorialBoxes;
    [SerializeField] private UnityEvent startAction;

    [Header("Restart TutorialBox")] 
    [SerializeField] private Workstation[] workstations;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private InitialTransitionController transitionController;
    

    private void Awake()
    {
        if (_instance)
            Destroy(this);
        else
            _instance = this;

        _tutorialBoxesQueue = new Queue<TutorialBox>();
        foreach (var tutorialBox in tutorialBoxes)
        {
            _tutorialBoxesQueue.Enqueue(tutorialBox);
        }
    }

    private void Start()
    {
        startAction.Invoke();
        ShowNext();
    }

    private void ShowNext()
    {
        if (_tutorialBoxesQueue.Count>0)
            _tutorialBoxesQueue.Peek().gameObject.SetActive(true);
    }

    public void CompleteTutorial()
    {
        _tutorialBoxesQueue.Dequeue().gameObject.SetActive(false);
        ShowNext();
    }

    public void RestartFailTutorialBox()
    {
        StartCoroutine(transitionController.RestartWithAction(RestartCurrentTutorialBox));
    }

    private void RestartCurrentTutorialBox()
    {
        foreach (var workstation in workstations)
            workstation.RestartWorkstation();

        if (playerInteract.ObjectPickedUp)
            Destroy(playerInteract.ObjectPickedUp);

        var currentTutorialBox = _tutorialBoxesQueue.Peek();
        
        playerInteract.transform.position = currentTutorialBox.transform.position;
        currentTutorialBox.RestartTutorial();
    }
}
