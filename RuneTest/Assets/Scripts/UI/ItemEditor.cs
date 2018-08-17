using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemTemplate))]
[CanEditMultipleObjects]
public class ItemEditor : Editor {

	SerializedProperty icon;
	SerializedProperty id;
	SerializedProperty description;

	SerializedProperty isConsumable;

	SerializedProperty isWeapon;

	SerializedProperty isArmor;

	SerializedProperty armorSlot;

	SerializedProperty isKey;

	SerializedProperty isRuneable;

	void OnEnable() {

		icon = serializedObject.FindProperty ("icon");
		id = serializedObject.FindProperty ("id");
		description = serializedObject.FindProperty ("description");

		isConsumable = serializedObject.FindProperty ("isConsumable");
		isWeapon = serializedObject.FindProperty ("isWeapon");
		isArmor = serializedObject.FindProperty ("isArmor");

		armorSlot = serializedObject.FindProperty ("armorSlot");

		isKey = serializedObject.FindProperty ("isKey");

		isRuneable = serializedObject.FindProperty ("isRuneable");
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();

		EditorStyles.textField.wordWrap = true;

		EditorGUILayout.PropertyField (icon);
		EditorGUILayout.PropertyField (id);
		EditorGUILayout.PropertyField (description,GUILayout.Height(80));

		EditorGUILayout.PropertyField (isConsumable);
		if (isConsumable.boolValue) {
			
		}

		EditorGUI.indentLevel = 0;
		EditorGUILayout.PropertyField (isWeapon);
		if (isWeapon.boolValue) {

		}

		EditorGUI.indentLevel = 0;
		EditorGUILayout.PropertyField (isArmor);
		if (isArmor.boolValue) {
			EditorGUI.indentLevel = 1;
			EditorGUILayout.PropertyField (armorSlot);
		}

		EditorGUI.indentLevel = 0;
		EditorGUILayout.PropertyField (isKey);
		if (isKey.boolValue) {

		}

		EditorGUI.indentLevel = 0;
		EditorGUILayout.PropertyField (isRuneable);

		serializedObject.ApplyModifiedProperties ();
	}
}
