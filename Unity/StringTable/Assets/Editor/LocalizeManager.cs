using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace SAM.LOCALIZE
{
    public enum LanguageCode
    {
        ko_KR,
        en_US,
        jp_JP,
    }

    public enum ProcessStatus
    {
        Success,
        UnknowError,
        DuplicateKey,
        EmptyValue,
        EmptyKey,
        WrongKey,
        WrongFormat,
        WrongFilePath,
        AlreadyAssetExists,
    }

    public class LocalizeManager
    {
        private const string AVAILABLE_KEY_FORMAT_PATTERN = @"^[a-zA-Z0-9_-]+$"; 
        private const string LOCALIZEASSET_DIR_PATH = "Assets/Resources/Localize";
        public LanguageCode CurrentLanguageCode { get; set; }
        public Dictionary<string, string> table { get; set; } = new Dictionary<string, string>();

        public ProcessStatus InsertData(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return ProcessStatus.EmptyKey;
            }

            if (string.IsNullOrEmpty(value))
            {
                return ProcessStatus.EmptyValue;
            }

            key = key.ToLower().Trim();
            
            if (table.ContainsKey(key))
            {
                return ProcessStatus.DuplicateKey;
            }

            if (!IsAvailableKey(key))
            {
                return ProcessStatus.WrongFormat;
            }

            value = value.Trim();
            table.Add(key, value);
            return ProcessStatus.Success;
        }

        public ProcessStatus DeleteData(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return ProcessStatus.EmptyKey;
            }

            key = key.ToLower().Trim();
            
            if (!table.ContainsKey(key))
            {
                return ProcessStatus.WrongKey;
            }

            table.Remove(key);
            
            return ProcessStatus.Success;
        }

        public ProcessStatus UpdateData(string key, string newValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                return ProcessStatus.EmptyKey;
            }

            if (string.IsNullOrEmpty(newValue))
            {
                return ProcessStatus.EmptyValue;
            }

            key = key.ToLower().Trim();

            if (!table.ContainsKey(key))
            {
                return ProcessStatus.WrongKey;
            }

            newValue = newValue.Trim();
            table[key] = newValue;
            
            return ProcessStatus.Success;
        }
        public Dictionary<string, string> FindData(string filter = "")
        {
            if (string.IsNullOrEmpty(filter))
            {
                return table;
            }

            filter = filter.Trim();
            return table.Where(source => source.Value.ToLower().Contains(filter.ToLower())).ToDictionary(x => x.Key, x => x.Value);
        }
        public ProcessStatus ExportToCSV(string path)
        {
            if (table.Keys.Count == 0)
            {
                return ProcessStatus.EmptyKey;
            }

            var builder = new StringBuilder();
            builder.AppendLine($"Key,Value");
            foreach (var row in table)
            {
                builder.AppendLine($"{row.Key},{row.Value}");
            }

            try
            {
                File.WriteAllText(path, builder.ToString());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return ProcessStatus.UnknowError;
            }
            
            return ProcessStatus.Success;
        }
        public ProcessStatus ImportFromCSV(string path)
        {
            var lines = File.ReadAllLines(path);
            if (lines.Length < 2)
            {
                return ProcessStatus.WrongFormat;
            }

            Dictionary<string, string> newTable = new Dictionary<string, string>();
            for (int idx = 1; idx < lines.Length; idx++)
            {
                var splitted = lines[idx].Split(',');
                if (splitted.Length != 2)
                {
                    return ProcessStatus.WrongFormat;
                }

                string key = splitted[0];
                string value = splitted[1];
                
                if (!IsAvailableKey(key))
                {
                    Debug.LogError($"Wrong Format Key : {key}");
                    return ProcessStatus.WrongFormat;
                }
                
                if (newTable.ContainsKey(key))
                {
                    newTable[key] = value;
                    Debug.LogWarning($"Duplicate Key : {key}");
                }
                else
                {
                    newTable.Add(key, value);    
                }
            }
            
            table.Clear();
            table = newTable;
                
            return ProcessStatus.Success;
        }

        public ProcessStatus IsSavable()
        {
            if (table.Keys.Count == 0)
            {
                return ProcessStatus.EmptyKey;
            }

            if (AssetDatabase.FindAssets(CurrentLanguageCode.ToString(), new[] {LOCALIZEASSET_DIR_PATH}).Length != 0)
            {
                return ProcessStatus.AlreadyAssetExists;
            }

            return ProcessStatus.Success;
        }
        public void SaveToLocalizeAsset()
        {
            if (table.Keys.Count == 0)
            {
                return;
            }

            string dstPath = Path.Combine(LOCALIZEASSET_DIR_PATH, CurrentLanguageCode.ToString() + ".asset");
            LocalizeAsset asset = ScriptableObject.CreateInstance<LocalizeAsset>();
            asset.languageCode = CurrentLanguageCode.ToString();
            foreach (var data in table)
            {
                asset.table.Add(new LocalizeAsset.Data()
                {
                    key = data.Key,
                    value = data.Value,
                });
            }

            Directory.CreateDirectory(LOCALIZEASSET_DIR_PATH);
            AssetDatabase.CreateAsset(asset, dstPath);
            AssetDatabase.SaveAssets();
        }
        public ProcessStatus LoadFromLocalizeAsset(LanguageCode code)
        {
            var assets = AssetDatabase.FindAssets(code.ToString(), new[] {LOCALIZEASSET_DIR_PATH});
            if (assets.Length == 0)
            {
                table.Clear();
                return ProcessStatus.WrongFilePath;
            }

            var localizeAsset = AssetDatabase.LoadAssetAtPath<LocalizeAsset>(AssetDatabase.GUIDToAssetPath(assets[0]));
            if (localizeAsset == null)
            {
                table.Clear();
                return ProcessStatus.UnknowError;
            }
            
            Dictionary<string, string> newTable = new Dictionary<string, string>();
            foreach (var data in localizeAsset.table)
            {
                string key = data.key;
                string value = data.value;
                if (newTable.ContainsKey(key))
                {
                    newTable[key] = value;
                    Debug.LogWarning($"Duplicate Key : {key}");
                }
                else
                {
                    newTable.Add(key, value);    
                }
            }
            
            table.Clear();
            table = newTable;
            CurrentLanguageCode = code;

            return ProcessStatus.Success;
        }

        private bool IsAvailableKey(string key)
        {
            return Regex.IsMatch(key, AVAILABLE_KEY_FORMAT_PATTERN);
        }
    }    
}

