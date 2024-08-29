using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image progress;
    [SerializeField] private GameObject timeBarContainer;
    [SerializeField] private TextMeshProUGUI textRecipe;
    [SerializeField] private GameObject completeUI;
    private Animator _animator;
    private bool _started;
    private bool _completed;
    private float _timer;
    private float _timeToComplete;
    private bool _noTime;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _timer = 0;
    }

    public void Complete()
    {
        _completed = true;
        completeUI.SetActive(true);
        _animator.SetTrigger("Complete");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="spriteRecipe"></param>
    /// <param name="nameRecipe"></param>
    /// <param name="time">Si quieres que no tenga un tiempo para ser completada debemos darle un valor negativo o igual a 0</param>
    public void SetUp(Sprite spriteRecipe,string nameRecipe, float time)
    {
        image.sprite = spriteRecipe;
        textRecipe.text = nameRecipe;
        _timeToComplete = time;
        _noTime = time <= 0; //si le pasamos un valor igual a 0 o menor, la receta no será con tiempo
        if(_noTime)
            timeBarContainer.SetActive(false);
    }

    private void Update()
    {
        if(!_noTime && _started && !_completed)
        {

            _timer += Time.deltaTime;
            UpdateUI();
            if (_timer >= _timeToComplete)
            {
                Fail();
            }
        }
    }

    public void Delete()
    {
        if(!_completed)
            OrderManager.Instance.FailOrder(this);
        
        Destroy(gameObject);
    }

    public void ResetUI()
    {
        _animator.SetTrigger("Reset");
        _timer = 0;
        progress.fillAmount = 0;
    }
    
    private void Fail()
    {
        Debug.Log("Faaail");
        _animator.SetTrigger("Fail");
        _started = false;
    }

    private void UpdateUI()
    {
        progress.fillAmount = _timer / _timeToComplete;
    }

    public void StartTimer()
    {
        _started = true;
    }

    public void Clean()
    {
        _completed = true;
        Fail();
    }
}
