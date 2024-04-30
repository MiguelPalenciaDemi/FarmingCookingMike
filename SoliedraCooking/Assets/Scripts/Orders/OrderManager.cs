using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private static OrderManager instance;
    public static OrderManager Instance => instance;
    
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
    }

    public void GenerateNewOrder()
    {
        var order = new Order(currentRecipeBook.GetRandomRecipe());
        var visual = Instantiate(orderUIPrefab, orderContainer).GetComponent<OrderUI>();
        var newOrder = new OrderStruct(order, visual);
        orders.Add(newOrder);
    }

    public bool DeliverOrder(Plate plate)
    {
        var orderIndex = orders.FindIndex(x => x.Order.Recipe.ComparePlate(plate.Ingredients) && !x.IsComplete());//Que coincida y no esté entregada
        if(orderIndex == -1) return false;
        
        orders[orderIndex].Complete();
        return true;
    }
}


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
        
        _orderUI.SetUp(_order.Recipe.RecipeImage, _order.Recipe.name);
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
