using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using FingerprintAnnotation;
  using User;

  public class AnswersOverviewPanel : MonoBehaviour
  {
    [Header("Total Points Gained")]
    [SerializeField] private TMP_Text pointsGainedText = null;
    [Header("Pattern")]
    [SerializeField] private Image patternPanel = null;
    [SerializeField] private TMP_Text answerPatternText = null;
    [SerializeField] private TMP_Text answeredPatternText = null;
    [SerializeField] private TMP_Text patternPointsText = null;
    [Header("Core")]
    [SerializeField] private Image corePanel = null;
    [SerializeField] private TMP_Text accuracyCoreText = null;
    [SerializeField] private TMP_Text answerCoreNumberText = null;
    [SerializeField] private TMP_Text answeredCoreNumberText = null;
    [SerializeField] private TMP_Text answeredCorePrecisionPercText = null;
    [SerializeField] private TMP_Text corePointsText = null;
    [SerializeField] private TMP_Text precisionCorePointsText = null;
    [Header("Delta")]
    [SerializeField] private Image deltaPanel = null;
    [SerializeField] private TMP_Text accuracyDeltaText = null;
    [SerializeField] private TMP_Text answerDeltaNumberText = null;
    [SerializeField] private TMP_Text answeredDeltaNumberText = null;
    [SerializeField] private TMP_Text answeredDeltaPrecisionPercText = null;
    [SerializeField] private TMP_Text deltaPointsText = null;
    [SerializeField] private TMP_Text precisionDeltaPointsText = null;
    [Header("Minutiae")]
    [SerializeField] private Image minutiaePanel = null;
    [SerializeField] private TMP_Text accuracyMinutiaeText = null;
    [SerializeField] private TMP_Text answerMinutiaeNumberText = null;
    [SerializeField] private TMP_Text annotatedMinutiaeNumberText = null;
    [SerializeField] private TMP_Text matchedMinutiaeNumberText = null;
    [SerializeField] private TMP_Text minutiaePointsText = null;
    [SerializeField] private TMP_Text minutiaeMatchedPointsText = null;
    [Header("Pattern Statistics")]
    [SerializeField] private Image patternStatisticsPanel = null;
    [SerializeField] private TMP_Text loopsPercentageCorrectThisExerciseText = null;
    [SerializeField] private TMP_Text whorlsPercentageCorrectThisExerciseText = null;
    [SerializeField] private TMP_Text archesPercentageCorrectThisExerciseText = null;
    [SerializeField] private TMP_Text loopsPercentageCorrectAllExercisesText = null;
    [SerializeField] private TMP_Text whorlsPercentageCorrectAllExercisesText = null;
    [SerializeField] private TMP_Text archesPercentageCorrectAllExercisesText = null;
    [Header("Core Statistics")]
    [SerializeField] private Image coreStatisticsPanel = null;
    [SerializeField] private TMP_Text coreAverageAccuracyThisExerciseText = null;
    [SerializeField] private TMP_Text coreAveragePrecisionThisExerciseText = null;
    [SerializeField] private TMP_Text coreAverageAccuracyAllExercisesText = null;
    [SerializeField] private TMP_Text coreAveragePrecisionAllExercisesText = null;
    [Header("Delta Statistics")]
    [SerializeField] private Image deltaStatisticsPanel = null;
    [SerializeField] private TMP_Text deltaAverageAccuracyThisExerciseText = null;
    [SerializeField] private TMP_Text deltaAveragePrecisionThisExerciseText = null;
    [SerializeField] private TMP_Text deltaAverageAccuracyAllExercisesText = null;
    [SerializeField] private TMP_Text deltaAveragePrecisionAllExercisesText = null;
    [Header("Minutiae Statistics")]
    [SerializeField] private Image minutiaeStatisticsPanel = null;
    [SerializeField] private TMP_Text minutiaeAverageAccuracyThisExerciseText = null;
    [SerializeField] private TMP_Text minutiaeAverageAccuracyAllExercisesText = null;

    private ScrollRect scrollView;
    private GraphicsGradientParent graphicsGradientParent;

    private void Awake()
    {
      scrollView = GetComponentInChildren<ScrollRect>(true);
      graphicsGradientParent = GetComponent<GraphicsGradientParent>();
    }

    private void OnEnable()
    {
      scrollView.verticalNormalizedPosition = 1f;

      patternPanel.gameObject.SetActive(false);
      patternStatisticsPanel.gameObject.SetActive(false);
      corePanel.gameObject.SetActive(false);
      coreStatisticsPanel.gameObject.SetActive(false);
      deltaPanel.gameObject.SetActive(false);
      deltaStatisticsPanel.gameObject.SetActive(false);
      minutiaePanel.gameObject.SetActive(false);
      minutiaeStatisticsPanel.gameObject.SetActive(false);
    }

    public void SetPointsGained( float pointsGained, float totalPoints )
    {
      float percCorrect = pointsGained / totalPoints;
      string prefixText = percCorrect >= 0.8f ? "Well done!"
        : percCorrect >= 0.5f && percCorrect < 0.8f ? "Not bad, but you can do better!"
        : "Looks like you answered incorrectly, have a look at what you did wrong!";
      string suffixText = percCorrect >= 0.5f ? "!" : ".";

      pointsGainedText.text = $"{prefixText} You scored {pointsGained.ToString("F1")} out of {totalPoints.ToString("F1")} points{suffixText}";

      graphicsGradientParent.SetGraphicsToValue(percCorrect * 2f);
    }

    public void SetPatternAnswers( bool usingBasicPatterns, PatternType answerPattern, PatternType answeredPattern, 
      float pointsGained, float totalPoints, CategoryStatistics loopsStatistics, CategoryStatistics whorlsStatistics, CategoryStatistics archesStatistics )
    {
      patternPanel.gameObject.SetActive(true);

      string answerPatternName = usingBasicPatterns ? Utils.ConvertPatternTypeToBasicName(answerPattern) : Utils.AddSpaceBeforeCapitals(answerPattern.ToString());
      string answeredPatternName = usingBasicPatterns ? Utils.ConvertPatternTypeToBasicName(answeredPattern) : Utils.AddSpaceBeforeCapitals(answeredPattern.ToString());

      answerPatternText.text = answerPatternName;
      answeredPatternText.text = answeredPatternName;
      patternPointsText.text = $"+{pointsGained.ToString("F2")}/{totalPoints.ToString("F2")} Points";

      float graphicsGradientValue = (pointsGained / totalPoints) * 2f;
      patternPanel.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(graphicsGradientValue);
      patternStatisticsPanel.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(graphicsGradientValue);

      UserProfile userProfile = UserProfile.Instance;

      // Set statistics values
      float percentageCorrect = loopsStatistics.PercentageCorrect;
      loopsPercentageCorrectThisExerciseText.text = $"{percentageCorrect.ToString("F2")}%";
      if (loopsStatistics.NumberEncountered == 0) loopsPercentageCorrectThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      else loopsPercentageCorrectThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percentageCorrect * 0.01f * 2f);

      percentageCorrect = whorlsStatistics.PercentageCorrect;
      whorlsPercentageCorrectThisExerciseText.text = $"{percentageCorrect.ToString("F2")}%";
      if (whorlsStatistics.NumberEncountered == 0) whorlsPercentageCorrectThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      else whorlsPercentageCorrectThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percentageCorrect * 0.01f * 2f);

      percentageCorrect = archesStatistics.PercentageCorrect;
      archesPercentageCorrectThisExerciseText.text = $"{percentageCorrect.ToString("F2")}%";
      if (archesStatistics.NumberEncountered == 0) archesPercentageCorrectThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      else archesPercentageCorrectThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percentageCorrect * 0.01f * 2f);

      percentageCorrect = userProfile.GetPatternPercentageCorrect(PatternType.LeftLoop);
      loopsPercentageCorrectAllExercisesText.text = $"{percentageCorrect.ToString("F2")}%";
      if (userProfile.GetNumberOfPatternEncounters(PatternType.LeftLoop) == 0) loopsPercentageCorrectAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      else loopsPercentageCorrectAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percentageCorrect * 0.01f * 2f);

      percentageCorrect = userProfile.GetPatternPercentageCorrect(PatternType.PlainWhorl);
      whorlsPercentageCorrectAllExercisesText.text = $"{percentageCorrect.ToString("F2")}%";
      if (userProfile.GetNumberOfPatternEncounters(PatternType.PlainWhorl) == 0) whorlsPercentageCorrectAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      else whorlsPercentageCorrectAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percentageCorrect * 0.01f * 2f);

      percentageCorrect = userProfile.GetPatternPercentageCorrect(PatternType.PlainArch);
      archesPercentageCorrectAllExercisesText.text = $"{percentageCorrect.ToString("F2")}%";
      if (userProfile.GetNumberOfPatternEncounters(PatternType.PlainArch) == 0) archesPercentageCorrectAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsPositive();
      else archesPercentageCorrectAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(percentageCorrect * 0.01f * 2f);
    }

    public void SetCoreAnswers( float accuracy, int numCore, int answeredNumCore, float precision, float pointsGained, float totalPoints,
      float componentMaxPoints, float precisionPoints, AnnotationStatistics annotationStatistics )
    {
      corePanel.gameObject.SetActive(true);

      accuracyCoreText.text = $"<b>Accuracy:</b> {accuracy.ToString("F2")}% (Max Points: {(totalPoints * accuracy * 0.01f).ToString("F2")})";

      answerCoreNumberText.text = $"Number of cores: {numCore}";
      answeredCoreNumberText.text = $"You answered: {answeredNumCore}";
      answeredCorePrecisionPercText.text = $"Precision %: {precision.ToString("F2")}%";

      corePointsText.text = $"+{pointsGained.ToString("F2")}/{totalPoints.ToString("F2")} Points";
      precisionCorePointsText.text = $"+{precisionPoints.ToString("F2")}/{componentMaxPoints.ToString("F2")} Points";

      corePanel.GetComponent<GraphicsGradientParent>().SetGraphicsToValue((pointsGained / totalPoints) * 2f);

      UserProfile userProfile = UserProfile.Instance;

      float averageAccuracy = annotationStatistics.AverageAccuracy;
      coreAverageAccuracyThisExerciseText.text = $"{averageAccuracy.ToString("F2")}%";
      coreAverageAccuracyThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(averageAccuracy * 0.01f * 2f);

      float averagePrecision = annotationStatistics.AveragePrecision;
      coreAveragePrecisionThisExerciseText.text = $"{averagePrecision.ToString("F2")}%";
      coreAveragePrecisionThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(averagePrecision * 0.01f * 2f);

      averageAccuracy = userProfile.GetAnnotatonsAccuracy(DetailType.Core);
      coreAverageAccuracyAllExercisesText.text = $"{averageAccuracy.ToString("F2")}%";
      coreAverageAccuracyAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(averageAccuracy * 0.01f * 2f);

      averagePrecision = userProfile.GetAnnotatonsPrecision(DetailType.Core);
      coreAveragePrecisionAllExercisesText.text = $"{averagePrecision.ToString("F2")}%";
      coreAveragePrecisionAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(averagePrecision * 0.01f * 2f);
    }

    public void SetDeltaAnswers( float accuracy, int numDelta, int answeredNumDelta, float precision, float pointsGained, float totalPoints,
      float componentMaxPoints, float precisionPoints, AnnotationStatistics annotationStatistics )
    {
      deltaPanel.gameObject.SetActive(true);

      accuracyDeltaText.text = $"<b>Accuracy:</b> {accuracy.ToString("F2")}% (Max Points: {(totalPoints * accuracy * 0.01f).ToString("F2")})";

      answerDeltaNumberText.text = $"Number of delta: {numDelta}";
      answeredDeltaNumberText.text = $"You answered: {answeredNumDelta}";
      answeredDeltaPrecisionPercText.text = $"Precision %: {precision.ToString("F2")}%";

      deltaPointsText.text = $"+{pointsGained.ToString("F2")}/{totalPoints.ToString("F2")} Points";
      precisionDeltaPointsText.text = $"+{precisionPoints.ToString("F2")}/{componentMaxPoints.ToString("F2")} Points";

      deltaPanel.GetComponent<GraphicsGradientParent>().SetGraphicsToValue((pointsGained / totalPoints) * 2f);

      UserProfile userProfile = UserProfile.Instance;

      float averageAccuracy = annotationStatistics.AverageAccuracy;
      deltaAverageAccuracyThisExerciseText.text = $"{averageAccuracy.ToString("F2")}%";
      deltaAverageAccuracyThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(averageAccuracy * 0.01f * 2f);

      float averagePrecision = annotationStatistics.AveragePrecision;
      deltaAveragePrecisionThisExerciseText.text = $"{averagePrecision.ToString("F2")}%";
      deltaAveragePrecisionThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(averagePrecision * 0.01f * 2f);

      averageAccuracy = userProfile.GetAnnotatonsAccuracy(DetailType.Delta);
      deltaAverageAccuracyAllExercisesText.text = $"{averageAccuracy.ToString("F2")}%";
      deltaAverageAccuracyAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(averageAccuracy * 0.01f * 2f);

      averagePrecision = userProfile.GetAnnotatonsPrecision(DetailType.Delta);
      deltaAveragePrecisionAllExercisesText.text = $"{averagePrecision.ToString("F2")}%";
      deltaAveragePrecisionAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(averagePrecision * 0.01f * 2f);
    }

    public void SetMinutiaeAnswers( float accuracy, int numMinutiae, int annotatedNumMinutie, int matchedNumMinutiae,
      float actualPointsGained, float matchedPointsGained, float totalPoints, AnnotationStatistics annotationStatistics )
    {
      minutiaePanel.gameObject.SetActive(true);

      accuracyMinutiaeText.text = $"<b>Accuracy:</b> {accuracy.ToString("F2")}% (Max Points: {(totalPoints * accuracy * 0.01f).ToString("F2")})";

      answerMinutiaeNumberText.text = $"Number of minutiae: {numMinutiae}";
      annotatedMinutiaeNumberText.text = $"Number annotated: {annotatedNumMinutie}";
      matchedMinutiaeNumberText.text = $"Number matched: {matchedNumMinutiae}";

      minutiaePointsText.text = $"+{matchedPointsGained.ToString("F2")}/{totalPoints.ToString("F2")} Points";
      minutiaeMatchedPointsText.text = $"+{actualPointsGained.ToString("F2")}/{totalPoints.ToString("F2")} Points";

      minutiaePanel.GetComponent<GraphicsGradientParent>().SetGraphicsToValue((matchedPointsGained / totalPoints) * 2f);

      UserProfile userProfile = UserProfile.Instance;

      float averageAccuracy = annotationStatistics.AverageAccuracy;
      minutiaeAverageAccuracyThisExerciseText.text = $"{averageAccuracy.ToString("F2")}%";
      minutiaeAverageAccuracyThisExerciseText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(averageAccuracy * 0.01f * 2f);

      averageAccuracy = userProfile.GetAnnotatonsAccuracy(DetailType.Minutiae);
      minutiaeAverageAccuracyAllExercisesText.text = $"{averageAccuracy.ToString("F2")}%";
      minutiaeAverageAccuracyAllExercisesText.GetComponent<GraphicsGradientParent>().SetGraphicsToValue(averageAccuracy * 0.01f * 2f);
    }
  }
}
