using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using User;
  using Modules;
  using SceneNavigation;

  public class ExerciseContentScreen : MonoBehaviour
  {
    [Header("Title and Description")]
    [SerializeField] private TMP_Text exerciseNameText = null;
    [SerializeField] private TMP_Text exerciseDescriptionText = null;
    [SerializeField] private bool isGamified = true;
    [Header("Buttons")]
    [SerializeField] private ExerciseDifficultySelectionButton beginnerExerciseButton = null; 
    [SerializeField] private ExerciseDifficultySelectionButton intermediateExerciseButton = null; 
    [SerializeField] private ExerciseDifficultySelectionButton expertExerciseButton = null; 
    [SerializeField] private SceneLoader beginExerciseButton = null;
    [Header("Exercise Content Information")]
    [SerializeField] private StarRankImage rankImage = null;
    [SerializeField] private TMP_Text highscoreText = null;
    [SerializeField] private TMP_Text attemptsText = null;

    private ExerciseDifficultySelectionButton selectedButton;

    private const string DefaultDescription = "Select an exercise below. There are up to 3 levels of difficulty per exercise, but you only need to pass the easiest difficulty to unlock the next content.";

    public void SetExerciseContent( GameObject exerciseDescriptionObject )
    {
      gameObject.SetActive(true);
      beginnerExerciseButton.gameObject.SetActive(false);
      intermediateExerciseButton.gameObject.SetActive(false);
      expertExerciseButton.gameObject.SetActive(false);
      rankImage.gameObject.SetActive(false);
      highscoreText.gameObject.SetActive(false);
      attemptsText.gameObject.SetActive(false);

      if (isGamified) exerciseDescriptionText.text = DefaultDescription;
      else exerciseDescriptionText.text = "Select the exercise below.";

      ExerciseSelectionButton exerciseDescription = exerciseDescriptionObject.GetComponent<ExerciseSelectionButton>();

      exerciseNameText.text = Utils.AddSpaceBeforeCapitals(exerciseDescription.ExerciseType.ToString());

      if (exerciseDescription.BeginnerDifficulty != null)
      {
        beginnerExerciseButton.gameObject.SetActive(true);
        beginnerExerciseButton.SetContent(exerciseDescription.BeginnerDifficulty, exerciseDescription.BeginnerRank);
      }

      if (exerciseDescription.IntermediateDifficulty != null)
      {
        intermediateExerciseButton.gameObject.SetActive(true);
        intermediateExerciseButton.SetContent(exerciseDescription.IntermediateDifficulty, exerciseDescription.IntermediateRank);
      }

      if (exerciseDescription.ExpertDifficulty != null)
      {
        expertExerciseButton.gameObject.SetActive(true);
        expertExerciseButton.SetContent(exerciseDescription.ExpertDifficulty, exerciseDescription.ExpertRank);
      }

      beginExerciseButton.GetComponent<Button>().interactable = false;
    }

    private void OnDisable()
    {
      if (selectedButton != null) selectedButton.GetComponent<Button>().interactable = true;
      selectedButton = null;
    }

    public void SetDifficultyContent( ExerciseDifficultySelectionButton exerciseDifficultyButton )
    {
      UserProfile userProfile = UserProfile.Instance;

      if (selectedButton != null) selectedButton.GetComponent<Button>().interactable = true;
      selectedButton = exerciseDifficultyButton;
      selectedButton.GetComponent<Button>().interactable = false;

      ExerciseDifficultyDescriptionContentScriptableObject descriptionContent = exerciseDifficultyButton.DescriptionContent;
      exerciseDescriptionText.text = descriptionContent.ExerciseDifficultyDescription;

      LessonExerciseType exerciseType = descriptionContent.ExerciseType;
      ExerciseDifficulty exerciseDifficulty = descriptionContent.ExerciseDifficulty;

      highscoreText.gameObject.SetActive(true);

      if (isGamified)
      {
        rankImage.gameObject.SetActive(true);
        rankImage.SetStarImageBasedOnRank(exerciseDifficultyButton.ExerciseDifficultyRank);

        attemptsText.gameObject.SetActive(true);
        attemptsText.text = $"Attempts: <b>{userProfile.GetExerciseAttempts(exerciseType, exerciseDifficulty)}</b>";

        highscoreText.text = $"Highscore: <b>{userProfile.GetExerciseHighscore(exerciseType, exerciseDifficulty)}</b>";
      }

      else
      {
        highscoreText.text = $"Highest Grade: <b>{userProfile.GetExerciseHighscore(exerciseType, exerciseDifficulty)}</b>";
      }

      beginExerciseButton.GetComponent<Button>().interactable = true;
      beginExerciseButton.SetSceneToLoad(descriptionContent.SceneToLoad);
    }
  }
}
