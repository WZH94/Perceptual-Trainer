using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.Modules
{
  using User;

  public class LessonProgressSaver : MonoBehaviour
  {
    [Header("Lesson Information")]
    [SerializeField] private LessonExerciseType lessonExerciseType;

    private bool progressSaved = false;

    private void Awake()
    {
      if (!progressSaved) SaveProgress();
    }

    private void SaveProgress()
    {
      UserProfile.Instance.SetLessonCompletion(lessonExerciseType);

      progressSaved = true;
    }
  }
}
