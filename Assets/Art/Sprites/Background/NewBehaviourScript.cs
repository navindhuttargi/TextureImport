using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    Preset preset;
    [SerializeField]
    Sprite texture2D;
    [ContextMenu("Test asset")]
    void Start()
    {
        //string[] guids = AssetDatabase.FindAssets("t:preset", new[] { "Assets" });
        //string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        //Preset preset = AssetDatabase.LoadAssetAtPath<Preset>(assetPath);
        Debug.Log(preset.ApplyTo(texture2D));
    }
}
