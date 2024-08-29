using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private InitialTransitionController transitionController;
    private bool _playingFade;
    public void PlayTutorial()
    {
        if(_playingFade) return;
        
        _playingFade = true;
        StartCoroutine(transitionController.EndWithAction( () =>{ SceneManager.LoadScene("TutorialScene"); }));

    }

    public void PlayArcade()
    {
        if(_playingFade) return;
        
        _playingFade = true;
        StartCoroutine(transitionController.EndWithAction(() => { SceneManager.LoadScene("ArcadeModeScene"); }));

    }
}
