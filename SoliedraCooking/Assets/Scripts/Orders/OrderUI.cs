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
    [SerializeField] private TextMeshProUGUI textRecipe;
    [SerializeField] private GameObject completeUI;
    private Animator _animator;
    private bool _started;
    private bool _completed;
    private float _timer;
    private float _timeToComplete;

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

    public void SetUp(Sprite spriteRecipe,string nameRecipe, float time)
    {
        image.sprite = spriteRecipe;
        textRecipe.text = nameRecipe;
        _timeToComplete = time;
    }

    private void Update()
    {
        if(_started && !_completed)
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
        OrderManager.Instance.FailOrder(this);
        Destroy(gameObject);
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
}
