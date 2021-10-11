namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  [System.Flags]
  public enum DetailType
  {
    Core = (1 << 0),
    Delta = (1 << 1),
    Minutiae = (1 << 2)
  }
}
