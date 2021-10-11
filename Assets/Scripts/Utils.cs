using System.Text.RegularExpressions;

namespace CwispyStudios.FingerprintTrainer
{
  using FingerprintAnnotation;

  public static class Utils
  {
    public static string AddSpaceBeforeCapitals( string stringToNicify )
    {
      return Regex.Replace(stringToNicify, "(\\B[A-Z])", " $1");
    }

    public static string ConvertPatternTypeToBasicName( PatternType patternType )
    {
      switch (patternType)
      {
        case PatternType.LeftLoop:
        case PatternType.RightLoop:
          return "Loop";

        case PatternType.PlainWhorl:
        case PatternType.CentralPocketLoop:
        case PatternType.DoubleLoopWhorl:
        case PatternType.AccidentalWhorl:
          return "Whorl";

        case PatternType.PlainArch:
        case PatternType.TentedArch:
          return "Arch";

        default: return "";
      }
    }
  }
}
