namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  [System.Flags]
  public enum PatternType
  {
    LeftLoop = (1 << 0),
    RightLoop = (1 << 1),
    //Loop = (LeftLoop | RightLoop),
    PlainWhorl = (1 << 2),
    CentralPocketLoop = (1 << 3),
    DoubleLoopWhorl = (1 << 4),
    AccidentalWhorl = (1 << 5),
    //Whorl = (PlainWhorl | CentralPocketLoop | DoubleLoopWhorl | AccidentalWhorl),
    PlainArch = (1 << 6),
    TentedArch = (1 << 7),
    //Arch = (PlainArch | TentedArch)
  }
}
