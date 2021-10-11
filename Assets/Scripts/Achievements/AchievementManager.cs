using UnityEngine;
using UnityEngine.SceneManagement;

namespace CwispyStudios.FingerprintTrainer.Achievements
{
  using FingerprintAnnotation;
  using Modules;
  using UI;
  using User;

  [RequireComponent(typeof(UserProfile))]
  public class AchievementManager : MonoBehaviour
  {
    [Header("Prefabs")]
    [SerializeField] private RectTransform achievementNotificationPanelPrefab = null;
    [SerializeField] private PopupScreen achievementNotificationPrefab = null;
    [Header("Achievements")]
    [SerializeField] private ModuleAchievementScriptableObject[] moduleAchievements = null;
    [SerializeField] private RankAchievementScriptableObject[] rankAchievements = null;
    [SerializeField] private DetailAchievementScriptableObject[] detailAchievements = null;

    private UserProfile userProfile;
    private RectTransform achievementNotificationPanel = null;

    private void Awake()
    {
      SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
      Initialise();
    }

    private void OnSceneLoaded( Scene scene, LoadSceneMode mode )
    {
      achievementNotificationPanel = Instantiate(achievementNotificationPanelPrefab, FindObjectOfType<Canvas>().transform);
    }

    private void Initialise()
    {
      userProfile = GetComponent<UserProfile>();
      userProfile.OnModuleCompleted += CheckModuleAchievements;
      userProfile.OnRankAchieved += CheckRankAchievements;
      userProfile.OnDetailsAnnotated += CheckDetailsAchievements;
    }

    private void CheckModuleAchievements( ModuleType moduleType, LessonExerciseType lessonExerciseType )
    {
      foreach (ModuleAchievementScriptableObject moduleAchievement in moduleAchievements)
      {
        if (moduleAchievement.ModuleType == moduleType && moduleAchievement.LessonExerciseType == lessonExerciseType)
        {
          AchievementType achievementType = moduleAchievement.AchievementType;

          if (!userProfile.GetAchievementCompleted(achievementType))
          {
            userProfile.SetAchievementComplete(achievementType);

            ShowAchievementNotification(moduleAchievement);
          }
        }
      }
    }

    private void CheckRankAchievements( ExerciseRank exerciseRank, int number )
    {
      foreach (RankAchievementScriptableObject rankAchievement in rankAchievements)
      {
        if (rankAchievement.ExerciseRank <= exerciseRank && rankAchievement.Number <= number)
        {
          AchievementType achievementType = rankAchievement.AchievementType;

          if (!userProfile.GetAchievementCompleted(achievementType))
          {
            userProfile.SetAchievementComplete(achievementType);

            ShowAchievementNotification(rankAchievement);
          }
        }
      }
    }

    private void CheckDetailsAchievements( DetailType detailType, int number )
    {
      foreach (DetailAchievementScriptableObject detailAchievement in detailAchievements)
      {
        if (detailAchievement.DetailType == detailType && detailAchievement.Number <= number)
        {
          AchievementType achievementType = detailAchievement.AchievementType;

          if (!userProfile.GetAchievementCompleted(achievementType))
          {
            userProfile.SetAchievementComplete(achievementType);

            ShowAchievementNotification(detailAchievement);
          }
        }
      }
    }

    private void ShowAchievementNotification( AchievementScriptableObject achievement )
    {
      Instantiate(achievementNotificationPrefab, achievementNotificationPanel.transform).SetContents(
        achievement.AchievementName, achievement.AchievementImage, achievement.AchievementDescription);
    }
  }
}
