using pg.core.view;

public class ProjectSceneManager : CoreSceneManager
{
    protected override string GenerateScenePath(string sceneName)
    {
        return "Assets/Resources/scenes/" + sceneName + ".unity";
    }
}
