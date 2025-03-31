using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogGraph : EditorWindow
{

    private DialogGraphView _graphView;
    private string _fileName = "New Narrative";

    [MenuItem("Window/Dialogue Graph")]

    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
        GenerateMinimap();
    }

    private void GenerateMinimap()
    {
        var miniMap = new MiniMap
        {
            anchored = true
        };
        miniMap.SetPosition(new Rect(10, 30, 200, 140));
        _graphView.Add(miniMap);
    }

    private void ConstructGraphView()
    {
        _graphView = new DialogGraphView
        {
            name = "Dialogue Graph"
        };
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var fileNameTextField = new TextField("File Name:");
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
        toolbar.Add(fileNameTextField);

        toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });
        toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });

        var nodeCreateButton = new Button(() =>
        {
            _graphView.CreateNode("Dialogue Node");
        });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name. ", "OK");
            return;
        }

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);

        if (save)
        {
            saveUtility.SaveGraph(_fileName);
        }
        else
        {
            saveUtility.LoadGraph(_fileName);
        }
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }
}
