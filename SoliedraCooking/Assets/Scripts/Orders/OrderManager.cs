using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderManager : MonoBehaviour
{
    private static OrderManager instance;
    public static OrderManager Instance => instance;
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
    }

    private void Start()
    {
        orders = new List<OrderStruct>();
        ManageOrder();
    }

    public void GenerateNewOrder()
    {
        var order = new Order(currentRecipeBook.GetRandomRecipe());
        var visual = Instantiate(orderUIPrefab, orderContainer).GetComponent<OrderUI>();
        var newOrder = new OrderStruct(order, visual);
        orders.Add(newOrder);
    }

    private void ManageOrder()
    {
        if (orders.Count<maxOrders)
        {
            GenerateNewOrder();
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
        return true;
    }
    
    public void FailOrder(OrderUI orderUI)
    {
        var index =orders.FindIndex(x => x.UI == orderUI);
        if(index!=-1)
            orders.RemoveAt(index);
        
    }

    private void CalculateScore(List<IngredientInfo> ingredients)
    {
        var score = 0f;
        foreach (var item in ingredients)
        {
            score += item.Price;
        }
    }
}

[Serializable]
struct OrderStruct
{
    private Order _order;
    private OrderUI _orderUI;

    public Order Order => _order;
    public OrderUI UI => _orderUI;

    public OrderStruct(Order order, OrderUI orderUI)
    {
        _order = order;
        _orderUI = orderUI;
        
        _orderUI.SetUp(_order.Recipe.RecipeImage, _order.Recipe.name, _order.Recipe.TimeToPrepare);
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
