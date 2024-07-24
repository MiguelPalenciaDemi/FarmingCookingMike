using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TutorialBox))]
public class TutorialBoxEditor : Editor
{
    private SerializedProperty popUpProperty;
    private SerializedProperty tutorialConditionProperty;
    private SerializedProperty eventProperty;
    
    

    private void OnEnable()
    {
        popUpProperty = GetProperty("popUp",serializedObject);
        tutorialConditionProperty = GetProperty("condition",serializedObject);
        eventProperty = GetProperty("events",serializedObject);
        
        
    }

    public override void OnInspectorGUI()
    {
        var instance = (TutorialBox)target;
        EditorGUILayout.PropertyField(popUpProperty, new GUIContent("PopUp"));
        //EditorGUILayout.PropertyField(conditionTypeProperty, new GUIContent("Type of Condition"));
        EditorGUILayout.PropertyField(tutorialConditionProperty, new GUIContent("Condition Configuration"));
        EditorGUILayout.PropertyField(eventProperty, new GUIContent("Event when complete"));
        
        
        
        serializedObject.ApplyModifiedProperties();//Aplicar para que los cambios tengan efecto
    }

    private SerializedProperty GetProperty(string nameOfProperty, SerializedObject serializedObj)
    {
        var property = serializedObj.FindProperty(nameOfProperty);
        return property;
    }
}

[CustomPropertyDrawer(typeof(TutorialCondition))]
public class TutorialConditionDrawerEdition : PropertyDrawer
{
    private SerializedProperty conditionTypeProperty;
    private SerializedProperty messageProperty;
    private SerializedProperty targetProperty;
    //Type Ingredient
    private SerializedProperty desiredProperty;
    //private SerializedProperty prefabProperty;
    private SerializedProperty prefabTag;
    private string tagSelected;
    
    
    private SerializedProperty recipeProperty;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        conditionTypeProperty = property.FindPropertyRelative("conditionType");
        messageProperty = property.FindPropertyRelative("conditionMessage");
        targetProperty = property.FindPropertyRelative("target");
        desiredProperty = property.FindPropertyRelative("desired");
        prefabTag = property.FindPropertyRelative("prefabCompare");
        recipeProperty = property.FindPropertyRelative("recipeCompare");
        tagSelected = prefabTag.stringValue;
        EditorGUI.BeginProperty(position, label, property);
        EditorGUILayout.PrefixLabel("Message");
        messageProperty.stringValue = EditorGUILayout.TextArea(messageProperty.stringValue);
        // EditorGUILayout.PropertyField(messageProperty);
        EditorGUILayout.PropertyField(targetProperty);
        EditorGUILayout.PropertyField(conditionTypeProperty);

        switch ((ConditionType)conditionTypeProperty.enumValueIndex)
        {
            case ConditionType.Ingredient:
                EditorGUILayout.PropertyField(desiredProperty);
                break;
            case ConditionType.Object:
                tagSelected = EditorGUILayout.TagField("Tag To Compare", tagSelected);
                prefabTag.stringValue = tagSelected;
                break;
            case ConditionType.Recipe:
                EditorGUILayout.PropertyField(recipeProperty);
                break;
                
        }
        
        
        EditorGUI.EndProperty();
        
        

    }
}
