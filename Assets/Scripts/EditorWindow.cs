using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;
public class EditorTool : EditorWindow
{
	#region variable declaration
	string[] searchOptions = new string[]
	{
		TextureImporterType.Default.ToString(),TextureImporterType.Sprite.ToString(),TextureImporterType.NormalMap.ToString()
	};
	int searchSelection = 0;
	int conversionSelection = 0;
	bool isPressed = false;
	List<Texture2D> assets;
	List<string> assetPaths;

	Vector2 scrollPos;
	#endregion
	[MenuItem("TessTools/TessTextureEditor")]
	public static void GetWindow()
	{
		GetWindow<EditorTool>("TextureEditor");
		//GetWindow<EditorTool>().minSize = new Vector2(500, 500);
		GetWindow<EditorTool>().maxSize = new Vector2(600, 600);
	}
	private void OnEnable()
	{
		isPressed = false;

	}
	private void OnGUI()
	{
		GUI.Window(1, new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 75
		   , 300, 250), ShowGUI, "Invalid word");
		GUILayout.Label("TessTextureEditor", EditorStyles.boldLabel);

		searchSelection = EditorGUILayout.Popup("Search Texture Type", searchSelection, searchOptions);
		if (GUILayout.Button("Search"))
		{
			SearchTextures();
		}

		EditorGUI.BeginDisabledGroup(!isPressed);
		conversionSelection = EditorGUILayout.Popup("Search Texture Type", conversionSelection, searchOptions);
		bool isConvert = false;
		if (GUILayout.Button("Convert"))
		{
			isConvert = EditorUtility.DisplayDialog("", "Are you sure", "Yes", "No");
		}
		EditorGUI.EndDisabledGroup();
		if (isConvert)
			ConvertTextures();

		if (isPressed)
		{

			if (assets.Count > 0 && assets != null)
				DrawOnGUITextures(assets.ToArray());


		}
	}
	void ShowGUI(int id)
	{

	}
	void SearchTextures()
	{
		assets = new List<Texture2D>();
		assetPaths = new List<string>();
		string[] guids;
		string texturetype = string.Empty;
		switch (searchSelection)
		{
			case 0:
				texturetype = TextureImporterType.Sprite.ToString();
				guids = AssetDatabase.FindAssets("t:texture2D", new[] { "Assets" });
				for (int i = 0; i < guids.Length; i++)
				{
					string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
					Texture2D asset = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
					if (asset != null)
					{
						TextureImporter textureImporter = (TextureImporter)TextureImporter.GetAtPath(assetPath);
						if (textureImporter.textureType == TextureImporterType.Default)
						{
							assets.Add(asset);
							assetPaths.Add(assetPath);
						}
					}
				}
				if (assets.Count > 0)
					isPressed = true;
				DrawOnGUITextures(assets.ToArray());
				break;
			case 1:
				texturetype = TextureImporterType.Sprite.ToString();
				guids = AssetDatabase.FindAssets("t:" + texturetype);
				for (int i = 0; i < guids.Length; i++)
				{
					string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
					Texture2D asset = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
					if (asset != null)
					{
						assets.Add(asset);
						assetPaths.Add(assetPath);
					}
				}
				if (assets.Count > 0)
					isPressed = true;
				DrawOnGUITextures(assets.ToArray());
				break;
			case 2:
				guids = AssetDatabase.FindAssets("t:texture2D", new[] { "Assets" });
				for (int i = 0; i < guids.Length; i++)
				{
					string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
					Texture2D asset = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
					if (asset != null)
					{
						TextureImporter textureImporter = (TextureImporter)TextureImporter.GetAtPath(assetPath);
						if (textureImporter.textureType == TextureImporterType.NormalMap)
						{
							assets.Add(asset);
							assetPaths.Add(assetPath);
						}
					}
				}
				if (assets.Count > 0)
					isPressed = true;
				DrawOnGUITextures(assets.ToArray());
				break;
			default:
				break;
		}
		if (assets.Count == 0)
			isPressed = false;
	}
	void DrawOnGUITextures(Texture2D[] sprites)
	{
		float gap = 50;
		float increamenter = 100;
		GUILayout.Label("Textures:");
		GUILayout.BeginVertical();
		Rect workArea = GUILayoutUtility.GetRect(10, 10000, 10, 10000);

		scrollPos = GUI.BeginScrollView(workArea, scrollPos, new Rect(10, sprites.Length, 10, sprites.Length * increamenter), true, true);
		for (int i = 0; i < sprites.Length; i++)
		{
			Color layoutColor = Color.gray;
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, layoutColor);
			texture2D.Apply();
			GUI.DrawTexture(new Rect(15, gap + 10, 90, 90), texture2D);
			GUI.DrawTexture(new Rect(20, gap + 15, 80, 80), sprites[i]);
			gap += increamenter;
		}
		GUI.EndScrollView();
		GUILayout.EndVertical();
	}
	void ConvertTextures()
	{
		switch (conversionSelection)
		{
			case 0:
				for (int i = 0; i < assetPaths.Count; i++)
				{
					ChangeFormat(assetPaths[i], TextureImporterType.Default);
				}
				break;
			case 1:
				for (int i = 0; i < assetPaths.Count; i++)
				{
					ChangeFormat(assetPaths[i], TextureImporterType.Sprite);
				}
				break;
			case 2:
				for (int i = 0; i < assetPaths.Count; i++)
				{
					ChangeFormat(assetPaths[i], TextureImporterType.NormalMap);
				}
				break;
			default:
				break;
		}
	}
	void ChangeFormat(string path, TextureImporterType textureImporterType)
	{
		TextureImporter textureImporter = (TextureImporter)TextureImporter.GetAtPath(path);
		textureImporter.textureType = textureImporterType;
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
		Sprite asset = AssetDatabase.LoadAssetAtPath<Sprite>(path);
		string assetPath1 =AssetDatabase.GetAssetPath(asset);
		var spriteImporter = AssetImporter.GetAtPath(assetPath1);
		string[] guids = AssetDatabase.FindAssets("t:preset", new[] { "Assets" });
		string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
		Preset preset=AssetDatabase.LoadAssetAtPath<Preset>(assetPath);
		Debug.Log(preset.ApplyTo(spriteImporter));
	}
}
