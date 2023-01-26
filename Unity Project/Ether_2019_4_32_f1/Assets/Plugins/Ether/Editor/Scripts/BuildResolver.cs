using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildResolver
{
    public static BuildTargetGroup GetBuildTargetGroupByBuildTarget(BuildTarget buildTarget)
    {
        switch (buildTarget)
        {
            case BuildTarget.Android:
                return BuildTargetGroup.Android;

            case BuildTarget.iOS:
                return BuildTargetGroup.iOS;

            case BuildTarget.StandaloneOSX:
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return BuildTargetGroup.Standalone;
            default:
                return BuildTargetGroup.Standalone;
        }
    }
    public static string GetAssemblyLocation(string Assembly)
    {
        if (File.Exists(Assembly) && Assembly.EndsWith(".dll"))
        {
            return Assembly;
        }
        if (!Assembly.EndsWith(".dll"))
        {
            Assembly += ".dll";
        }
        string Path;
        if (GetAssemblyLocationInTempStagingArea(Assembly, out Path))
        {
            return Path;
        }
        return null;
    }
    public static bool GetAssemblyLocationInTempStagingArea(string AssemblyName, out string AssemblyLocation)
    {
        string TempStagingAreaDirectory = Path.GetDirectoryName(Application.dataPath);
        TempStagingAreaDirectory = Path.Combine(TempStagingAreaDirectory, "Temp");
        TempStagingAreaDirectory = Path.Combine(TempStagingAreaDirectory, "StagingArea");
        TempStagingAreaDirectory = Path.Combine(TempStagingAreaDirectory, "Data");
        TempStagingAreaDirectory = Path.Combine(TempStagingAreaDirectory, "Managed");
        string Asmpath = Path.Combine(TempStagingAreaDirectory, AssemblyName);
        if (File.Exists(Asmpath))
        {
            AssemblyLocation = Asmpath;
            return true;
        }
        AssemblyLocation = null;
        return false;
    }
    public static string GetGameManagersAssetFromTemp()
    {
        string path = Path.GetDirectoryName(Application.dataPath);
        path = Path.Combine(path, "Temp");
        path = Path.Combine(path, "StagingArea");
        path = Path.Combine(path, "Data");
        return Path.Combine(path, "globalgamemanagers.assets");
    }
    public static string GetGameManagerAssetFromCache(BuildTarget _BuildTarget)
    {
        string path = Path.GetDirectoryName(Application.dataPath);
        path = Path.Combine(path, "Library");
        path = Path.Combine(path, "PlayerDataCache");
        path = Path.Combine(path, _BuildTarget.ToString());
        path = Path.Combine(path, "Data");
        return Path.Combine(path, "globalgamemanagers.assets");
    }
    public static string GetGameManagersAssetStandalone(BuildReport report)
    {
        string outputPath = report.summary.outputPath;
        if((EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneOSX && EditorUserBuildSettings.GetPlatformSettings("OSXUniversal", "CreateXcodeProject").Equals("true"))
         ||(EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows && EditorUserBuildSettings.GetPlatformSettings("Standalone", "CreateSolution").Equals("true"))
         ||(EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 &&  EditorUserBuildSettings.GetPlatformSettings("Standalone", "CreateSolution").Equals("true"))
         ||(EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneLinux64 && EditorUserBuildSettings.GetPlatformSettings("Standalone", "CreateSolution").Equals("true")))
        {
            return GetPathForVisualStudioProjectWindowsAndLinuxStandalone(outputPath);
        }
        if (report.summary.platform == BuildTarget.StandaloneOSX)
        {
            return GetPathForMacStandalone(outputPath);
        }
        return GetPathForWindowsOrLinuxStandalone(outputPath);
    }
    static string GetPathForMacStandalone(string outpath)
    {
        string text = Path.Combine(Path.GetDirectoryName(outpath), Path.GetFileNameWithoutExtension(outpath) + ".app");
        text = Path.Combine(text, "Contents");
        text = Path.Combine(text, "Resources");
        text = Path.Combine(text, "Data");
        return Path.Combine(text, "globalgamemanagers.assets");
    }
    static string GetPathForWindowsOrLinuxStandalone(string outpath)
    {
        string text = Path.Combine(Path.GetDirectoryName(outpath), Path.GetFileNameWithoutExtension(outpath) + "_Data");
        return Path.Combine(text, "globalgamemanagers.assets");
    }
    static string GetPathForVisualStudioProjectWindowsAndLinuxStandalone(string outpath)
    {
        string text = Path.Combine(Path.GetDirectoryName(outpath), "build");
        text = Path.Combine(text, "bin");
        text = Path.Combine(text, Path.GetFileNameWithoutExtension(outpath) + "_Data");
        return Path.Combine(text, "globalgamemanagers.assets");
    }
}
