using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    private Recipe _recipe;
    private bool _isDone;
    public bool IsDone => _isDone;
    public Recipe Recipe => _recipe;

    public Order(Recipe recipe)
    {
        _recipe = recipe;
    }
    
    public void Complete()
    {
        _isDone = true;
    }
    
    
}
