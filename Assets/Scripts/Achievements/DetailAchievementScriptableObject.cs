using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.Achievements
{
  using FingerprintAnnotation;

  [CreateAssetMenu(fileName = "Detail Achievements", menuName = "Scriptable Objects/Achievements/Detail Achievements", order = 2)]
  public class DetailAchievementScriptableObject : AchievementScriptableObject
  {
    public DetailType DetailType;
    public int Number;
  }
}
