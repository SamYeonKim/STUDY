using System;
using System.IO;
using System.Linq.Expressions;
using System.Security.Principal;
using UnityEditor;
using UnityEngine;

namespace SAM.LOCALIZE
{
    public class LocalizeEditorWindow : UnityEditor.EditorWindow
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
        
        [UnityEditor.MenuItem("Test/Create")]
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
            window.manager.LoadFromLocalizeAsset((LanguageCode) Enum.Parse(typeof(LanguageCode), languageCode));
            window.Show();
        }

        private void Awake()
        {
            manager = new LocalizeManager();
        }

        private void OnExportCSV()
        {
            string dstPath = EditorUtility.SaveFilePanel("Export to CSV", Path.GetDirectoryName(Application.dataPath), manager.CurrentLanguageCode.ToString(), "csv");
            if (string.IsNullOrEmpty(dstPath))
            {
                return;
            }
            
            ProcessStatus status = manager.ExportToCSV(dstPath);
            if (status == ProcessStatus.Success)
            {
                EditorUtility.RevealInFinder(dstPath);
                return;
            }
            
            EditorUtility.DisplayDialog("ERROR", status.ToString(), "OK");
        }
        private void OnImportCSV()
        {
            string filePath =
                EditorUtility.OpenFilePanelWithFilters("Select Target CSV", Application.dataPath, new[] {"CSV Files", "csv"});

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
            
            ProcessStatus status = manager.ImportFromCSV(filePath);
            if (status == ProcessStatus.Success)
            {
                return;
            }

            EditorUtility.DisplayDialog("ERROR", status.ToString(), "OK");
        }
        private void OnSave()
        {
            ProcessStatus status = manager.IsSavable();
            if (status != ProcessStatus.Success)
            {
                if (status == ProcessStatus.AlreadyAssetExists)
                {
                    if (EditorUtility.DisplayDialog("Warning", $"{manager.CurrentLanguageCode} 이 이미 있습니다 덮어 씌우시겠습니까?",
                        "OK", "NO"))
                    {
                        manager.SaveToLocalizeAsset();
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("ERROR", status.ToString(), "OK");   
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
            if (status == ProcessStatus.Success)
            {
                return; 
            }
            
            EditorUtility.DisplayDialog("ERROR", status.ToString(), "OK");
        }
        private void OnChangeValue(string key, string newValue)
        {
            ProcessStatus status = manager.UpdateData(key, newValue);
            if (status == ProcessStatus.Success)
            {
                return;
            }

            EditorUtility.DisplayDialog("ERROR", status.ToString(), "OK");
        }

        private void OnDeleteRow(string key)
        {
            ProcessStatus status = manager.DeleteData(key);
            if (status == ProcessStatus.Success)
            {
                return;
            }

            EditorUtility.DisplayDialog("ERROR", status.ToString(), "OK");
        }

        private void OnAddRow(string key, string value)
        {
            ProcessStatus status = manager.InsertData(key, value);
            if (status == ProcessStatus.Success)
            {
                newAddKey = "";
                newAddValue = "";
                return;
            }

            EditorUtility.DisplayDialog("ERROR", status.ToString(), "OK");
        }

        private void OnGUI()
        {
            if ( manager == null )
                return;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Export CSV"))
            {
                OnExportCSV();
            }
            if (GUILayout.Button("Import CSV"))
            {
                OnImportCSV();
            }
            
            EditorGUILayout.Space();
            
            manager.CurrentLanguageCode = (LanguageCode)EditorGUILayout.EnumPopup("Current Language:", manager.CurrentLanguageCode);
            
            EditorGUILayout.Space();

            if (GUILayout.Button("Save"))
            {
                OnSave();
            }

            if (GUILayout.Button("Load"))
            {
                OnLoad();
            }
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            searchFilter = EditorGUILayout.TextField("Search", searchFilter);
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginVertical();
            scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos);
            var table = manager.FindData(searchFilter);
            foreach (var row in table)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(row.Key);
                EditorGUI.BeginChangeCheck();
                string newValue = row.Value;
                newValue = EditorGUILayout.TextField("", newValue);
                if (EditorGUI.EndChangeCheck())
                {
                    editStatus = EditStatus.ChangeValue;
                    changedKey = row.Key;
                    changedNewValue = newValue;
                }

                if (GUILayout.Button("X"))
                {
                    editStatus = EditStatus.DeleteData;
                    changedKey = row.Key;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (editStatus != EditStatus.None)
            {
                if (editStatus == EditStatus.ChangeValue)
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
            newAddValue = EditorGUILayout.TextField("", newAddValue);
            if (GUILayout.Button("+"))
            {
                OnAddRow(newAddKey, newAddValue);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }
    }    
}
