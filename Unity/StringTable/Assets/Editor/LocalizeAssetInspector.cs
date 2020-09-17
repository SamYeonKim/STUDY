using System;
using UnityEditor;
using UnityEngine;

namespace SAM.LOCALIZE
{
    [CustomEditor(typeof(LocalizeAsset))]
    public class LocalizeAssetInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            LocalizeAsset asset = (LocalizeAsset) target;
            
            if (GUILayout.Button("Edit"))
            {
                LocalizeEditorWindow.ShowWindow(asset.languageCode);
            }
        }
    }
}