using System;
using System.Collections.Generic;

using UnityEngine;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using Achievements;
  using FingerprintAnnotation;
  using Modules;
  using Modules.MCQ;
  using User;

  public class UserProfilePanel : MonoBehaviour
  {
    [Header("Time Spent")]
    [SerializeField] private TMP_Text totalTimeSpentText = null;
    [Header("Exercise Ranks")]
    [SerializeField] private TMP_Text bronzeRanksEarnedText = null;
    [SerializeField] private TMP_Text silverRanksEarnedText = null;
    [SerializeField] private TMP_Text goldRanksEarnedText = null;
    [Header("Pattern Statistics")]
    [SerializeField] private TMP_Text loopsCorrectPercentageText = null;
    [SerializeField] private TMP_Text whorlsCorrectPercentageText = null;
    [SerializeField] private TMP_Text archesCorrectPercentageText = null;
    [Header("Core Statistics")]
    [SerializeField] private TMP_Text coreMatchedText = null;
    [SerializeField] private TMP_Text coreAverageAccuracyText = null;
    [SerializeField] private TMP_Text coreAveragePrecisionText = null;
    [Header("Delta Statistics")]
    [SerializeField] private TMP_Text deltaMatchedText = null;
    [SerializeField] private TMP_Text deltaAverageAccuracyText = null;
    [SerializeField] private TMP_Text deltaAveragePrecisionText = null;
    [Header("Minutiae Statistics")]
    [SerializeField] private TMP_Text bifurcationCorrectPercentageText = null;
    [SerializeField] private TMP_Text ridgeEndingCorrectPercentageText = null;
    [SerializeField] private TMP_Text dotCorrectPercentageText = null;
    [SerializeField] private TMP_Text shortRidgeCorrectPercentageText = null;
    [SerializeField] private TMP_Text minutiaeMatchedText = null;
    [SerializeField] private TMP_Text minutiaeAverageAccuracyText = null;
    [Header("Achievements")]
    [SerializeField] private PopupScreen achievementTabPrefab = null;
    [SerializeField] private RectTransform scrollRectContent = null;
    [SerializeField] private AchievementScriptableObject[] allAchievements = null;

    private UserProfile userProfile;

    private void Awake()
    {
      userProfile = UserProfile.Instance;

      float timeInSeconds = userProfile.GetTimeSpentInLessonsAndExercises();

      // Time spent
      totalTimeSpentText.text =
        $"Time Spent in Lessons and Exercises: <b>{TimeSpan.FromSeconds(timeInSeconds).Hours} hrs {TimeSpan.FromSeconds(timeInSeconds).Minutes} mins </b>";

      // Exercise ranks
      int bronzeEarned = 0;
      int silverEarned = 0;
      int goldEarned = 0;

      foreach (LessonExerciseType exercistType in Enum.GetValues(typeof(LessonExerciseType)))
      {
        if (exercistType != LessonExerciseType.Count)
        {
          foreach (ExerciseDifficulty exerciseDifficulty in Enum.GetValues(typeof(ExerciseDifficulty)))
          {
            switch (userProfile.GetExerciseRank(exercistType, exerciseDifficulty))
            {
              case ExerciseRank.Pass:
                ++bronzeEarned;
                break;

              case ExerciseRank.Silver:
                ++bronzeEarned;
                ++silverEarned;
                break;

              case ExerciseRank.Gold:
                ++bronzeEarned;
                ++silverEarned;
                ++goldEarned;
                break;
            }
          }
        }
      }

      bronzeRanksEarnedText.text = $"Earned: {bronzeEarned}/10";
      silverRanksEarnedText.text = $"Earned: {silverEarned}/10";
      goldRanksEarnedText.text = $"Earned: {goldEarned}/10";

      // Pattern statistics
      loopsCorrectPercentageText.text = $"Loops Correct Percentage: {userProfile.GetPatternPercentageCorrect(PatternType.LeftLoop).ToString("F2")}%";
      whorlsCorrectPercentageText.text = $"Whorls Correct Percentage: {userProfile.GetPatternPercentageCorrect(PatternType.PlainWhorl).ToString("F2")}%";
      archesCorrectPercentageText.text = $"Arches Correct Percentage: {userProfile.GetPatternPercentageCorrect(PatternType.PlainArch).ToString("F2")}%";

      // Core statistics
      coreMatchedText.text = $"Number of Core Identified: {userProfile.GetAnnotatonsMatched(DetailType.Core)}";
      coreAverageAccuracyText.text = $"Average Accuracy: {userProfile.GetAnnotatonsAccuracy(DetailType.Core).ToString("F2")}%";
      coreAveragePrecisionText.text = $"Average Precision: {userProfile.GetAnnotatonsPrecision(DetailType.Core).ToString("F2")}%";

      // Delta statistics
      deltaMatchedText.text = $"Number of Delta Identified: {userProfile.GetAnnotatonsMatched(DetailType.Delta)}";
      deltaAverageAccuracyText.text = $"Average Accuracy: {userProfile.GetAnnotatonsAccuracy(DetailType.Delta).ToString("F2")}%";
      deltaAveragePrecisionText.text = $"Average Precision: {userProfile.GetAnnotatonsPrecision(DetailType.Delta).ToString("F2")}%";

      // Minutiae statistics
      bifurcationCorrectPercentageText.text = $"Bifurcation Correct Percentage: {userProfile.GetMinutiaePercentageCorrect(MinutiaeType.Bifurcation).ToString("F2")}%";
      ridgeEndingCorrectPercentageText.text = $"Ridge Ending Correct Percentage: {userProfile.GetMinutiaePercentageCorrect(MinutiaeType.RidgeEnding).ToString("F2")}%";
      dotCorrectPercentageText.text = $"Dot Correct Percentage: {userProfile.GetMinutiaePercentageCorrect(MinutiaeType.Dot).ToString("F2")}%";
      shortRidgeCorrectPercentageText.text = $"Short Ridge Correct Percentage: {userProfile.GetMinutiaePercentageCorrect(MinutiaeType.ShortRidge).ToString("F2")}%";
      minutiaeMatchedText.text = $"Number of Minutiae Matched: {userProfile.GetAnnotatonsMatched(DetailType.Minutiae)}";
      minutiaeAverageAccuracyText.text = $"Average Accuracy: {userProfile.GetAnnotatonsAccuracy(DetailType.Minutiae).ToString("F2")}%";

      // Achievements
      Dictionary<AchievementType, AchievementScriptableObject> allAchievementsDictionary = new Dictionary<AchievementType, AchievementScriptableObject>();

      foreach (AchievementScriptableObject achievementScriptableObject in allAchievements)
      {
        allAchievementsDictionary.Add(achievementScriptableObject.AchievementType, achievementScriptableObject);
      }

      foreach (AchievementType achievementType in Enum.GetValues(typeof(AchievementType)))
      {
        PopupScreen achievementTab = Instantiate(achievementTabPrefab, scrollRectContent.transform);
        AchievementScriptableObject achievement = allAchievementsDictionary[achievementType];
        achievementTab.SetContents(achievement.AchievementName, achievement.AchievementImage, achievement.AchievementDescription);

        GraphicsGradientParent[] graphicsGradientParents = achievementTab.GetComponentsInChildren<GraphicsGradientParent>();
        bool achievementUnlocked = userProfile.GetAchievementCompleted(achievementType);

        foreach (GraphicsGradientParent graphicsGradientParent in graphicsGradientParents)
        {
          if (achievementUnlocked) graphicsGradientParent.SetGraphicsPositive();
          else graphicsGradientParent.SetGraphicsNegative();
        }
      }
    }
  }
}
