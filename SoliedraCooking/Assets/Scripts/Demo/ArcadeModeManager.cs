using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ArcadeModeManager : MonoBehaviour
{
    private static ArcadeModeManager instance;
    public static ArcadeModeManager Instance => instance;


    [SerializeField] private int maxFails = 3;
    [SerializeField] private GameObject endMenu;
    [SerializeField] private InitialTransitionController transitionController;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private int _currentFails;
    private int _totalPlatesDelivered;
    private bool _gameOver;
    public bool GameOver => _gameOver;
    private void Awake()
    {
        if (instance)
            Destroy(this);
        else
            instance = this;
    }

    public void Start()
    {
        //StartMode();
    }

    public void StartMode()
    {
        //Empieza a generar los pedidos
        OrderManager.Instance.GenerateRandomOrders();
    }

    public void FailOrder()
    {
        _currentFails++;
        
        if (_currentFails >= maxFails)
            EndGame();
        
    }

    public void CompleteOrder()
    {
        _totalPlatesDelivered++;
        //Cada 10 pedidos
        if(_totalPlatesDelivered%10 == 0)
            OrderManager.Instance.IncrementMaxOrder();
    }

    private void EndGame()
    {
        _gameOver = true;
        OrderManager.Instance.CleanOrders();
        MenuManager.Instance.ShowEndMenu(true);
        
        scoreText.text = $"Has conseguido entregar {_totalPlatesDelivered} platos. El valor total que has conseguido es de {OrderManager.Instance.GetMoney()}";

    }

    public void Exit()
    {
        StartCoroutine(transitionController.EndWithAction( () =>{ SceneManager.LoadScene("StartScene"); }));
        
    }
}
