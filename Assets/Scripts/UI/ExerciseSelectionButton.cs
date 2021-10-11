using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using Modules;
  using User;

  public class ExerciseSelectionButton : MonoBehaviour
  {
    [SerializeField] private LessonExerciseType exerciseType;
    [SerializeField] private ExerciseDifficultyDescriptionContentScriptableObject beginnerDifficultyContent;
    [SerializeField] private ExerciseDifficultyDescriptionContentScriptableObject intermediateDifficulty;
    [SerializeField] private ExerciseDifficultyDescriptionContentScriptableObject expertDifficulty;

    [SerializeField] private StarRankImage beginnerRank = null;
    [SerializeField] private StarRankImage intermediateRank = null;
    [SerializeField] private StarRankImage expertRank = null;

    [HideInInspector] public ExerciseRank BeginnerRank;
    [HideInInspector] public ExerciseRank IntermediateRank;
    [HideInInspector] public ExerciseRank ExpertRank;

    private void Awake()
    {
      UserProfile userProfile = UserProfile.Instance;

      if (beginnerDifficultyContent == null) beginnerRank.gameObject.SetActive(false);
      if (intermediateDifficulty == null) intermediateRank.gameObject.SetActive(false);
      if (expertDifficulty == null) expertRank.gameObject.SetActive(false);

      if (userProfile.GetLessonCompletion(exerciseType))
      {
        GetComponent<MultiTargetButton>().interactable = true;
        GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      }

      else
      {
        GetComponent<MultiTargetButton>().interactable = false;
        GetComponent<GraphicsGradientParent>().SetGraphicsNegative();
      }

      if (beginnerDifficultyContent != null)
      {
        BeginnerRank = userProfile.GetExerciseRank(ExerciseType, ExerciseDifficulty.Beginner);
        beginnerRank.SetStarImageBasedOnRank(BeginnerRank);
      }

      if (intermediateDifficulty != null)
      {
        IntermediateRank = userProfile.GetExerciseRank(ExerciseType, ExerciseDifficulty.Intermediate);
        intermediateRank.SetStarImageBasedOnRank(IntermediateRank);
      }

      if (expertDifficulty != null)
      {
        ExpertRank = userProfile.GetExerciseRank(ExerciseType, ExerciseDifficulty.Expert);
        expertRank.SetStarImageBasedOnRank(ExpertRank);
      }
    }

    public LessonExerciseType ExerciseType => exerciseType;
    public ExerciseDifficultyDescriptionContentScriptableObject BeginnerDifficulty => beginnerDifficultyContent;
    public ExerciseDifficultyDescriptionContentScriptableObject IntermediateDifficulty => intermediateDifficulty;
    public ExerciseDifficultyDescriptionContentScriptableObject ExpertDifficulty => expertDifficulty;
  }
}
