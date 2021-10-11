using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using Modules;
  using User;

  public class LessonSelectionButton : MonoBehaviour
  {
    [SerializeField] private Image completedImage = null;
    [SerializeField] private LessonExerciseType lessonType;
    [SerializeField] private LessonDescriptionContentScriptableObject lessonDescription;
    [SerializeField] private LessonExerciseType requiresCompletionOf;
    [SerializeField] private ModuleType requiresCompletionOfType;

    private bool lessonCompleted = false;

    public LessonExerciseType LessonType => lessonType;
    public LessonDescriptionContentScriptableObject LessonDescription => lessonDescription;
    public bool LessonCompleted => lessonCompleted;

    private void Awake()
    {
      UserProfile userProfile = UserProfile.Instance;

      if (userProfile.GetLessonCompletion(lessonType))
      {
        lessonCompleted = true;
        completedImage.gameObject.SetActive(true);
      }

      if (requiresCompletionOf == LessonExerciseType.Count) return;

      bool isUnlocked = false;

      // Requires completion of a prior lesson only
      if (requiresCompletionOfType == ModuleType.Lesson)
      {
        isUnlocked = userProfile.GetLessonCompletion(requiresCompletionOf);
      }

      // Requires completiion of a prior exercise
      else
      {
        isUnlocked = userProfile.GetExerciseRank(requiresCompletionOf, ExerciseDifficulty.Beginner) != ExerciseRank.Fail;
      }

      if (isUnlocked)
      {
        GetComponent<MultiTargetButton>().interactable = true;
        GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      }

      else
      {
        GetComponent<MultiTargetButton>().interactable = false;
        GetComponent<GraphicsGradientParent>().SetGraphicsNegative();
      }
    }
  }
}
