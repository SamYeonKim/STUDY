using System.Collections.Generic;
using UnityEngine;

namespace SAM.LOCALIZE
{
	public class LocalizeAsset : ScriptableObject
	{
		[System.Serializable]
		public class LocalizeData
		{
			public string key;
			public string localizedString;
		}
		[HideInInspector]
		public string localeIsoCode;
		[HideInInspector]
		public List<LocalizeData> stringTables;

		public void SetLocalizedString(string key, string localizedString)
		{
			if ( stringTables == null )
				stringTables = new List<LocalizeData>();

			stringTables.Add(new LocalizeData()
			{
				key = key,
				localizedString = localizedString,
			});
		}
		public string GetLocalizedString(string key)
		{
			var idFromKey = key.GetHashCode();
			var localizedData = stringTables.Find((item) => item.key == key);

			return localizedData == null ? key : localizedData.localizedString;
		}
	}	
}
