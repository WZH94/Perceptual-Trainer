namespace CwispyStudios.FingerprintTrainer.User
{
  [System.Serializable]
  public class CategoryStatistics
  {
    public int NumberEncountered;
    public int NumberAnsweredCorrectly;
    public float PercentageCorrect;

    public CategoryStatistics()
    {
      NumberEncountered = 0;
      NumberAnsweredCorrectly = 0;
      PercentageCorrect = 0f;
    }

    public void AddStatistics( CategoryStatistics statisticsToAdd )
    {
      NumberEncountered += statisticsToAdd.NumberEncountered;
      NumberAnsweredCorrectly += statisticsToAdd.NumberAnsweredCorrectly;
      UpdatePercentageCorrect();
    }

    public void UpdatePercentageCorrect()
    {
      PercentageCorrect = (float)NumberAnsweredCorrectly / (float)NumberEncountered * 100f;
    }
  }
}
