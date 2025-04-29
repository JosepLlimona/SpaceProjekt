using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using DS.Enumarations;

public class DSGraphView : GraphView
{
    public DSGraphView()
    {
        AddGridBackground();

        AddStyles();

        AddManipulators();
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();

        ports.ForEach(port =>
        {
            if (startPort == port)
                return;
            if (startPort.node == port.node)
                return;
            if (startPort.direction == port.direction)
                return;

            compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }

    private DSNode CreateNode(DSDialogueType dialogueType, Vector2 position)
    {
        /*Type nodeType = Type.GetType($"DS.Elements.DS{dialogueType}Node");
        DSNode node = (DSNode) Activator.CreateInstance(nodeType);*/

        DSNode node = (dialogueType == DSDialogueType.SingleChoice) ? new DSSingleChoiceNode() : new DSMultipleChoiceNode();

        node.Initialize(position);
        node.Draw();
        return node;
    }

    private void AddManipulators()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DSDialogueType.SingleChoice));
        this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DSDialogueType.MultipleChoice));
    }

    private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, actionEvent.eventInfo.localMousePosition)))
            );

        return contextualMenuManipulator;
    }

    private void AddStyles()
    {
        StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("DialogueTool/DialogueToolStyle.uss");
        styleSheets.Add(styleSheet);
    }

    private void AddGridBackground()
    {
        GridBackground gridBackground = new GridBackground();

        gridBackground.StretchToParentSize();

        Insert(0, gridBackground);
    }
}
