using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.Achievements
{
  using Modules;

  [CreateAssetMenu(fileName = "Rank Achievements", menuName = "Scriptable Objects/Achievements/Rank Achievements", order = 1)]
  public class RankAchievementScriptableObject : AchievementScriptableObject
  {
    public ExerciseRank ExerciseRank;
    public int Number;
  }
}
