using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.Achievements
{
  public class AchievementScriptableObject : ScriptableObject
  {
    public AchievementType AchievementType;
    public string AchievementName;
    public string AchievementDescription;
    public Sprite AchievementImage;
  }
}
