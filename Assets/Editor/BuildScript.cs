using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.Build.Reporting;

public class BuildScript
{
    [MenuItem("Build/Build Windows")]
    public static void BuildWindows()
    {
        // 빌드 타겟 플랫폼
        BuildTarget buildTarget = BuildTarget.StandaloneWindows64;

        // 빌드 출력 폴더
        string buildPath = "Builds/Windows";

        // 메인 씬 경로 (필요한 씬들 넣어야 함)
        string[] scenes = {
            "Assets/Scenes/LoginScene.unity",
            "Assets/Scenes/LoadingScene.unity",
            "Assets/Scenes/CharacterSelectScene.unity",
            "Assets/Scenes/TownScene.unity",
            "Assets/Scenes/DungeonScene.unity",
        };

        // 빌드 옵션 설정
        BuildOptions options = BuildOptions.None;

        // 빌드 경로가 없으면 생성
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }

        // 빌드 실행
        BuildReport report = BuildPipeline.BuildPlayer(scenes, buildPath + "/MyGame.exe", buildTarget, options);

        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + report.summary.outputPath);
        }
        else
        {
            Debug.LogError("Build failed.");
        }
    }
}