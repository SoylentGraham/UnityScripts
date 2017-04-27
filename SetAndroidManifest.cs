﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SetAndroidManifest : MonoBehaviour {

	const string AndroidManifestAssetName = "AndroidManifest";
	const string AndroidManifestFilename = "AndroidManifest.xml";

	[MenuItem("Android/Set Manifest category.LAUNCHER (Store)")]
	public static void SetAndroidManifest_Launcher()
	{
		SetAndroidManifestTo ("LAUNCHER");
	}

	[MenuItem("Android/Set Manifest category.INFO (Debug)")]
	public static void SetAndroidManifest_Info()
	{
		SetAndroidManifestTo ("INFO");
	}

	public static void SetAndroidManifestTo(string NewCategory)
	{
		//	find manifest assets
		var XmlGuids = AssetDatabase.FindAssets (AndroidManifestAssetName, null);

		//	filter matching filenames
		var XmlFileGuids = new List<string> (XmlGuids).FindAll ((Guid) => {
			var Path = AssetDatabase.GUIDToAssetPath (Guid);
			return Path.EndsWith (AndroidManifestFilename);
		});

		//	setup regular expression
		string MatchPattern = "<category android:name=\"android.intent.category.([A-Z]+)\"/>";
		//	gr: I thought it would just repalce the match, but in c# the whole match is replaced :)
		//string Replacement = NewCategory;
		string Replacement = "<category android:name=\"android.intent.category." + NewCategory + "\"/>";
		Regex RegExpression = new Regex(MatchPattern);

		foreach (var XmlGuid in XmlFileGuids) {
			var Path = AssetDatabase.GUIDToAssetPath (XmlGuid);
			try
			{
				var Contents = System.IO.File.ReadAllText( Path );
				Contents = RegExpression.Replace( Contents, Replacement );
				System.IO.File.WriteAllText( Path, Contents );
				Debug.Log("Updated " + Path );
			}
			catch(System.Exception e) {
				Debug.LogError ("Error replacing manifest in " + Path);
				Debug.LogException (e);
			}
		}

		//	search/replace in file
		//	<category android:name="android.intent.category.LAUNCHER"/>
	}



}
