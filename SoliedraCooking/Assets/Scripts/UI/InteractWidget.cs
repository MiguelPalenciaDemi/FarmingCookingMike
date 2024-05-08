using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractWidget : MonoBehaviour
{
    [Serializable]
    public struct InteractIcon
    {
        public GameObject icon;
        public IngredientState state;
    }

    [SerializeField] private List<InteractIcon> icons;
    [SerializeField] private GameObject panel;

    
    //[SerializeField] private List<IngredientState> stateChangers; //estados que se pueden cambiar
    private void Start()
    {
        Hide();
    }

    public void Show(GameObject newObject)
    {
        if (!newObject.TryGetComponent(out Ingredient ingredient)) return;//Comprobamos que es un ingrediente lo que hemos depositado
        
        Hide();
        
        if(ingredient.GetState() == IngredientState.Overcooked) return; //Si esta quemado no mostramos nada;
        
        var states = GetIngredientStates(ingredient);
        
       //Mostramos todos los que correspondan
        foreach (var state in states)
        {
            panel.SetActive(true);
            var index = icons.FindIndex(x => x.state == state);
            if(index != -1)
                icons[index].icon.SetActive(true);
        }
    }

    public void Hide()
    {
        panel.SetActive(false);

        //Ocultamos todos los iconos
        foreach (var item in icons)
            item.icon.SetActive(false);
    }
    private List<IngredientState> GetIngredientStates(Ingredient ingredient)
    {
        var states = new List<IngredientState>();
        if(ingredient.GetIngredientInfo().IsChoppable && ingredient.GetState() != IngredientState.Chopped)
            states.Add(IngredientState.Chopped);
        if(ingredient.GetIngredientInfo().IsSmashable && ingredient.GetState() != IngredientState.Smashed )
            states.Add(IngredientState.Smashed);
        if(ingredient.GetIngredientInfo().TimeCooking>0)
            states.Add(IngredientState.Cooked);

        return states;
    }

    
    
}
