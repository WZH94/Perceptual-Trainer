using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.User
{
  using Achievements;
  using FingerprintAnnotation;
  using Modules;
  using Modules.MCQ;

  public class UserProfile : SingletonObject<UserProfile>
  {
    private bool isLoaded = false;
    private SaveData userData;

    private string savePath;

    public Action<ModuleType, LessonExerciseType> OnModuleCompleted;
    public Action<ExerciseRank, int> OnRankAchieved;
    public Action<DetailType, int> OnDetailsAnnotated;

    private void Awake()
    {
      if (!isLoaded && FindObjectsOfType<UserProfile>().Length > 1) Destroy(gameObject);
      DontDestroyOnLoad(this);
      savePath = Application.persistentDataPath + "/usersave.save";

      if (!isLoaded)
      {
        LoadData();
      }
    }

    private void LoadData()
    {
      if (File.Exists(savePath))
      {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(savePath, FileMode.Open);
        userData = (SaveData)bf.Deserialize(file);
        file.Close();
      }

      else CreateSaveData();
    }

    private void CreateSaveData()
    {
      userData = new SaveData();

      SaveData();
    }

    private void SaveData()
    {
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Create(savePath);
      bf.Serialize(file, userData);
      file.Close();
    }

    public void DeleteSaveData()
    {
      File.Delete(savePath);

      Application.Quit();
    }

    public void AddTimeSpentInLessonsAndExercises( float seconds )
    {
      userData.TotalTimeSpentInLessonsAndExercises += seconds;

      SaveData();
    }

    public float GetTimeSpentInLessonsAndExercises()
    {
      return userData.TotalTimeSpentInLessonsAndExercises;
    }

    public void SetLessonCompletion( LessonExerciseType lessonType )
    {
      if (!userData.LessonsCompleted[(int)lessonType])
      {
        userData.LessonsCompleted[(int)lessonType] = true;
        SaveData();

        OnModuleCompleted?.Invoke(ModuleType.Lesson, lessonType);
      }
    }

    public bool GetLessonCompletion( LessonExerciseType lessonType )
    {
      return userData.LessonsCompleted[(int)lessonType];
    }

    public void AddExerciseAttempt( LessonExerciseType exerciseType, ExerciseDifficulty exerciseDifficulty )
    {
      ++userData.ExercisesCompletion[(int)exerciseType][(int)exerciseDifficulty].Attempts;
      SaveData();
    }

    public int GetExerciseAttempts( LessonExerciseType exerciseType, ExerciseDifficulty exerciseDifficulty )
    {
      return userData.ExercisesCompletion[(int)exerciseType][(int)exerciseDifficulty].Attempts;
    }

    public void SetExerciseResultsData( LessonExerciseType exerciseType, ExerciseDifficulty exerciseDifficulty, float score, ExerciseRank exerciseRank )
    {
      ExerciseResultsData exerciseResultsData = userData.ExercisesCompletion[(int)exerciseType][(int)exerciseDifficulty];

      if (score > exerciseResultsData.HighScore)
      {
        exerciseResultsData.HighScore = score;
        exerciseResultsData.ExerciseRank = exerciseRank;

        OnRankAchieved?.Invoke(exerciseRank, userData.GetNumberOfRanks(exerciseRank));
      }

      SaveData();

      if (score >= 50f) OnModuleCompleted?.Invoke(ModuleType.Exercise, exerciseType);
    }

    public float GetExerciseHighscore( LessonExerciseType exerciseType, ExerciseDifficulty exerciseDifficulty )
    {
      return userData.ExercisesCompletion[(int)exerciseType][(int)exerciseDifficulty].HighScore;
    }

    public ExerciseRank GetExerciseRank( LessonExerciseType exerciseType, ExerciseDifficulty exerciseDifficulty )
    {
      return userData.ExercisesCompletion[(int)exerciseType][(int)exerciseDifficulty].ExerciseRank;
    }

    public void UpdatePatternStatistics( PatternType patternType, CategoryStatistics patternStatistics )
    {
      switch (patternType)
      {
        case PatternType.LeftLoop:
        case PatternType.RightLoop:
          userData.LoopsStatistics.AddStatistics(patternStatistics);
          break;

        case PatternType.PlainWhorl:
        case PatternType.CentralPocketLoop:
        case PatternType.DoubleLoopWhorl:
        case PatternType.AccidentalWhorl:
          userData.WhorlsStatistics.AddStatistics(patternStatistics);
          break;

        case PatternType.PlainArch:
        case PatternType.TentedArch:
          userData.ArchesStatistics.AddStatistics(patternStatistics);
          break;
      }

      SaveData();
    }

    public int GetNumberOfPatternEncounters( PatternType patternType )
    {
      switch (patternType)
      {
        case PatternType.LeftLoop:
        case PatternType.RightLoop:
          return userData.LoopsStatistics.NumberEncountered;

        case PatternType.PlainWhorl:
        case PatternType.CentralPocketLoop:
        case PatternType.DoubleLoopWhorl:
        case PatternType.AccidentalWhorl:
          return userData.WhorlsStatistics.NumberEncountered;

        case PatternType.PlainArch:
        case PatternType.TentedArch:
          return userData.ArchesStatistics.NumberEncountered;
      }

      return 0;
    }

    public float GetPatternPercentageCorrect( PatternType patternType )
    {
      switch (patternType)
      {
        case PatternType.LeftLoop:
        case PatternType.RightLoop:
          return userData.LoopsStatistics.PercentageCorrect;

        case PatternType.PlainWhorl:
        case PatternType.CentralPocketLoop:
        case PatternType.DoubleLoopWhorl:
        case PatternType.AccidentalWhorl:
          return userData.WhorlsStatistics.PercentageCorrect;

        case PatternType.PlainArch:
        case PatternType.TentedArch:
          return userData.ArchesStatistics.PercentageCorrect;
      }

      return 0f;
    }

    public void UpdateAnnotationStatistics( DetailType detailType, AnnotationStatistics annotationStatistics )
    {
      switch (detailType)
      {
        case DetailType.Core:
          userData.CoreStatistics.AddStatistics(annotationStatistics);
          break;

        case DetailType.Delta:
          userData.DeltaStatistics.AddStatistics(annotationStatistics);
          break;

        case DetailType.Minutiae:
          userData.MinutiaeStatistics.AddStatistics(annotationStatistics);
          break;
      }

      SaveData();

      OnDetailsAnnotated?.Invoke(detailType, GetAnnotatonsMatched(detailType));
    }

    public int GetAnnotatonsMatched( DetailType detailType )
    {
      switch (detailType)
      {
        case DetailType.Core:
          return userData.CoreStatistics.NumberMatched;

        case DetailType.Delta:
          return userData.DeltaStatistics.NumberMatched;

        case DetailType.Minutiae:
          return userData.MinutiaeStatistics.NumberMatched;
      }

      return 0;
    }

    public float GetAnnotatonsAccuracy( DetailType detailType )
    {
      switch (detailType)
      {
        case DetailType.Core:
          return userData.CoreStatistics.AverageAccuracy;

        case DetailType.Delta:
          return userData.DeltaStatistics.AverageAccuracy;

        case DetailType.Minutiae:
          return userData.MinutiaeStatistics.AverageAccuracy;
      }

      return 0f;
    }
    public float GetAnnotatonsPrecision( DetailType detailType )
    {
      switch (detailType)
      {
        case DetailType.Core:
          return userData.CoreStatistics.AveragePrecision;

        case DetailType.Delta:
          return userData.DeltaStatistics.AveragePrecision;

        case DetailType.Minutiae:
          return userData.MinutiaeStatistics.AveragePrecision;
      }

      return 0f;
    }

    public void UpdateMinutiaeCategoryStatistics( MinutiaeType minutiaeType, CategoryStatistics minutiaeStatistics )
    {
      switch (minutiaeType)
      {
        case MinutiaeType.Bifurcation:
          userData.BifurcationStatistics.AddStatistics(minutiaeStatistics);
          break;

        case MinutiaeType.RidgeEnding:
          userData.RidgeEndingStatistics.AddStatistics(minutiaeStatistics);
          break;

        case MinutiaeType.Dot:
          userData.DotStatistics.AddStatistics(minutiaeStatistics);
          break;

        case MinutiaeType.ShortRidge:
          userData.ShortRidgeStatistics.AddStatistics(minutiaeStatistics);
          break;
      }

      SaveData();
    }

    public float GetMinutiaePercentageCorrect( MinutiaeType minutiaeType )
    {
      switch (minutiaeType)
      {
        case MinutiaeType.Bifurcation:
          return userData.BifurcationStatistics.PercentageCorrect;

        case MinutiaeType.RidgeEnding:
          return userData.RidgeEndingStatistics.PercentageCorrect;

        case MinutiaeType.Dot:
          return userData.DotStatistics.PercentageCorrect;

        case MinutiaeType.ShortRidge:
          return userData.ShortRidgeStatistics.PercentageCorrect;
      }

      return 0f;
    }

    public int GetNumberOfMinutiaeEncounters( MinutiaeType minutiaeType )
    {
      switch (minutiaeType)
      {
        case MinutiaeType.Bifurcation:
          return userData.BifurcationStatistics.NumberEncountered;

        case MinutiaeType.RidgeEnding:
          return userData.RidgeEndingStatistics.NumberEncountered;

        case MinutiaeType.Dot:
          return userData.DotStatistics.NumberEncountered;

        case MinutiaeType.ShortRidge:
          return userData.ShortRidgeStatistics.NumberEncountered;
      }

      return 0;
    }

    public void SetProgrammePopupEncountered()
    {
      userData.ProgrammesPopupEncountered = true;

      SaveData();
    }

    public void SetCoursePopupEncountered()
    {
      userData.CoursesPopupEncountered = true;

      SaveData();
    }

    public void SetHoverImageEncountered()
    {
      userData.HoverImageEncountered = true;

      SaveData();
    }

    public void SetPopupImageEncountered()
    {
      userData.PopupImageEncountered = true;

      SaveData();
    }

    public bool GetProgrammePopupEncountered()
    {
      return userData.ProgrammesPopupEncountered;
    }

    public bool GetCoursePopupEncountered()
    {
      return userData.CoursesPopupEncountered;
    }

    public bool GetHoverImageEncountered()
    {
      return userData.HoverImageEncountered;
    }

    public bool GetPopupImageEncountered()
    {
      return userData.PopupImageEncountered;
    }

    public void SetAchievementComplete( AchievementType achievementType )
    {
      userData.AchievementsCompleted[(int)achievementType] = true;
    }

    public bool GetAchievementCompleted( AchievementType achievementType )
    {
      return userData.AchievementsCompleted[(int)achievementType];
    }
  }
}
