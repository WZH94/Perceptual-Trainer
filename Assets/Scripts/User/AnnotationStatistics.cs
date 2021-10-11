namespace CwispyStudios.FingerprintTrainer.User
{
  [System.Serializable]
  public class AnnotationStatistics
  {
    public int NumberOfExerciseAnnotations;
    public int NumberMatched;
    public float AverageAccuracy;
    public float AveragePrecision;

    public AnnotationStatistics()
    {
      NumberOfExerciseAnnotations = 0;
      NumberMatched = 0;
      AverageAccuracy = 0f;
      AveragePrecision = 0f;
    }

    public void AddStatistics( AnnotationStatistics statisticsToAdd )
    {
      ++NumberOfExerciseAnnotations;
      NumberMatched += statisticsToAdd.NumberMatched;
      AverageAccuracy = AverageAccuracy + (statisticsToAdd.AverageAccuracy - AverageAccuracy) / NumberOfExerciseAnnotations;
      AveragePrecision = AveragePrecision + (statisticsToAdd.AveragePrecision - AveragePrecision) / NumberOfExerciseAnnotations;
    }
  }
}
