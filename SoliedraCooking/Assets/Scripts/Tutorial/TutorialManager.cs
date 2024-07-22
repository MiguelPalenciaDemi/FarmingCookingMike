using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager _instance;
    public static TutorialManager Instance => _instance;
    [SerializeField] private TutorialBox[] tutorialBoxes;
    private Queue<TutorialBox> tutorialBoxesQueue;

    private void Awake()
    {
        if (_instance)
            Destroy(this);
        else
            _instance = this;

        tutorialBoxesQueue = new Queue<TutorialBox>();
        foreach (var tutorialBox in tutorialBoxes)
        {
            tutorialBoxesQueue.Enqueue(tutorialBox);
        }
    }

    private void Start()
    {
        ShowNext();
    }

    private void ShowNext()
    {
        if (tutorialBoxesQueue.Count>0)
            tutorialBoxesQueue.Peek().gameObject.SetActive(true);
    }

    public void CompleteTutorial()
    {
        tutorialBoxesQueue.Dequeue().gameObject.SetActive(false);
        ShowNext();

    }
}
