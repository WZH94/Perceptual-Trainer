namespace CwispyStudios.FingerprintTrainer.Modules
{
  [System.Serializable]
  public class ExerciseResultsData
  {
    public int Attempts;
    public ExerciseRank ExerciseRank;
    public float HighScore;

    public ExerciseResultsData()
    {
      Attempts = 0;
      ExerciseRank = ExerciseRank.Fail;
      HighScore = 0f;
    }
  }
}
