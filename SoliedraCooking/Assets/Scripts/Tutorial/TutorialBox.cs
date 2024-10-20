using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum ConditionType
{
    Ingredient, Object, Recipe, Destination, OrderInTime
}
[Serializable]
public struct TutorialCondition
{
    [SerializeField] private GameObject target;
    [SerializeField] private IngredientInfo desired;
    [SerializeField] private string conditionMessage;
    public string ConditionMessage => conditionMessage;

    public ConditionType GetConditionType => conditionType;

    public PointCondition GetPointCondition => pointCondition;

    [SerializeField] private ConditionType conditionType;
    [SerializeField] private string prefabCompare;
    [SerializeField] private Recipe recipeCompare;
    [SerializeField] private PointCondition pointCondition;
    private Order _orderCondition;

    public void Start()
    {
        if (conditionType == ConditionType.OrderInTime)
        {
            _orderCondition = OrderManager.Instance.GenerateTutorialOrder(recipeCompare);
        }
        else if(conditionType == ConditionType.Destination && pointCondition)
            pointCondition.gameObject.SetActive(false);
    }

    public bool IsComplete()
    {
        PlayerInteract player;
        Workstation workstation;
        Ingredient ingredient;
        Plate plate;
        Pot pot;
        
        switch (conditionType)
        {
            case ConditionType.Ingredient:
                //En caso de que el target sea el player
                if (target.TryGetComponent(out player))
                {
                    //Comprobamos que tenemos un ingrediente en la mano, si es asi comprobamos si es lo que deseamos.
                    if (player.ObjectPickedUp && player.ObjectPickedUp.TryGetComponent(out ingredient))
                        return ingredient.GetIngredientInfo() == desired;
                }
                else if (target.TryGetComponent(out workstation)) //En caso de que el target sea una mesa de trabajo
                {
                    //Comprobamos que hay un ingrediente puesto en la mesa
                    if(workstation.ObjectInWorktop && workstation.ObjectInWorktop.TryGetComponent(out ingredient))
                        return ingredient.GetIngredientInfo() == desired;
                }
                break;

            case ConditionType.Object:
            {
                if (target.TryGetComponent(out player))
                {
                    //Comprobamos que tenemos un ingrediente en la mano, si es asi comprobamos si es lo que deseamos.
                    if (player.ObjectPickedUp)
                        return player.ObjectPickedUp.CompareTag(prefabCompare);
                    //return PrefabUtility.GetCorrespondingObjectFromSource(player.ObjectPickedUp) == prefabCompare;
                }
                else if (target.TryGetComponent(out workstation)) //En caso de que el target sea una mesa de trabajo
                {
                    //Comprobamos que hay un ingrediente puesto en la mesa
                    if (workstation.ObjectInWorktop)
                        return workstation.ObjectInWorktop.CompareTag(prefabCompare);
                        //return PrefabUtility.GetCorrespondingObjectFromSource(workstation.ObjectInWorktop) == prefabCompare;
                        //return workstation.ObjectInWorktop.GetType() == prefabCompare.GetType();
                }
                break;
            }
            case ConditionType.Recipe:

                //En caso de que el target sea el player
                if (target.TryGetComponent(out player))
                {
                    //Comprobamos que tenemos un plato en la mano, si es asi comprobamos si es lo que deseamos.
                    if (player.ObjectPickedUp && player.ObjectPickedUp.TryGetComponent(out plate))
                    {
                        return recipeCompare.ComparePlate(plate.Ingredients);
                    }
                    else if(player.ObjectPickedUp && player.ObjectPickedUp.TryGetComponent(out pot))
                    {
                        return recipeCompare.ComparePlate(pot.Ingredients);
                    }
                        
                }
                else if (target.TryGetComponent(out workstation)) //En caso de que el target sea una mesa de trabajo
                {
                    //Comprobamos que hay un plato puesto en la mesa
                    if (workstation.ObjectInWorktop && workstation.ObjectInWorktop.TryGetComponent(out plate))
                    {
                        return recipeCompare.ComparePlate(plate.Ingredients);
                    }
                    else if (workstation.ObjectInWorktop && workstation.ObjectInWorktop.TryGetComponent(out pot))
                    {
                        return recipeCompare.ComparePlate(pot.Ingredients);
                    }
                }
                break;
            
            case ConditionType.Destination:
                    return pointCondition.IsPlayerIn();
                break;

            case ConditionType.OrderInTime:
                return _orderCondition.IsDone;
                break;
            
        }
       
        return false;
    }
}
public class TutorialBox : MonoBehaviour
{
    [SerializeField] private DialoguePopUp popUp;
    [SerializeField] private TutorialCondition condition;
    [SerializeField] private UnityEvent events;
    // Start is called before the first frame update
    private void Start()
    {
        popUp.SetText(condition.ConditionMessage);
        condition.Start();
    }

    // Update is called once per frame
    void Update()
    {

        if (condition.IsComplete())
        {
            events.Invoke();
            TutorialManager.Instance.CompleteTutorial();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popUp.Show(true);
            if(condition.GetConditionType == ConditionType.Destination) //Activamos el pointcondition para que no completemos sin querer el objetivo
                condition.GetPointCondition.gameObject.SetActive(true);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popUp.Show(false);

        }
    }

    public void RestartTutorial()
    {
        condition.Start();
    }
}
