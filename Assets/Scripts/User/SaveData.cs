using System;
using System.Collections.Generic;

namespace CwispyStudios.FingerprintTrainer.User
{
  using Achievements;
  using Modules;

  [Serializable]
  public class SaveData
  {
    // Seconds
    public float TotalTimeSpentInLessonsAndExercises;

    // Module completion
    public List<bool> LessonsCompleted;
    public List<List<ExerciseResultsData>> ExercisesCompletion;

    public int NumberBronzeStars;
    public int NumberSilverStars;
    public int NumberGoldStars;

    // Pattern accuracy
    public CategoryStatistics LoopsStatistics;
    public CategoryStatistics WhorlsStatistics;
    public CategoryStatistics ArchesStatistics;

    // Annotation statistics for core, delta, and minutiae
    public AnnotationStatistics CoreStatistics;
    public AnnotationStatistics DeltaStatistics;
    public AnnotationStatistics MinutiaeStatistics;

    // Minutiae MCQ accuracy
    public CategoryStatistics BifurcationStatistics;
    public CategoryStatistics RidgeEndingStatistics;
    public CategoryStatistics DotStatistics;
    public CategoryStatistics ShortRidgeStatistics;

    public float CompletionPerc;

    public bool ProgrammesPopupEncountered;
    public bool CoursesPopupEncountered;
    public bool LessonsPopupEncountered;
    public bool HoverImageEncountered;
    public bool PopupImageEncountered;

    // Achievements
    public List<bool> AchievementsCompleted;

    public SaveData()
    {
      TotalTimeSpentInLessonsAndExercises = 0f;

      LessonsCompleted = new List<bool>();
      ExercisesCompletion = new List<List<ExerciseResultsData>>();

      // Loop through every lesson/exercise type
      for (int i = 0; i < (int)LessonExerciseType.Count; ++i)
      {
        LessonsCompleted.Add(false);
        // Each exercise has 3 possible difficulties
        ExercisesCompletion.Add(new List<ExerciseResultsData>());

        // Beginner, Intermediate, Expert difficulties per exercise
        for (int difficulty = 0; difficulty < 3; ++difficulty)
        {
          ExercisesCompletion[i].Add(new ExerciseResultsData());
        }
      }

      LoopsStatistics = new CategoryStatistics();
      WhorlsStatistics = new CategoryStatistics();
      ArchesStatistics = new CategoryStatistics();

      CoreStatistics = new AnnotationStatistics();
      DeltaStatistics = new AnnotationStatistics();
      MinutiaeStatistics = new AnnotationStatistics();

      BifurcationStatistics = new CategoryStatistics();
      RidgeEndingStatistics = new CategoryStatistics();
      DotStatistics = new CategoryStatistics();
      ShortRidgeStatistics = new CategoryStatistics();

      NumberBronzeStars = 0;
      NumberSilverStars = 0;
      NumberGoldStars = 0;

      CompletionPerc = 0f;

      ProgrammesPopupEncountered = false;
      CoursesPopupEncountered = false;
      HoverImageEncountered = false;
      PopupImageEncountered = false;

      AchievementsCompleted = new List<bool>();

      for (int i = 0; i < Enum.GetValues(typeof(AchievementType)).Length; ++i)
      {
        AchievementsCompleted.Add(false);
      }
    }

    public int GetNumberOfRanks( ExerciseRank exerciseRank )
    {
      int number = 0;

      foreach (List<ExerciseResultsData> exerciseResultsDatas in ExercisesCompletion)
      {
        foreach (ExerciseResultsData exerciseResultsData in exerciseResultsDatas)
        {
          if (exerciseResultsData.ExerciseRank >= exerciseRank) ++number;
        }
      }

      return number;
    }
  }
}
