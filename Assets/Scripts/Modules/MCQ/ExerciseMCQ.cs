using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.Modules.MCQ
{
  using UI;
  using User;

  public class ExerciseMCQ : MonoBehaviour
  {
    [Header("Components")]
    [SerializeField] private MinutiaeDatabase minutiaeDatabase = null;
    [SerializeField] private MinutiaeMCQSelection mcqSelection = null;
    [SerializeField] private Image minutiaeImage = null;
    [SerializeField] private Button nextMinutiaeButton = null;
    [SerializeField] private Button nextButton = null;
    [SerializeField] private GraphicsGradientParent scoreCard = null;
    [SerializeField] private TMP_Text scoreText = null;
    [SerializeField] private TMP_Text exerciseNumberText = null;
    [SerializeField] private MCQAnswersOverviewPanel answersOverviewPanel = null;
    [SerializeField] private ExerciseResults exerciseResults = null;
    [Header("Exercise Type")]
    [SerializeField] private LessonExerciseType exerciseType;
    [SerializeField] private ExerciseDifficulty exerciseDifficulty;
    [Header("Content")]
    [SerializeField, Range(1, 50)] private int numberOfQuestions = 40;

    private CategoryStatistics bifurcationStatistics = new CategoryStatistics();
    private CategoryStatistics ridgeEndingStatistics = new CategoryStatistics();
    private CategoryStatistics dotStatistics = new CategoryStatistics();
    private CategoryStatistics shortRidgeStatistics = new CategoryStatistics();

    private int questionsCompleted = 0;
    private float pointsPerCorrectAnsweredExercise;

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

    private void Start()
    {
      nextButton.gameObject.SetActive(false);

      // The maximum points awardedable per completed exercise
      pointsPerCorrectAnsweredExercise = 100f / (float)numberOfQuestions;

      RandomiseMinutiaeImage();

      UserProfile.Instance.AddExerciseAttempt(exerciseType, exerciseDifficulty);
    }

    private void RandomiseMinutiaeImage()
    {
      exerciseNumberText.text = $"Question {questionsCompleted + 1} / {numberOfQuestions}";

      minutiaeImage.sprite = minutiaeDatabase.GetMinutiaeAndRemoveFromPool();
    }

    private void CheckMCQAnswer()
    {
      CategoryStatistics minutiaeStatisticsThisQuestion = new CategoryStatistics();
      ++minutiaeStatisticsThisQuestion.NumberEncountered;

      CategoryStatistics relevantStatisticsToUpdate = new CategoryStatistics();

      MinutiaeType answeredMinutiaeType = mcqSelection.GetMCQSelection();
      mcqSelection.ResetSelection();
      mcqSelection.gameObject.SetActive(false);
      MinutiaeType correctAnswer = minutiaeDatabase.GetMinutiaeTypeOfImage(minutiaeImage.sprite);

      switch (correctAnswer)
      {
        case MinutiaeType.Bifurcation:
          relevantStatisticsToUpdate = bifurcationStatistics;
          break;

        case MinutiaeType.RidgeEnding:
          relevantStatisticsToUpdate = ridgeEndingStatistics;
          break;

        case MinutiaeType.Dot:
          relevantStatisticsToUpdate = dotStatistics;
          break;

        case MinutiaeType.ShortRidge:
          relevantStatisticsToUpdate = shortRidgeStatistics;
          break;
      }

      float pointsGained = answeredMinutiaeType == correctAnswer ? pointsPerCorrectAnsweredExercise : 0f;

      if (answeredMinutiaeType == correctAnswer)
      {
        CurrentScore += pointsGained;
        ++minutiaeStatisticsThisQuestion.NumberAnsweredCorrectly;
      }

      relevantStatisticsToUpdate.AddStatistics(minutiaeStatisticsThisQuestion);
      UserProfile.Instance.UpdateMinutiaeCategoryStatistics(correctAnswer, minutiaeStatisticsThisQuestion);

      answersOverviewPanel.SetPointsGained(pointsGained, pointsPerCorrectAnsweredExercise);
      answersOverviewPanel.SetAnswer(correctAnswer, answeredMinutiaeType, bifurcationStatistics, ridgeEndingStatistics, dotStatistics, shortRidgeStatistics);
    }

    private void CloseExercise()
    {
      nextButton.gameObject.SetActive(true);

      exerciseResults.SetResultsOfExercise(true, exerciseType, exerciseDifficulty, currentScore);
    }

    public void SubmitAnswer()
    {
      answersOverviewPanel.gameObject.SetActive(true);

      CheckMCQAnswer();

      // Complete 1 exercise, then check if the exercise is over
      ++questionsCompleted;

      if (questionsCompleted < numberOfQuestions) nextMinutiaeButton.gameObject.SetActive(true);
      else CloseExercise();
    }

    public void MoveToNextMinutiae()
    {
      RandomiseMinutiaeImage();

      nextMinutiaeButton.gameObject.SetActive(false);
      answersOverviewPanel.gameObject.SetActive(false);
      mcqSelection.gameObject.SetActive(true);
    }
  }
}
