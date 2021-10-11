using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.UI
{
  [RequireComponent(typeof(GraphicsGradientParent))]
  public class CourseProgressBar : MonoBehaviour
  {
    [Header("Course Components")]
    [SerializeField] private LessonSelectionButton[] lessons = null;
    [SerializeField] private ExerciseSelectionButton[] exercises = null;

    private GraphicsGradientParent graphicsGradientParent;
    private TMP_Text progressText;
    private Slider progressBar;

    private void Start()
    {
      graphicsGradientParent = GetComponent<GraphicsGradientParent>();
      progressText = GetComponentInChildren<TMP_Text>();
      progressBar = GetComponentInChildren<Slider>();

      CalculateCompletionPercentage();
    }

    private void CalculateCompletionPercentage()
    {
      // Each lesson and exercise comprises 1 part
      // Each exercise part is divided by the number of difficulties
      // Each rank comprises 1/3 the difficulty
      int numberOfParts = 0;
      int numberOfDifficulties = 0;
      int combinedRank = 0;

      int completedLessons = 0;

      foreach (LessonSelectionButton lesson in lessons)
      {
        ++numberOfParts;
        if (lesson.LessonCompleted) ++completedLessons;
      }

      foreach (ExerciseSelectionButton exercise in exercises)
      {
        ++numberOfParts;

        if (exercise.BeginnerDifficulty != null)
        {
          ++numberOfDifficulties;
          combinedRank += (int)exercise.BeginnerRank;
        }

        if (exercise.IntermediateDifficulty != null)
        {
          ++numberOfDifficulties;
          combinedRank += (int)exercise.IntermediateRank;
        }

        if (exercise.ExpertDifficulty != null)
        {
          ++numberOfDifficulties;
          combinedRank += (int)exercise.ExpertRank;
        }
      }

      float percCompletedPerPart = 100f / (float)numberOfParts;
      float percCompletedPerDifficulty = (numberOfDifficulties > 0) ? percCompletedPerPart / (float)numberOfDifficulties : 0f;
      float percCompletedPerRank = percCompletedPerDifficulty / 3f;

      float totalCompletion = percCompletedPerPart * (float)completedLessons;
      totalCompletion += percCompletedPerRank * combinedRank;

      progressText.text = $"{totalCompletion.ToString("F0")}% Completed";
      progressBar.value = totalCompletion;
      graphicsGradientParent.SetGraphicsToValue(totalCompletion * 0.01f * 2f);
    }
  }
}
