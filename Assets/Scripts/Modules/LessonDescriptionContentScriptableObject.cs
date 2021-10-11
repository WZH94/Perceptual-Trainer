using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.Modules
{
  [CreateAssetMenu(fileName = "LessonDescriptionContent", menuName = "Scriptable Objects/Lesson Description Content", order = 0)]
  public class LessonDescriptionContentScriptableObject : ScriptableObject
  {
    public string ModuleName;
    [TextArea(3,7)] public string ModuleDescription;
    public string SceneName;
  }
}
