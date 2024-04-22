using UnityEngine;
using UnityEngine.UI;
using EasyUI.Tabs;

public class TabsUIVertical : TabsUI
{
    #if UNITY_EDITOR
    private void OnValidate() {
        base.Validate(TabsType.Vertical);
    }
    #endif
}
