using UnityEditor;
using UnityEditor.Build.Reporting;

public static class Chapter62Build
{
    [MenuItem("Build/Chapter 62 Windows Build")]
    public static void BuildWindows()
    {
        BuildReport report = BuildPipeline.BuildPlayer(
            new[] { "Assets/Scenes/Chapter62_Combat.unity" },
            "Builds/Chapter62CombatDemo.exe",
            BuildTarget.StandaloneWindows64,
            BuildOptions.None);
        if (report.summary.result != BuildResult.Succeeded)
            throw new System.Exception("Chapter 62 build failed: " + report.summary.result);
    }
}
