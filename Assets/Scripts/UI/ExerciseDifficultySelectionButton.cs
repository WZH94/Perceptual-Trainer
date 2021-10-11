using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using Modules;
  using User;

  public class ExerciseDifficultySelectionButton : MonoBehaviour
  {
    [SerializeField] private StarRankImage starImage = null;

    private ExerciseDifficultyDescriptionContentScriptableObject descriptionContent;
    public ExerciseDifficultyDescriptionContentScriptableObject DescriptionContent => descriptionContent;

    public ExerciseRank ExerciseDifficultyRank;

    public void SetContent( ExerciseDifficultyDescriptionContentScriptableObject content, ExerciseRank rank )
    {
      UserProfile userProfile = UserProfile.Instance;

      descriptionContent = content;

      ExerciseDifficultyRank = rank;
      starImage.SetStarImageBasedOnRank(ExerciseDifficultyRank);

      // Unlock or lock based on progress
      LessonExerciseType exerciseType = descriptionContent.ExerciseType;
      ExerciseDifficulty difficulty = descriptionContent.ExerciseDifficulty;

      bool isUnlocked = false;

      switch (difficulty)
      {
        case ExerciseDifficulty.Beginner:
          isUnlocked = true;
          break;

        case ExerciseDifficulty.Intermediate:
          if (userProfile.GetExerciseRank(exerciseType, ExerciseDifficulty.Beginner) == ExerciseRank.Fail) isUnlocked = false;
          else isUnlocked = true;
          break;

        case ExerciseDifficulty.Expert:
          if (userProfile.GetExerciseRank(exerciseType, ExerciseDifficulty.Intermediate) == ExerciseRank.Fail) isUnlocked = false;
          else isUnlocked = true;
          break;
      }

      if (isUnlocked)
      {
        GetComponent<MultiTargetButton>().enabled = true;
        GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      }

      else
      {
        GetComponent<MultiTargetButton>().enabled = false;
        GetComponent<GraphicsGradientParent>().SetGraphicsNegative();
      }
    }
  }
}
