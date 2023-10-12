﻿using System.Linq;
using JH_ECS;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EntityBehaviour))]
public class EntityCustomEditor : Editor
{
    private int selectedComponentIndex = 0; // 用户选择的组件索引
    private bool showComponents = false;
    private string searchKeyword="";

    public override void OnInspectorGUI()
    {
        // 获取当前选择的对象
        EntityBehaviour targetScript = (EntityBehaviour)target;
        if (targetScript != null)
        {
            // 显示和编辑脚本中的属性
            GUI.backgroundColor = Color.red; // 设置按钮的背景颜色为红色
            if (GUILayout.Button("Destory Entity"))
            {
              targetScript.DestroySelf();
            }
            GUI.backgroundColor = Color.white; // 恢复背景颜色为白色
            // 显示Components列表
            EditorGUILayout.BeginHorizontal();
            EditorGUI.indentLevel++; // 增加缩进级别以使列表元素缩进
            showComponents = EditorGUILayout.Foldout(showComponents, "Components", true);
            EditorGUILayout.EndHorizontal();
            if (showComponents)
            {
                for (int i = 0; i < targetScript.Components.Count; i++)
                {
                    var component = targetScript.Components[i];
                    // 获取实现了IComponent接口的对象的类型名称
                    string componentName = component.GetType().Name;

                    EditorGUILayout.BeginHorizontal();

                    // 创建一个带有背景颜色的独立单元
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    EditorGUILayout.LabelField(componentName);
                    EditorGUILayout.EndVertical();

                    // 添加Remove按钮
                    if (GUILayout.Button("Remove", GUILayout.Width(80)))
                    {
                        targetScript.Entity._components.Remove(targetScript.Components[i].GetType());
                        i--; // 减少i以处理下一个元素
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
            // 添加一个下拉框来选择要添加的组件
            EditorGUILayout.BeginHorizontal();
            selectedComponentIndex = EditorGUILayout.Popup(selectedComponentIndex, GetAvailableComponentNames());
            if (GUILayout.Button("Add Component"))
            {
                targetScript.Entity.AddComponent(ComponentController.GetAllComponents()[selectedComponentIndex]);
            }
            EditorGUILayout.EndHorizontal();
        }
        
        // 获取可用组件的类型名称数组
    }
    private string[] GetAvailableComponentNames()
    {
        return ComponentController.GetAllComponents().Select(type =>type.GetType().Name).ToArray();
    }

}