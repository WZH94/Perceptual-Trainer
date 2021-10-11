using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.FingerprintTrainer.UI
{
  public class MultiTargetButton : Button
  {
    public bool IgnoreImages;

    private Graphic[] graphics;

    protected override void Awake()
    {
      base.Awake();

      graphics = GetComponentsInChildren<Graphic>();
    }

    protected override void DoStateTransition( SelectionState state, bool instant )
    {
      base.DoStateTransition(state, instant);

      Color targetColour =
        state == SelectionState.Disabled ? colors.disabledColor :
        state == SelectionState.Highlighted ? colors.highlightedColor :
        state == SelectionState.Normal ? colors.normalColor :
        state == SelectionState.Pressed ? colors.pressedColor :
        state == SelectionState.Selected ? colors.selectedColor : Color.white;

      if (graphics != null)
      {
        foreach (Graphic graphic in graphics)
        {
          if (!IgnoreImages || (IgnoreImages && graphic.GetComponent<Image>() == null))
            graphic.CrossFadeColor(targetColour, instant ? 0 : colors.fadeDuration, true, true);
        }
      }
    }
  }
}
