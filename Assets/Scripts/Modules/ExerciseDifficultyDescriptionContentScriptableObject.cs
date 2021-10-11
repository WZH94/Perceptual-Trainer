using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.Modules
{
  [CreateAssetMenu(fileName = "ExerciseDifficultyDescriptionContent", menuName = "Scriptable Objects/Exercise Difficulty Description Content", order = 0)]
  public class ExerciseDifficultyDescriptionContentScriptableObject : ScriptableObject
  {
    public LessonExerciseType ExerciseType;
    public ExerciseDifficulty ExerciseDifficulty;
    [TextArea(3,7)] public string ExerciseDifficultyDescription;
    public string SceneToLoad;
  }
}
