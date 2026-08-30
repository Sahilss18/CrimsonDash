#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CrimsonDash.Editor
{
    public static class WebGLBuilder
    {
        public static void BuildWebGL()
        {
            string[] scenes = new string[]
            {
                "Assets/Scenes/Creation.unity",
                "Assets/Scenes/Play.unity"
            };

            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
            PlayerSettings.WebGL.decompressionFallback = false;

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = "docs",
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            Debug.Log("Starting CrimsonDash WebGL Build to 'docs/'...");
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"WebGL Build succeeded: {summary.totalSize} bytes");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogError($"WebGL Build failed with {summary.totalErrors} errors.");
            }
        }
    }
}
#endif
