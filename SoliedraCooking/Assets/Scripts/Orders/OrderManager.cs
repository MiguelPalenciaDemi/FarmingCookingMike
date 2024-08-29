using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class OrderManager : MonoBehaviour
{
    private static OrderManager instance;
    public static OrderManager Instance => instance;


    [SerializeField] private float money;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private int minTimeBetweenOrders = 8;
    [SerializeField] private int maxTimeBetweenOrders = 16;
    [SerializeField] private int maxOrders = 4;
    [SerializeField] private RecipeBook currentRecipeBook;
    [SerializeField] private List<OrderStruct> orders;
    [SerializeField] private GameObject orderUIPrefab;
    [SerializeField] private Transform orderContainer;
    public RecipeBook GetCurrentRecipeBook() => currentRecipeBook;


    private void Awake()
    {
        if (instance)
            Destroy(this);
        else
            instance = this;
        orders = new List<OrderStruct>();
    }

    private void Start()
    {
        //ManageOrder();
        UpdateMoneyUI();
    }

    #region ManageOrders

    private void GenerateRandomOrder(bool isTimeTrial)
    {
        if (isTimeTrial)
            GenerateNewOrderWithTime(currentRecipeBook.GetRandomRecipe());
        else
            GenerateNewOrderNoTime(currentRecipeBook.GetRandomRecipe());
    }

    public void GenerateNewOrderNoTime(Recipe newRecipe)
    {
        var order = new Order(newRecipe);
        var visual = Instantiate(orderUIPrefab, orderContainer).GetComponent<OrderUI>();
        var newOrder = new OrderStruct(order, visual, false);
        orders.Add(newOrder);
        Debug.Log(newRecipe.name);
    }

    public void GenerateNewOrderWithTime(Recipe newRecipe)
    {
        var order = new Order(newRecipe);
        var visual = Instantiate(orderUIPrefab, orderContainer).GetComponent<OrderUI>();
        var newOrder = new OrderStruct(order, visual, true);
        orders.Add(newOrder);
    }

    public Order GenerateTutorialOrder(Recipe newRecipe)
    {
        var order = new Order(newRecipe);
        var visual = Instantiate(orderUIPrefab, orderContainer).GetComponent<OrderUI>();
        var newOrder = new OrderStruct(order, visual, true, true);
        orders.Add(newOrder);
        return newOrder.Order;
    }

    public void GenerateRandomOrders()
    {
        if(ArcadeModeManager.Instance && ArcadeModeManager.Instance.GameOver) return;
            
        if (orders.Count < maxOrders)
        {
            GenerateRandomOrder(true);
            Invoke(nameof(GenerateRandomOrders), Random.Range(minTimeBetweenOrders, maxTimeBetweenOrders));
        }
        else
        {
            Invoke(nameof(GenerateRandomOrders), 2);
            Debug.Log("no he podido generar uno nuevo");
            
        }
    }

    public void CleanOrders()
    {
        foreach (var order in orders.ToList())
        {
            order.Destroy();
            orders.Remove(order);
        }
    }

    #endregion

    #region FinishOrder

    public bool DeliverOrder(Plate plate)
    {
        var orderIndex =
            orders.FindIndex(x =>
                x.Order.Recipe.ComparePlate(plate.Ingredients) && !x.IsComplete()); //Que coincida y no esté entregada
        if (orderIndex == -1) return false;

        orders[orderIndex].Complete();
        money += CalculateScore(plate.Ingredients);
        UpdateMoneyUI();
        
        //Comprobamos si estamos en Modo Arcade
        if (ArcadeModeManager.Instance)
            ArcadeModeManager.Instance.CompleteOrder();
        
        orders.RemoveAt(orderIndex);


        return true;
    }

    public void FailOrder(OrderUI orderUI)
    {
        var index = orders.FindIndex(x => x.UI == orderUI);
        if (index == -1) return;

        // //En caso de que no sea una order que se repite nos dará true por la que podremos eliminarla de la lista
        // if (!orders[index].Fail()) return;
        orders[index].Fail();
        //Destroy(orders[index].UI.gameObject);//Destruimos ese gameobject
        orders.RemoveAt(index);
    }
    #endregion

    public void IncrementMaxOrder()
    {
        maxOrders++;
    }

    #region Score
    private float CalculateScore(List<IngredientInfo> ingredients)
    {
        var score = 0f;
        foreach (var item in ingredients)
        {
            score += item.Price;
        }

        return score;
    }

    private void UpdateMoneyUI()
    {
        moneyText.text = money.ToString();
    }

    public float GetMoney() => money;

    #endregion
}

[Serializable]
struct OrderStruct
{
    private Order _order;
    private OrderUI _orderUI;
    private bool _isTutorial;
    public Order Order => _order;
    public OrderUI UI => _orderUI;


    public OrderStruct(Order order, OrderUI orderUI, bool isTimeTrial, bool isLoop = false)
    {
        _order = order;
        _orderUI = orderUI;
        var time = isTimeTrial ? _order.Recipe.TimeToPrepare : 0;
        _orderUI.SetUp(_order.Recipe.RecipeImage, _order.Recipe.name, time);
        _isTutorial = isLoop;
    }

    public void Fail()
    {
        if (_isTutorial)
            TutorialManager.Instance.RestartFailTutorialBox();

        //Comprobamos que estamos en el modo Arcade => ArcadeMode.Instance != null
        if (ArcadeModeManager.Instance)
            ArcadeModeManager.Instance.FailOrder();
    }

    public void Complete()
    {
        _order.Complete();
        _orderUI.Complete();
    }

    public bool IsComplete()
    {
        return _order.IsDone;
    }

    public void Destroy()
    {
        _order = null;
        _orderUI.Clean();
        
    }
}