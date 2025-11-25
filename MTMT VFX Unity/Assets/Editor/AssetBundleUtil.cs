using UnityEditor;
using UnityEngine;

public class AssetBundleUtility
{
    [MenuItem("Assets/Clear All AssetBundle Names")]
    static void ClearAllAssetBundleNames()
    {
        string[] allBundleNames = AssetDatabase.GetAllAssetBundleNames();
        foreach (string bundleName in allBundleNames)
        {
            // Remove the name from all assets assigned to this bundle
            string[] assets = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
            foreach (string assetPath in assets)
            {
                AssetImporter.GetAtPath(assetPath).assetBundleName = null;
            }
        }

        // Remove the bundle names themselves
        AssetDatabase.RemoveUnusedAssetBundleNames();

        Debug.Log("All AssetBundle names cleared.");
    }
}