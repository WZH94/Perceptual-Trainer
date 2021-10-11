using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.Modules
{
  using FingerprintAnnotation;
  using UI;
  using User;

  public class ExerciseHandler : MonoBehaviour
  {
    [Header("Components")]
    [SerializeField] private TMP_Text hintText = null;
    [SerializeField] private AnswersOverviewPanel answersOverviewPanel = null;
    [SerializeField] private Button nextButton = null;
    [SerializeField] private Button submitButton = null;
    [SerializeField] private Button nextPrintButton = null;
    [SerializeField] private GraphicsGradientParent scoreCard = null;
    [SerializeField] private TMP_Text scoreText = null;
    [SerializeField] private TMP_Text questionNumberText = null;
    [SerializeField] private ShowDetailsPanel showDetailsPanel = null;
    [SerializeField] private ExerciseResults exerciseResults = null;
    [Header("Exercise Type")]
    [SerializeField] private LessonExerciseType exerciseType;
    [SerializeField] private ExerciseDifficulty exerciseDifficulty;
    [Header("Exercise Contents")]
    [SerializeField] private bool isGamified = true;
    [SerializeField, Range(1, 50)] private int numberOfQuestions = 10;
    [SerializeField] private bool testingPatternTypes = false;
    [SerializeField] private bool useBasicPatternTypes = false;
    [SerializeField] private bool allowPrintsWithoutTestedLevel1Detail = false;
    [SerializeField] private PatternType testedPatternTypes = 0;
    [SerializeField] private DetailType testedDetailTypes = 0;
    [SerializeField] private bool provideHints = false;
    [SerializeField, Range(0.1f, 0.5f)] private float minimumPassPrecisionMarginPerc = 0.5f;
    [SerializeField, Range(0.5f, 1f)] private float maxMinutiaeFoundPerc = 0.85f;

    private Annotater annotater;
    private AnnotationTools annotationTools;
    private ClassificationTools classificationTools;

    // Exercise statistics
    private CategoryStatistics loopsStatistics = new CategoryStatistics();
    private CategoryStatistics whorlsStatistics = new CategoryStatistics();
    private CategoryStatistics archesStatistics = new CategoryStatistics();
    private AnnotationStatistics coreStatistics = new AnnotationStatistics();
    private AnnotationStatistics deltaStatistics = new AnnotationStatistics();
    private AnnotationStatistics minutiaeStatistics = new AnnotationStatistics();

    private int questionsCompleted = 0;
    private float pointsPerCorrectAnsweredQuestion;
    private float pointsPerCorrectCategory;
    private float currentScore = 0f;
    private float CurrentScore
    {
      get { return currentScore; }
      set
      {
        currentScore = value;
        scoreText.text = $"{currentScore.ToString("F1")} / 100";

        scoreCard.SetGraphicsToValue(currentScore * 0.01f * 2f);
      }
    }

    private FingerprintDetails selectedPrint;
    private List<FingerprintDetails> allEligibleFingerprints = new List<FingerprintDetails>();
    private List<DetailType> testedDetailList = new List<DetailType>();

    public Action OnNewPrint;

    private void Start()
    {
      annotater = GetComponentInChildren<Annotater>();
      annotationTools = GetComponentInChildren<AnnotationTools>();
      classificationTools = GetComponentInChildren<ClassificationTools>();

      nextButton.gameObject.SetActive(false);

      BuildEligibleFingerprintDatabase();

      // Tell the dropdown list for patterns to change its options to the basic 3 patterns
      if (testingPatternTypes && useBasicPatternTypes) GetComponentInChildren<DropdownListPatternTypeEnumPopulator>().SetBasicPatternTypes();

      // In case database is too small, limit the number of exercises to the number of eligible prints
      if (allEligibleFingerprints.Count < numberOfQuestions) numberOfQuestions = allEligibleFingerprints.Count;
      if (numberOfQuestions == 0) Debug.LogWarning("Warning! No eligible print exists in database!");

      // The maximum points awardedable per completed exercise
      pointsPerCorrectAnsweredQuestion = 100f / (float)numberOfQuestions;

      // Calculate points gained per component. This is the number of details test + tested pattern type.
      int numberOfComponents = testedDetailList.Count;
      if (testingPatternTypes) ++numberOfComponents;
      pointsPerCorrectCategory = pointsPerCorrectAnsweredQuestion / numberOfComponents;

      // Adjust available tools based on the tested components
      annotationTools.SetAvailableTools(testedDetailTypes);
      classificationTools.SetAvailableTools(testingPatternTypes);
      showDetailsPanel.SetAvailableOptions(testedDetailTypes);

      // Randomise a print to be tested
      RandomisePrint();

      UserProfile.Instance.AddExerciseAttempt(exerciseType, exerciseDifficulty);
    }

    private void BuildEligibleFingerprintDatabase()
    {
      FingerprintDatabase fingerprintDatabase = FingerprintDatabase.Instance;

      // Add all the tested patterns to a list
      Array patternTypes = Enum.GetValues(typeof(PatternType));

      // Get every fingerprint image of the tested pattern types
      foreach (PatternType patternType in patternTypes)
      {
        if (testedPatternTypes.HasFlag(patternType))
        {
          allEligibleFingerprints.AddRange(fingerprintDatabase.GetFingerprintsOfPatternType(patternType));
        }
      }

      // Filter database based on annotated details
      if (testedDetailTypes != 0)
      {
        Array testedDetailTypesArray = Enum.GetValues(typeof(DetailType));

        // Get the list of tested details (if any) so we don't have to keep looping over every enum value
        foreach (DetailType detailType in testedDetailTypesArray)
        {
          if (testedDetailTypes.HasFlag(detailType))
          {
            testedDetailList.Add(detailType);
          }
        }

        // If details are tested, remove prints that do not have the tested details annotated already
        for (int i = 0; i < allEligibleFingerprints.Count; ++i)
        {
          FingerprintDetails fingerprintDetails = allEligibleFingerprints[i];

          bool imageNotAnnotated = fingerprintDetails.DetailsScale > 0.9f;
          bool noCoreAnnotations = !fingerprintDetails.CanUseForCore ||
            !allowPrintsWithoutTestedLevel1Detail &&
            testedDetailTypes.HasFlag(DetailType.Core) &&
            fingerprintDetails.CoreCoordinates.Count == 0;
          bool noDeltaAnnotations = !fingerprintDetails.CanUseForDelta ||
            !allowPrintsWithoutTestedLevel1Detail &&
            testedDetailTypes.HasFlag(DetailType.Delta) &&
            fingerprintDetails.DeltaCoordinates.Count == 0;
          bool noMinutiaeAnnotations = testedDetailTypes.HasFlag(DetailType.Minutiae) && fingerprintDetails.MinutiaeCoordinates.Count == 0;

          if (imageNotAnnotated || noCoreAnnotations || noDeltaAnnotations || noMinutiaeAnnotations)
          {
            allEligibleFingerprints.RemoveAt(i);
            --i;
          }
        }
      }
    }

    private void RandomisePrint()
    {
      int numberOfEligiblePrints = allEligibleFingerprints.Count;
      int randomPrintIndex = UnityEngine.Random.Range(0, numberOfEligiblePrints);
      selectedPrint = allEligibleFingerprints[randomPrintIndex];

      annotater.LoadPrint(selectedPrint.FingerprintImage, selectedPrint.DetailsScale);
      allEligibleFingerprints.Remove(selectedPrint);

      questionNumberText.text = $"Question {questionsCompleted + 1} / {numberOfQuestions}";

      // Show hint
      if (!provideHints || (!testedDetailList.Contains(DetailType.Core) && !testedDetailList.Contains(DetailType.Delta))) hintText.gameObject.SetActive(false);
      else
      {
        hintText.text = "There is ";

        if (testedDetailList.Contains(DetailType.Core) && testedDetailList.Contains(DetailType.Delta))
          hintText.text += $"<b>{selectedPrint.GetDetailsOfType(DetailType.Core).Count} core</b> and <b>{selectedPrint.GetDetailsOfType(DetailType.Delta).Count} delta </b>";
        else if (testedDetailList.Contains(DetailType.Core)) hintText.text += $"<b>{selectedPrint.GetDetailsOfType(DetailType.Core).Count} core </b>";
        else if (testedDetailList.Contains(DetailType.Delta)) hintText.text += $"<b>{selectedPrint.GetDetailsOfType(DetailType.Delta).Count} delta </b>";

        hintText.text += "in this fingerprint.";
      }
    }

    private float CheckPatternAnswer()
    {
      CategoryStatistics patternStatisticsThisQuestion = new CategoryStatistics();
      ++patternStatisticsThisQuestion.NumberEncountered;
      CategoryStatistics relevantStatisticsToUpdate = new CategoryStatistics();

      // Get the dropdown list value and turn that into the enum flag
      int answeredPatternInt = classificationTools.GetPatternAnswer();
      PatternType answeredPattern = (PatternType)(1 << answeredPatternInt);

      bool answeredCorrectly = false;
      float pointsGainedThisComponent = 0;

      if (useBasicPatternTypes)
      {
        answeredPattern = answeredPatternInt == 0 ? PatternType.LeftLoop : answeredPatternInt == 1 ? PatternType.PlainWhorl : PatternType.PlainArch;

        switch (selectedPrint.PatternType)
        {
          case PatternType.LeftLoop:
          case PatternType.RightLoop:
            relevantStatisticsToUpdate = loopsStatistics;
            if (answeredPatternInt == 0) answeredCorrectly = true;

            break;

          case PatternType.PlainWhorl:
          case PatternType.CentralPocketLoop:
          case PatternType.DoubleLoopWhorl:
          case PatternType.AccidentalWhorl:
            relevantStatisticsToUpdate = whorlsStatistics;
            if (answeredPatternInt == 1) answeredCorrectly = true;

            break;

          case PatternType.PlainArch:
          case PatternType.TentedArch:
            relevantStatisticsToUpdate = archesStatistics;
            if (answeredPatternInt == 2) answeredCorrectly = true;

            break;
        }
      }

      else if (answeredPattern == selectedPrint.PatternType)
      {
         answeredCorrectly = true;
      }

      if (answeredCorrectly)
      {
        pointsGainedThisComponent = pointsPerCorrectCategory;
        ++patternStatisticsThisQuestion.NumberAnsweredCorrectly;
      }

      relevantStatisticsToUpdate.AddStatistics(patternStatisticsThisQuestion);
      UserProfile.Instance.UpdatePatternStatistics(selectedPrint.PatternType, patternStatisticsThisQuestion);

      answersOverviewPanel.SetPatternAnswers(useBasicPatternTypes, selectedPrint.PatternType, answeredPattern, 
        pointsGainedThisComponent, pointsPerCorrectCategory, loopsStatistics, whorlsStatistics, archesStatistics);

      return pointsGainedThisComponent;
    }

    private float CheckAnswerOfLevel1Annotations( DetailType detailType )
    {
      AnnotationStatistics detailStatisticsThisQuestion = new AnnotationStatistics();
      ++detailStatisticsThisQuestion.NumberOfExerciseAnnotations;

      // Get the list of answers for this detail type, make a copy since we will be altering this list
      List<Detail> answeredDetails = new List<Detail>(annotater.GetAnnotationsOfDetailType(detailType));
      // Get the answers for this detail type
      IList<SerializedVector2> answerDetails = selectedPrint.GetDetailsOfType(detailType);

      int numberOfAnswerDetails = answerDetails.Count;
      int numberOfAnsweredDetails = answeredDetails.Count;

      float pointsPerCorrectComponent = pointsPerCorrectCategory;
      float pointsGainedInThisCategory = 0f;

      // The average precision percantage of this answer
      float pointsGainedFromPrecisionPoints = 0f;
      float averagePrecisionPerc = 0f;

      int numMatches = 0;

      bool moreAnsweredThanAnswers = false;

      // If fingerprint has core/delta
      if (numberOfAnswerDetails > 0)
      {
        float pointsPerCorrectAnnotation = pointsPerCorrectComponent / answerDetails.Count;

        // Radius of the prefab used to mark details in the image
        float prefabRadius = annotater.GetPrefabRadius();
        // The distance from the center of the answer position to be considered 100% precision
        float perfectPrecisionMargin = prefabRadius * 0.9f;
        // The max distance from the center of the answer position before it becomes 0% precision
        float totalPrecisionMargin = prefabRadius * 7f;
        // The distance from 100% to 0% precision
        float precisionMargin = totalPrecisionMargin - perfectPrecisionMargin;

        // Check if location of details inputted is correct.
        // Even if the number of answered details is different, we only check against the number of answer details to find the nearest answered spots
        foreach (SerializedVector2 serializedAnswerLocation in answerDetails)
        {
          Vector2 answerLocation = serializedAnswerLocation.ToVector2();
          Detail closestAnswer = null;
          float closestDistanceToAnswer = float.MaxValue;

          // Check every answered location against each answer then remove the closest answered location
          foreach (Detail answeredDetail in answeredDetails)
          {
            Vector2 answeredLocation = answeredDetail.GetComponent<RectTransform>().anchoredPosition;
            float distanceBetweenAnswers = Vector2.Distance(answerLocation, answeredLocation);

            if (distanceBetweenAnswers < closestDistanceToAnswer)
            {
              closestAnswer = answeredDetail;
              closestDistanceToAnswer = distanceBetweenAnswers;
            }
          }

          // There is a user answer to link to the answer
          if (closestAnswer != null)
          {
            float pointsGainedForThisDetail = 0f;
            float precisionPerc = 0f;

            // Annotated distance is 100% precision
            if (closestDistanceToAnswer <= perfectPrecisionMargin)
            {
              ++numMatches;

              pointsGainedFromPrecisionPoints += pointsPerCorrectAnnotation;
              pointsGainedInThisCategory += pointsPerCorrectAnnotation;
              pointsGainedForThisDetail = pointsPerCorrectAnnotation;

              precisionPerc = 100f;
              averagePrecisionPerc += precisionPerc / Mathf.Min(numberOfAnswerDetails, numberOfAnsweredDetails);
            }

            // In between 100% to 0%
            else if (closestDistanceToAnswer > perfectPrecisionMargin && closestDistanceToAnswer <= totalPrecisionMargin)
            {
              ++numMatches;

              // The distance from the answered position within the precision margin
              float distanceWithinPrecisionMargin = closestDistanceToAnswer - perfectPrecisionMargin;
              // The distance at which the user will still be given a score based on the minimum pass percentage
              // If the percentage is lower, then the distance at which the learner has to pass is lower
              // For example, if precision margin is 10, and pass percentage is 0.4, then to pass the user has to get a distance within 4
              // Therefore, max scoreable distance becomes 8
              float scoreablePrecisionDistance = precisionMargin * minimumPassPrecisionMarginPerc * 2f;

              // The percentage the user scores based on the min pass distance
              float scorePrecisionPerc = 1f - (distanceWithinPrecisionMargin / scoreablePrecisionDistance);
              float scorePrecision = scorePrecisionPerc * pointsPerCorrectAnnotation;

              pointsGainedFromPrecisionPoints += scorePrecision;
              pointsGainedInThisCategory += scorePrecision;
              pointsGainedForThisDetail = scorePrecision;

              // The actual precision percentage regardless of grading
              precisionPerc = 100f - (distanceWithinPrecisionMargin / precisionMargin) * 100f;
              averagePrecisionPerc += precisionPerc / Mathf.Min(numberOfAnswerDetails, numberOfAnsweredDetails); ;
            }

            // Else 0 points scored

            // Annotate the answer and link it with the answered detail prefab
            PopupAnnotationInformation popupAnnotationInformation =
              new PopupAnnotationInformation(detailType, true, closestDistanceToAnswer / perfectPrecisionMargin, precisionPerc, pointsGainedForThisDetail);
            annotater.LoadAnswerAndLinkToDetail(popupAnnotationInformation, answerLocation, closestAnswer);

            answeredDetails.Remove(closestAnswer);
          }

          // There is no user answer to link
          else
          {
            PopupAnnotationInformation popupAnnotationInformation = new PopupAnnotationInformation(detailType, false, 0f, 0f, 0f);
            annotater.LoadAnswerAndLinkToDetail(popupAnnotationInformation, answerLocation, null);
          }
        }

        moreAnsweredThanAnswers = answeredDetails.Count > 0;
      }

      else
      {
        // Check that user also inputted 0 details, if so award the full points
        if (answeredDetails.Count == 0)
        {
          pointsGainedInThisCategory += pointsPerCorrectCategory;
        }

        else moreAnsweredThanAnswers = true;
      }

      // User answered more than there are answers
      if (moreAnsweredThanAnswers)
      {
        foreach (Detail answeredDetail in answeredDetails) annotater.LoadUnlinkedAnnotationAnswerToDetail(answeredDetail);
      }

      float accuracy;

      if ((numberOfAnswerDetails == numberOfAnsweredDetails) && (numMatches == numberOfAnswerDetails)) accuracy = 100f;
      else accuracy = (float)numMatches / (float)Mathf.Max(numberOfAnswerDetails, numberOfAnsweredDetails) * 100f;

      pointsGainedInThisCategory = Mathf.Min(pointsGainedInThisCategory, pointsPerCorrectCategory * accuracy * 0.01f);

      if (detailType == DetailType.Core)
      {
        detailStatisticsThisQuestion.NumberMatched = numMatches;
        detailStatisticsThisQuestion.AverageAccuracy = accuracy;
        detailStatisticsThisQuestion.AveragePrecision = averagePrecisionPerc;

        coreStatistics.AddStatistics(detailStatisticsThisQuestion);
        UserProfile.Instance.UpdateAnnotationStatistics(detailType, detailStatisticsThisQuestion);

        answersOverviewPanel.SetCoreAnswers(
        accuracy, numberOfAnswerDetails, numberOfAnsweredDetails, averagePrecisionPerc,
        pointsGainedInThisCategory, pointsPerCorrectCategory, pointsPerCorrectComponent, pointsGainedFromPrecisionPoints, coreStatistics);
      }

      else
      {
        detailStatisticsThisQuestion.NumberMatched = numMatches;
        detailStatisticsThisQuestion.AverageAccuracy = accuracy;
        detailStatisticsThisQuestion.AveragePrecision = averagePrecisionPerc;

        deltaStatistics.AddStatistics(detailStatisticsThisQuestion);
        UserProfile.Instance.UpdateAnnotationStatistics(detailType, detailStatisticsThisQuestion);

        answersOverviewPanel.SetDeltaAnswers(
        accuracy, numberOfAnswerDetails, numberOfAnsweredDetails, averagePrecisionPerc,
        pointsGainedInThisCategory, pointsPerCorrectCategory, pointsPerCorrectComponent, pointsGainedFromPrecisionPoints, deltaStatistics);
      }

      return pointsGainedInThisCategory;
    }

    private float CheckAnswersOfMinutiae()
    {
      AnnotationStatistics detailStatisticsThisQuestion = new AnnotationStatistics();
      ++detailStatisticsThisQuestion.NumberOfExerciseAnnotations;

      DetailType detailType = DetailType.Minutiae;

      // Get the list of answers for this detail type, make a copy since we will be altering this list
      List<Detail> answeredDetails = new List<Detail>(annotater.GetAnnotationsOfDetailType(detailType));
      // Get the answers for this detail type
      List<SerializedVector2> answerDetails = new List<SerializedVector2>(selectedPrint.GetDetailsOfType(detailType));

      int numberOfAnswerDetails = answerDetails.Count;
      int numberOfAnsweredDetails = answeredDetails.Count;

      int maxNumberOfMinutiaeForMaxGrade = Mathf.RoundToInt(numberOfAnswerDetails * maxMinutiaeFoundPerc);
      int numberOfMatchedMinutiae = 0;

      float pointsGainedInThisCategory = 0f;
      float pointsGainedPerMatchedMinutiae = pointsPerCorrectCategory / maxNumberOfMinutiaeForMaxGrade;

      // Radius of the prefab used to mark details in the image
      float prefabRadius = annotater.GetPrefabRadius();
      float maxAcceptableDistance = prefabRadius * 4f;

      Dictionary<Detail, List<SerializedVector2>> answeredMatchedWithOrderedAnswerDistances = new Dictionary<Detail, List<SerializedVector2>>();

      // Loop through every answered detail and find all answers within range of it
      foreach (Detail answeredDetail in answeredDetails)
      {
        List<SerializedVector2> orderedAnswerDistances = new List<SerializedVector2>();

        Vector2 answeredLocation = answeredDetail.GetComponent<RectTransform>().anchoredPosition;

        foreach (SerializedVector2 serializedAnswerLocation in answerDetails)
        {
          Vector2 answerLocation = serializedAnswerLocation.ToVector2();

          float distance = Vector2.Distance(answerLocation, answeredLocation);

          if (distance < maxAcceptableDistance)
          {
            orderedAnswerDistances.Add(serializedAnswerLocation);
            orderedAnswerDistances = orderedAnswerDistances.OrderBy(o => Vector2.SqrMagnitude(answeredLocation - o.ToVector2())).ToList();
          }
        }

        // Check if this detail has any answer within acceptable distance
        if (orderedAnswerDistances.Count > 0)
        {
          answeredMatchedWithOrderedAnswerDistances.Add(answeredDetail, orderedAnswerDistances);
        }

        // Otherwise, nothing to link with
        else
        {
          annotater.LoadUnlinkedAnnotationAnswerToDetail(answeredDetail);
        }
      }

      Detail detailWithClosestMatch = null;
      float nearestSqrDistance = float.MaxValue;

      if (answeredMatchedWithOrderedAnswerDistances.Count > 0)
      {
        while (answeredMatchedWithOrderedAnswerDistances.Keys.Count > 0)
        {
          // Loop through every answered detail and find the one with the closest matching distance
          foreach (Detail minutiae in answeredMatchedWithOrderedAnswerDistances.Keys)
          {
            Vector2 answeredLocation = minutiae.GetComponent<RectTransform>().anchoredPosition;
            Vector2 nearestAnswerLocation = answeredMatchedWithOrderedAnswerDistances[minutiae][0].ToVector2();

            float sqrMag = Vector2.SqrMagnitude(nearestAnswerLocation - answeredLocation);

            if (sqrMag < nearestSqrDistance)
            {
              detailWithClosestMatch = minutiae;
            }
          }

          ++numberOfMatchedMinutiae;

          // Now match the closest answered to answer minutiae
          PopupAnnotationInformation popupAnnotationInformation =
            new PopupAnnotationInformation(detailType, true, 0f, 0f, pointsGainedPerMatchedMinutiae);

          SerializedVector2 matchedAnswerLocation = answeredMatchedWithOrderedAnswerDistances[detailWithClosestMatch][0];
          annotater.LoadAnswerAndLinkToDetail(popupAnnotationInformation, matchedAnswerLocation.ToVector2(), detailWithClosestMatch);

          // Remove the matched details from the dictionary and list
          answeredMatchedWithOrderedAnswerDistances.Remove(detailWithClosestMatch);
          answerDetails.Remove(matchedAnswerLocation);

          List<Detail> detailsToRemove = new List<Detail>();

          // Remove the matched answer from every detail's ordered list
          foreach (Detail minutiae in answeredMatchedWithOrderedAnswerDistances.Keys)
          {
            answeredMatchedWithOrderedAnswerDistances[minutiae].Remove(matchedAnswerLocation);

            if (answeredMatchedWithOrderedAnswerDistances[minutiae].Count == 0)
            {
              annotater.LoadUnlinkedAnnotationAnswerToDetail(minutiae);
              detailsToRemove.Add(minutiae);
            }
          }

          foreach (Detail minutiaeToRemove in detailsToRemove) answeredMatchedWithOrderedAnswerDistances.Remove(minutiaeToRemove);
        }
      }

      // Answers that did not get matched
      foreach (SerializedVector2 serializedAnswerLocation in answerDetails)
      {
        PopupAnnotationInformation popupAnnotationInformation = new PopupAnnotationInformation(detailType, false, 0f, 0f, 0f);
        annotater.LoadAnswerAndLinkToDetail(popupAnnotationInformation, serializedAnswerLocation.ToVector2(), null);
      }

      float percentagePoints = Mathf.Clamp((float)numberOfMatchedMinutiae / (float)maxNumberOfMinutiaeForMaxGrade * 100f  , 0f, 100f);
      float accuracy = numberOfMatchedMinutiae > 0 ? (float)numberOfMatchedMinutiae / (float)numberOfAnsweredDetails * 100f : 0f;
      float percentagePointsAfterAccuracy = Mathf.Min(percentagePoints, accuracy);

      float actualPointsGained = pointsPerCorrectCategory * percentagePoints * 0.01f;
      pointsGainedInThisCategory = pointsPerCorrectCategory * percentagePointsAfterAccuracy * 0.01f;

      detailStatisticsThisQuestion.NumberMatched = numberOfMatchedMinutiae;
      detailStatisticsThisQuestion.AverageAccuracy = accuracy;

      minutiaeStatistics.AddStatistics(detailStatisticsThisQuestion);
      UserProfile.Instance.UpdateAnnotationStatistics(detailType, detailStatisticsThisQuestion);

      answersOverviewPanel.SetMinutiaeAnswers(accuracy, numberOfAnswerDetails, numberOfAnsweredDetails, numberOfMatchedMinutiae,
        actualPointsGained, pointsGainedInThisCategory, pointsPerCorrectCategory, minutiaeStatistics);

      return pointsGainedInThisCategory;
    }

    private void CloseExercise()
    {
      nextButton.gameObject.SetActive(true);
      submitButton.gameObject.SetActive(false);

      exerciseResults.SetResultsOfExercise(isGamified, exerciseType, exerciseDifficulty, currentScore);
    }

    public void SubmitAnswer()
    {
      if (isGamified)
      {
        // Activate and deactivate relevant UI components to show answers overview and disable annotations and submitting answers
        answersOverviewPanel.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(false);
        annotater.enabled = false;
        annotationTools.transform.parent.gameObject.SetActive(false);
        showDetailsPanel.ShowPanel();
      }

      // The number of points gained this exercise
      float totalPointsGained = 0f;

      // Check pattern
      if (testingPatternTypes)
      {
        totalPointsGained += CheckPatternAnswer();
      }

      // Check annotations
      foreach (DetailType detailType in testedDetailList)
      {
        switch (detailType)
        {
          // Level 1 details, 
          case DetailType.Core:
          case DetailType.Delta:
            totalPointsGained += CheckAnswerOfLevel1Annotations(detailType);
            break;

          case DetailType.Minutiae:
            totalPointsGained += CheckAnswersOfMinutiae();
            break;
        }
      }

      CurrentScore += totalPointsGained;

      if (isGamified) answersOverviewPanel.SetPointsGained(totalPointsGained, pointsPerCorrectAnsweredQuestion);

      // Complete 1 exercise, then check if the exercise is over
      ++questionsCompleted;

      if (questionsCompleted < numberOfQuestions)
      {
        if (isGamified) nextPrintButton.gameObject.SetActive(true);
        else MoveToNextPrint();
      }
      else CloseExercise();

    }

    public void MoveToNextPrint()
    {
      RandomisePrint();

      submitButton.gameObject.SetActive(true);
      nextPrintButton.gameObject.SetActive(false);
      answersOverviewPanel.gameObject.SetActive(false);
      showDetailsPanel.HidePanel();

      annotater.enabled = true;
      annotationTools.transform.parent.gameObject.SetActive(true);

      OnNewPrint?.Invoke();
    }
  }
}
