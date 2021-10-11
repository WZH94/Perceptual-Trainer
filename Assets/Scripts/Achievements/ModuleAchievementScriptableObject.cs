using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.Achievements
{
  using Modules;

  [CreateAssetMenu(fileName = "Module Achievements", menuName = "Scriptable Objects/Achievements/Module Achievements", order = 0)]
  public class ModuleAchievementScriptableObject : AchievementScriptableObject
  {
    public ModuleType ModuleType;
    public LessonExerciseType LessonExerciseType;
  }
}
