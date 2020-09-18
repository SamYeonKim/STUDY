using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SAM.LOCALIZE
{
    public class LocalizeEditorWindow : EditorWindow
    {
        enum EditStatus
        {
            None,
            ChangeValue,
            DeleteData,
        }

        private LocalizeManager manager;
        private Vector2 scrollViewPos;
        private string newAddKey;
        private string newAddValue;
        private string changedKey;
        private string changedNewValue;
        private EditStatus editStatus;
        private string searchFilter;

        [MenuItem("Localize/Open Localize Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<LocalizeEditorWindow>();
            window.titleContent = new GUIContent("LocalizeEditor");
            window.Show();
        }

        public static void ShowWindow(string languageCode)
        {
            var window = GetWindow<LocalizeEditorWindow>();
            window.titleContent = new GUIContent("LocalizeEditor");
            window.manager.LoadFromLocalizeAsset((LanguageCode)Enum.Parse(typeof(LanguageCode), languageCode));
            window.ShowUtility();
        }

        private void Awake()
        {
            manager = new LocalizeManager();
        }

        private void OnExportCSV()
        {
            string dstPath = EditorUtility.SaveFilePanel("Export to CSV", Path.GetDirectoryName(Application.dataPath), manager.CurrentLanguageCode.ToString(), "csv");
            if ( string.IsNullOrEmpty(dstPath) )
            {
                return;
            }

            ProcessStatus status = manager.ExportToCSV(dstPath);
            if ( status == ProcessStatus.Success )
            {
                EditorUtility.RevealInFinder(dstPath);
            }

            CommonProcessStatusHandle(status);
        }
        private void OnImportCSV()
        {
            string filePath =
                EditorUtility.OpenFilePanelWithFilters("Select Target CSV", Application.dataPath, new[] { "CSV Files", "csv" });

            if ( string.IsNullOrEmpty(filePath) )
            {
                return;
            }

            ProcessStatus status = manager.ImportFromCSV(filePath);
            if ( status == ProcessStatus.Success )
            {
                ResetScrollView();
            }

            CommonProcessStatusHandle(status);
        }
        private void OnSave()
        {
            ProcessStatus status = manager.IsSavable();
            if ( status != ProcessStatus.Success )
            {
                if ( status == ProcessStatus.AlreadyAssetExists )
                {
                    if ( EditorUtility.DisplayDialog("Warning", $"{manager.CurrentLanguageCode} 이 이미 있습니다 덮어 씌우시겠습니까?",
                        "OK", "NO") )
                    {
                        manager.SaveToLocalizeAsset();
                    }
                }
                else
                {
                    CommonProcessStatusHandle(status);
                }
            }
            else
            {
                manager.SaveToLocalizeAsset();
            }
        }
        private void OnLoad()
        {
            ProcessStatus status = manager.LoadFromLocalizeAsset(manager.CurrentLanguageCode);
            CommonProcessStatusHandle(status);
            if ( status == ProcessStatus.Success )
            {
                ResetScrollView();
            }
        }
        private void OnChangeValue(string key, string newValue)
        {
            ProcessStatus status = manager.UpdateData(key, newValue);
            CommonProcessStatusHandle(status);
        }

        private void OnDeleteRow(string key)
        {
            ProcessStatus status = manager.DeleteData(key);
            CommonProcessStatusHandle(status);
        }

        private void OnAddRow(string key, string value)
        {
            ProcessStatus status = manager.InsertData(key, value);
            CommonProcessStatusHandle(status);

            if ( status == ProcessStatus.Success )
            {
                newAddKey = "";
                newAddValue = "";
            }
        }

        private void CommonProcessStatusHandle(ProcessStatus status)
        {
            if ( status != ProcessStatus.Success )
            {
                EditorUtility.DisplayDialog("ERROR", status.ToString(), "OK");
            }
        }
        private void ResetScrollView()
        {
            scrollViewPos = Vector2.zero;
        }

        private void OnGUI()
        {
            if ( manager == null )
                return;

            EditorGUILayout.BeginHorizontal();
            if ( GUILayout.Button("Export CSV") )
            {
                OnExportCSV();
            }
            if ( GUILayout.Button("Import CSV") )
            {
                OnImportCSV();
            }

            EditorGUILayout.Space();

            manager.CurrentLanguageCode = (LanguageCode)EditorGUILayout.EnumPopup("Current Language:", manager.CurrentLanguageCode);

            EditorGUILayout.Space();

            if ( GUILayout.Button("Save") )
            {
                OnSave();
            }

            if ( GUILayout.Button("Load") )
            {
                OnLoad();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            searchFilter = EditorGUILayout.TextField("Search", searchFilter);
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();
            scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos);
            var style = GUI.skin.textField;
            var table = manager.FindData(searchFilter);
            int maxKeyLength = table.Keys.Count == 0 ? 0 : table.Keys.Max(key => key.Length);
            float maxKeyWidth = GUI.skin.label.CalcSize(new GUIContent(" ")).x * maxKeyLength * 0.8f;

            foreach ( var row in table )
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(row.Key, GUILayout.Width(maxKeyWidth));
                EditorGUI.BeginChangeCheck();
                string newValue = row.Value;
                float height = style.CalcHeight(new GUIContent(newValue), style.lineHeight);
                newValue = EditorGUILayout.TextArea(newValue, GUILayout.Height(height));
                if ( EditorGUI.EndChangeCheck() )
                {
                    editStatus = EditStatus.ChangeValue;
                    changedKey = row.Key;
                    changedNewValue = newValue;
                }

                if ( GUILayout.Button("X", GUILayout.Width(50)) )
                {
                    editStatus = EditStatus.DeleteData;
                    changedKey = row.Key;
                }
                EditorGUILayout.EndHorizontal();
            }

            if ( editStatus != EditStatus.None )
            {
                Undo.RecordObject(this, "ChangeValue");
                if ( editStatus == EditStatus.ChangeValue )
                {
                    OnChangeValue(changedKey, changedNewValue);
                }
                else
                {
                    OnDeleteRow(changedKey);
                }
                changedKey = "";
                changedNewValue = "";
                editStatus = EditStatus.None;
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();
            newAddKey = EditorGUILayout.TextField("Add New Data", newAddKey);
            float newAddValueHeight = style.CalcHeight(new GUIContent(newAddValue), style.lineHeight);
            newAddValue = EditorGUILayout.TextArea(newAddValue, GUILayout.Height(newAddValueHeight));
            if ( GUILayout.Button("+", GUILayout.Width(50)) )
            {
                OnAddRow(newAddKey, newAddValue);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }
    }
}
