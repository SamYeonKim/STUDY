using UnityEditor;
using UnityEngine;

namespace SAM.LOCALIZE
{
    [CustomEditor(typeof(LocalizeAsset))]
    public class LocalizeAssetInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            LocalizeAsset asset = (LocalizeAsset)target;

            if ( GUILayout.Button(new GUIContent("Edit", "Open Localize Editor Window")) )
            {
                LocalizeEditorWindow.ShowWindow(asset.localeIsoCode);
            }
        }
    }
}