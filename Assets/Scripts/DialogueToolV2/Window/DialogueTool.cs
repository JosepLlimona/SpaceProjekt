using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
 
public class DialogueTool : EditorWindow
{
    [MenuItem("Window/Dialogue/DialogueTool")]
    public static void ShowExample()
    {
        DialogueTool wnd = GetWindow<DialogueTool>();
        wnd.titleContent = new GUIContent("DialogueTool");
    }

    private void OnEnable()
    {
        AddGraphView();
    }

    private void AddGraphView()
    {
        DSGraphView grapView = new DSGraphView();

        grapView.StretchToParentSize();

        rootVisualElement.Add(grapView);
    }
}
