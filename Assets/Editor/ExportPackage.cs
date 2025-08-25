using UnityEngine;
using UnityEditor;
 
public class ExportPackage{
    [MenuItem ("Export/Export All Data")]
    static void Export()
    {
        AssetDatabase.ExportPackage(AssetDatabase.GetAllAssetPaths(), PlayerSettings.productName + ".unitypackage",
            ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies |
            ExportPackageOptions.IncludeLibraryAssets);
    }
}