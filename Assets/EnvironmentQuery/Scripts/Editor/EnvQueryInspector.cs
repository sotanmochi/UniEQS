using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(EnvQuery))]
[CanEditMultipleObjects]
public class EnvQueryInspector : Editor
{
	private ReorderableList reorderableTestList;
	private float lineHeightSpace;

	void OnEnable()
	{
		lineHeightSpace = EditorGUIUtility.singleLineHeight + 3;

		reorderableTestList = new ReorderableList(serializedObject, 
												  serializedObject.FindProperty("EnvQueryTests"),
        										  true, true, true, true);

		reorderableTestList.drawHeaderCallback = (Rect rect) => {
			EditorGUI.LabelField(rect, "Env Query Tests");
		};

		reorderableTestList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
		{
			SerializedProperty element = reorderableTestList.serializedProperty.GetArrayElementAtIndex(index);

			EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element.displayName);

			if(element.objectReferenceValue != null)
			{
				SerializedObject elementObj = new SerializedObject(element.objectReferenceValue);
				// elementObj.Update();
				SerializedProperty propertyIterator = elementObj.GetIterator();

				int i = 1;
				while(propertyIterator.NextVisible(true))
				{
					EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, EditorGUIUtility.singleLineHeight), propertyIterator);
					i++;
				}

				elementObj.ApplyModifiedProperties();
			}
		};

		reorderableTestList.elementHeightCallback = (int index) =>
		{
			float height = 0;

			SerializedProperty element = reorderableTestList.serializedProperty.GetArrayElementAtIndex(index);

			int i = 1;
			if(element.objectReferenceValue != null)
			{
				SerializedObject elementObj = new SerializedObject(element.objectReferenceValue);
				// elementObj.Update();
				SerializedProperty propertyIterator = elementObj.GetIterator();

				while(propertyIterator.NextVisible(true))
				{
					i++;
				}
			}

			height = lineHeightSpace * i;

			return height;
		};

		reorderableTestList.onAddDropdownCallback = (Rect buttonRect, ReorderableList list) => {
			GenericMenu menu = new GenericMenu();
			menu.AddItem(new GUIContent ("Distance"), false, AddEnvQueryTestHandler, "Distance");
			menu.AddItem(new GUIContent ("Dot"), false, AddEnvQueryTestHandler, "Dot");
			menu.AddItem(new GUIContent ("PathFinding"), false, AddEnvQueryTestHandler, "PathFinding");
			menu.AddItem(new GUIContent ("Trace"), false, AddEnvQueryTestHandler, "Trace");
			menu.DropDown(buttonRect);
		};
	}

	private void AddEnvQueryTestHandler(object type)
	{
		string testType = (string)type;

		if(testType == "Distance")
		{
			int index = reorderableTestList.serializedProperty.arraySize;
			reorderableTestList.serializedProperty.arraySize++;
			reorderableTestList.index = index;

			SerializedProperty element = reorderableTestList.serializedProperty.GetArrayElementAtIndex(index);
			element.objectReferenceValue = ScriptableObject.CreateInstance<EnvQueryTestDistance>();
		}
		else if(testType == "Dot")
		{
			int index = reorderableTestList.serializedProperty.arraySize;
			reorderableTestList.serializedProperty.arraySize++;
			reorderableTestList.index = index;

			SerializedProperty element = reorderableTestList.serializedProperty.GetArrayElementAtIndex(index);
			element.objectReferenceValue = ScriptableObject.CreateInstance<EnvQueryTestDot>();
		}
		else if(testType == "PathFinding")
		{
			int index = reorderableTestList.serializedProperty.arraySize;
			reorderableTestList.serializedProperty.arraySize++;
			reorderableTestList.index = index;

			SerializedProperty element = reorderableTestList.serializedProperty.GetArrayElementAtIndex(index);
			element.objectReferenceValue = ScriptableObject.CreateInstance<EnvQueryTestPathFinding>();
		}
		else if(testType == "Trace")
		{
			int index = reorderableTestList.serializedProperty.arraySize;
			reorderableTestList.serializedProperty.arraySize++;
			reorderableTestList.index = index;

			SerializedProperty element = reorderableTestList.serializedProperty.GetArrayElementAtIndex(index);
			element.objectReferenceValue = ScriptableObject.CreateInstance<EnvQueryTestTrace>();
		}

		serializedObject.ApplyModifiedProperties();
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		DrawDefaultInspector();
		reorderableTestList.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}
}
