using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractWidget : MonoBehaviour
{
    

    [SerializeField] private List<InteractIcon> icons;
    [SerializeField] private GameObject panel;

    
    //[SerializeField] private List<IngredientState> stateChangers; //estados que se pueden cambiar
    private void Start()
    {
        Hide();
    }

    public void Show(GameObject newObject)
    {
        if (newObject.TryGetComponent(out Ingredient ingredient))
        {
            Hide();
            
            if(ingredient.GetState() == IngredientState.Overcooked) return; //Si esta quemado no mostramos nada;
            
            var states = GetIngredientActions(ingredient);
            
           //Mostramos todos los que correspondan
            foreach (var state in states)
            {
                var index = icons.FindIndex(x => x.action == state);
                if (index != -1)
                {
                    icons[index].icon.SetActive(true);
                    panel.SetActive(true);
                }
                
            }
            
        }
        else if (newObject.TryGetComponent(out Pot pot))
        {
            Hide();
            if (!pot.CanCook()) return;
            var index = icons.FindIndex(x => x.action == CookAction.Cook);
            if (index != -1)
            {
                icons[index].icon.SetActive(true);
                panel.SetActive(true);
            }
        }

        //return;//Comprobamos que es un ingrediente lo que hemos depositado}
        
        
    }

    public void Hide()
    {
        panel.SetActive(false);

        //Ocultamos todos los iconos
        foreach (var item in icons)
            item.icon.SetActive(false);
    }
    private List<CookAction> GetIngredientActions(Ingredient ingredient)
    {
        var actions = new List<CookAction>();
        if(ingredient.GetIngredientInfo().CanDoAction(CookAction.Chop))
            actions.Add(CookAction.Chop);
        if(ingredient.GetIngredientInfo().CanDoAction(CookAction.Cook))
            actions.Add(CookAction.Cook);
        if(ingredient.GetIngredientInfo().CanDoAction(CookAction.Smash))
            actions.Add(CookAction.Smash);

        return actions;
    }

    
    
}
