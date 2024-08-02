using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    private void Awake()
    {
        if(instance)
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

    public void GenerateRandomOrder(bool isTimeTrial)
    {
        var order = new Order(currentRecipeBook.GetRandomRecipe());
        var visual = Instantiate(orderUIPrefab, orderContainer).GetComponent<OrderUI>();
        var newOrder = new OrderStruct(order, visual, isTimeTrial);
        orders.Add(newOrder);
    }

    public void GenerateNewOrderNoTime(Recipe newRecipe)
    {
        var order = new Order(newRecipe);
        var visual = Instantiate(orderUIPrefab, orderContainer).GetComponent<OrderUI>();
        var newOrder = new OrderStruct(order, visual,false);
        orders.Add(newOrder);
        Debug.Log(newRecipe.name);
    }
    
    public void GenerateNewOrderWithTime(Recipe newRecipe)
    {
        var order = new Order(newRecipe);
        var visual = Instantiate(orderUIPrefab, orderContainer).GetComponent<OrderUI>();
        var newOrder = new OrderStruct(order, visual,true);
        orders.Add(newOrder);
    }

    private void ManageOrder()
    {
        if (orders.Count<maxOrders)
        {
            GenerateRandomOrder(true);
            Invoke(nameof(ManageOrder),Random.Range(minTimeBetweenOrders,maxTimeBetweenOrders));
        }
        else
            Invoke(nameof(ManageOrder),2);

    }

    public bool DeliverOrder(Plate plate)
    {
        var orderIndex = orders.FindIndex(x => x.Order.Recipe.ComparePlate(plate.Ingredients) && !x.IsComplete());//Que coincida y no esté entregada
        if(orderIndex == -1) return false;
        
        orders[orderIndex].Complete();
        money += CalculateScore(plate.Ingredients);
        UpdateMoneyUI();
        return true;
    }
    
    public void FailOrder(OrderUI orderUI)
    {
        var index =orders.FindIndex(x => x.UI == orderUI);
        if(index!=-1)
            orders.RemoveAt(index);
        
    }

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

    public RecipeBook GetCurrentRecipeBook() => currentRecipeBook;
}

[Serializable]
struct OrderStruct
{
    private Order _order;
    private OrderUI _orderUI;

    public Order Order => _order;
    public OrderUI UI => _orderUI;

    // public OrderStruct(Order order, OrderUI orderUI)
    // {
    //     _order = order;
    //     _orderUI = orderUI;
    //     
    //     _orderUI.SetUp(_order.Recipe.RecipeImage, _order.Recipe.name, _order.Recipe.TimeToPrepare);
    // }
    
    public OrderStruct(Order order, OrderUI orderUI, bool isTimeTrial)
    {
        _order = order;
        _orderUI = orderUI;
        var time = isTimeTrial ? _order.Recipe.TimeToPrepare : 0;
        _orderUI.SetUp(_order.Recipe.RecipeImage, _order.Recipe.name, time);
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

   
}
