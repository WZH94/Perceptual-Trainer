using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using Modules.MCQ;
  using User;

  public class MCQAnswersOverviewPanel : MonoBehaviour
  {
    [Header("Total Points Gained")]
    [SerializeField] private TMP_Text pointsGainedText = null;
    [Header("Minutiae")]
    [SerializeField] private TMP_Text answerMinutiaeText = null;
    [SerializeField] private TMP_Text answeredMinutiaeText = null;
    [Header("Minutiae Statistics")]
    [SerializeField] private TMP_Text bifurcationPercentageCorrectThisExerciseText = null;
    [SerializeField] private TMP_Text ridgeEndingPercentageCorrectThisExerciseText = null;
    [SerializeField] private TMP_Text dotPercentageCorrectThisExerciseText = null;
    [SerializeField] private TMP_Text shortRidgePercentageCorrectThisExerciseText = null;
    [SerializeField] private TMP_Text bifurcationPercentageCorrectAllExercisesText = null;
    [SerializeField] private TMP_Text ridgeEndingPercentageCorrectAllExercisesText = null;
    [SerializeField] private TMP_Text dotPercentageCorrectAllExercisesText = null;
    [SerializeField] private TMP_Text shortRidgePercentageCorrectAllExercisesText = null;

    private GraphicsGradientParent graphicsGradientParent;

    private void Awake()
    {
      graphicsGradientParent = GetComponent<GraphicsGradientParent>();
    }

    public void SetPointsGained( float pointsGained, float totalPoints)
    {
      float percCorrect = pointsGained / totalPoints;
      string prefixText = percCorrect >= 0.9f ? "You answered correctly!"
        : "You answered wrongly!";
      string suffixText = percCorrect >= 0.5f ? "!" : ".";

      pointsGainedText.text = $"{prefixText} You scored {pointsGained.ToString("F1")} out of {totalPoints.ToString("F1")} points{suffixText}";

      graphicsGradientParent.SetGraphicsToValue(percCorrect * 2f);
    }

    public void SetAnswer( MinutiaeType correctAnswer, MinutiaeType answered,
      CategoryStatistics bifurcationStatistics, CategoryStatistics ridgeEndingStatistics, CategoryStatistics dotStatistics, CategoryStatistics shortRidgeStatistics )
    {
      answerMinutiaeText.text = Utils.AddSpaceBeforeCapitals(correctAnswer.ToString());
      answeredMinutiaeText.text = Utils.AddSpaceBeforeCapitals(answered.ToString());

      // Statistics
      UserProfile userProfile = UserProfile.Instance;

      // Set statistics values
      float percentageCorrect = bifurcationStatistics.PercentageCorrect;
      bifurcationPercentageCorrectThisExerciseText.text = $"{percentageCorrect.ToString("F2")}%";
      if (bifurcationStatistics.NumberEncountered == 0) bifurcationPercentageCorrectThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      else bifurcationPercentageCorrectThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percentageCorrect * 0.01f * 2f);

      percentageCorrect = ridgeEndingStatistics.PercentageCorrect;
      ridgeEndingPercentageCorrectThisExerciseText.text = $"{percentageCorrect.ToString("F2")}%";
      if (ridgeEndingStatistics.NumberEncountered == 0) ridgeEndingPercentageCorrectThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      else ridgeEndingPercentageCorrectThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percentageCorrect * 0.01f * 2f);

      percentageCorrect = dotStatistics.PercentageCorrect;
      dotPercentageCorrectThisExerciseText.text = $"{percentageCorrect.ToString("F2")}%";
      if (dotStatistics.NumberEncountered == 0) dotPercentageCorrectThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      else dotPercentageCorrectThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percentageCorrect * 0.01f * 2f);

      percentageCorrect = shortRidgeStatistics.PercentageCorrect;
      shortRidgePercentageCorrectThisExerciseText.text = $"{percentageCorrect.ToString("F2")}%";
      if (shortRidgeStatistics.NumberEncountered == 0) shortRidgePercentageCorrectThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      else shortRidgePercentageCorrectThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percentageCorrect * 0.01f * 2f);

      percentageCorrect = userProfile.GetMinutiaePercentageCorrect(MinutiaeType.Bifurcation);
      bifurcationPercentageCorrectAllExercisesText.text = $"{percentageCorrect.ToString("F2")}%";
      if (userProfile.GetNumberOfMinutiaeEncounters(MinutiaeType.Bifurcation) == 0) bifurcationPercentageCorrectAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      else bifurcationPercentageCorrectAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percentageCorrect * 0.01f * 2f);

      percentageCorrect = userProfile.GetMinutiaePercentageCorrect(MinutiaeType.RidgeEnding);
      ridgeEndingPercentageCorrectAllExercisesText.text = $"{percentageCorrect.ToString("F2")}%";
      if (userProfile.GetNumberOfMinutiaeEncounters(MinutiaeType.RidgeEnding) == 0) ridgeEndingPercentageCorrectAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      else ridgeEndingPercentageCorrectAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percentageCorrect * 0.01f * 2f);

      percentageCorrect = userProfile.GetMinutiaePercentageCorrect(MinutiaeType.Dot);
      dotPercentageCorrectAllExercisesText.text = $"{percentageCorrect.ToString("F2")}%";
      if (userProfile.GetNumberOfMinutiaeEncounters(MinutiaeType.Dot) == 0) dotPercentageCorrectAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      else dotPercentageCorrectAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percentageCorrect * 0.01f * 2f);

      percentageCorrect = userProfile.GetMinutiaePercentageCorrect(MinutiaeType.ShortRidge);
      shortRidgePercentageCorrectAllExercisesText.text = $"{percentageCorrect.ToString("F2")}%";
      if (userProfile.GetNumberOfMinutiaeEncounters(MinutiaeType.ShortRidge) == 0) shortRidgePercentageCorrectAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      else shortRidgePercentageCorrectAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percentageCorrect * 0.01f * 2f);
    }
  }
}
