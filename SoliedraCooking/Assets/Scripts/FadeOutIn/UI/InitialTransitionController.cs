using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InitialTransitionController : MonoBehaviour
{
    [Header("Pre Animation")] 
    [SerializeField] private bool playOnAwake;
    [SerializeField] private float delayStart;//Time to wait until we call the transition;
    
    [Header("Animation Properties")]
    [SerializeField] private Image circle;
    [SerializeField] private float timeTransition;
    
    [Header("Aspect Ratio SetUp")]
    [SerializeField] private Image imageBackground;

    private float maxRadius; //Radius to cover the whole screen
    private float currentRadius; 
    private float timer;//time since animation started
    private RectTransform cavasRectTransform;
    
    private void Awake()
    {
        cavasRectTransform = GetComponent<RectTransform>();
        circle.rectTransform.sizeDelta = Vector2.zero;//WE SHOULD DO THIS IN EDITOR
    }

    private void Start()
    {
        SetUpCorrectResolution();
        if(playOnAwake)
            Invoke(nameof(StartFadeIn),delayStart);
    }
    
    public void InitAnimation()
    {
        SetUpCorrectResolution();//Call here in case resolution would have changed since Start
        currentRadius = 0;
        timer = 0;
        circle.rectTransform.sizeDelta = Vector2.zero;//width = 0 && height = 0;
    }

    private IEnumerator FadeIn()
    {
        InputManager.Instance.ActiveControl(false);

        while (timer<timeTransition)
        {
            timer += Time.deltaTime;
            currentRadius = maxRadius * (timer/ timeTransition);
            circle.rectTransform.sizeDelta = new Vector2(currentRadius, currentRadius); //(width,height)
            yield return null;
        }
        timer = 0;
        InputManager.Instance.ActiveControl(true);

    }
    
    private IEnumerator FadeOut()
    {
        //_inputManager.ActiveControl(false);
        InputManager.Instance.ActiveControl(false);
        
        while (timer<timeTransition)
        {
            timer += Time.deltaTime;
            currentRadius = (maxRadius * timer) / timeTransition;
            circle.rectTransform.sizeDelta = new Vector2(maxRadius - currentRadius, maxRadius - currentRadius); //(width,height)
            yield return null;
        }

        timer = 0;

    }

    private void SetUpCorrectResolution()
    {
        var rect = cavasRectTransform.rect;
        imageBackground.rectTransform.sizeDelta = new Vector2( rect.width, rect.height);
        maxRadius = rect.width + rect.width*0.4f; // + rect.width*0.4f to add the border space
        
        
    }

    public void StartFadeOut()
    {
        InitAnimation();
        StartCoroutine(nameof(FadeOut));
    }

    public void StartFadeIn()
    {
        InitAnimation();
        StartCoroutine(nameof(FadeIn));
    }

    public IEnumerator RestartWithAction(System.Action callback)
    {
        InitAnimation();
        yield return StartCoroutine(nameof(FadeOut));
        callback?.Invoke();
        InitAnimation();        
        yield return StartCoroutine(nameof(FadeIn));
        
    }
    
    public IEnumerator EndWithAction(System.Action callback)
    {
        Debug.Log("hey empiezo la animacion");
        yield return StartCoroutine(nameof(FadeOut));
        callback?.Invoke();
    }
}
