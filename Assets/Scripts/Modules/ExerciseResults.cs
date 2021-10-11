using UnityEngine;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.Modules
{
  using UI;
  using User;

  public class ExerciseResults : MonoBehaviour
  {
    [Header("Components")]
    [SerializeField] private TMP_Text titleText = null;
    [SerializeField] private TMP_Text descriptionText = null;
    [SerializeField] private GraphicsGradientParent scoreCard = null;
    [SerializeField] private TMP_Text scoreCardText = null;
    [SerializeField] private GraphicsGradientParent highscoreCard = null;
    [SerializeField] private TMP_Text highscoreCardText = null;
    [SerializeField] private TMP_Text newHighscoreText = null;
    [SerializeField] private StarRankImage starRank = null;
    [SerializeField] private TMP_Text currentRankText = null;

    private const float passPerc = 0.5f;
    private const float silverPerc = 0.7f;
    private const float goldPerc = 0.9f;

    public void SetResultsOfExercise( bool isGamified, LessonExerciseType exerciseType, ExerciseDifficulty exerciseDifficulty, float results )
    {
      UserProfile userProfile = UserProfile.Instance;

      float percCorrect = results * 0.01f;
      float highscore = userProfile.GetExerciseHighscore(exerciseType, exerciseDifficulty);

      GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percCorrect * 2f);

      if (isGamified && percCorrect < passPerc)
      {
        titleText.text = "You failed the exercise!";
        descriptionText.gameObject.SetActive(true);

        if (results > highscore)
        {
          descriptionText.text += " But you are doing better, so keep at it!";
        }
      }

      scoreCard.SetGraphicsToValue(percCorrect * 2f);
      scoreCardText.text = $"{results.ToString("F2")}/100";

      ExerciseRank exerciseRank = ExerciseRank.Fail;
      ExerciseRank currentRank = userProfile.GetExerciseRank(exerciseType, exerciseDifficulty);

      if (results <= highscore)
      {
        exerciseRank = currentRank;
      }

      else
      {
        highscore = results;

        newHighscoreText.gameObject.SetActive(true);

        if (percCorrect >= passPerc) exerciseRank = ExerciseRank.Pass;
        if (percCorrect >= silverPerc) exerciseRank = ExerciseRank.Silver;
        if (percCorrect >= goldPerc) exerciseRank = ExerciseRank.Gold;

        if (exerciseRank != currentRank)
        {
          currentRankText.text = "New Rank!";
        }
      }

      highscoreCard.SetGraphicsToValue(highscore * 0.01f * 2f);
      highscoreCardText.text = $"{highscore.ToString("F2")}/100";

      userProfile.SetExerciseResultsData(exerciseType, exerciseDifficulty, results, exerciseRank);

      starRank.SetStarImageBasedOnRank(exerciseRank);

      if (!isGamified) titleText.text = $"You have completed the exercise and have been graded {results.ToString("F2")} / 100.";
    }
  }
}
