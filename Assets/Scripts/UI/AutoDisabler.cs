using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.FingerprintTrainer.UI
{
  public class AutoDisabler : MonoBehaviour
  {
    [SerializeField] private float secondsToDisable = 5f;
    [SerializeField] private float secondsToFadeOut = 2f;

    private float secondsCounter = 0f;

    private bool stopUpdate = false;

    private void Update()
    {
      if (secondsCounter < secondsToDisable) secondsCounter += Time.deltaTime;
      else if (!stopUpdate)
      {
        stopUpdate = true; 
        StartCoroutine(FadeOutAndDestroy());
      }
    }

    private IEnumerator FadeOutAndDestroy()
    {
      Graphic[] graphics = GetComponentsInChildren<Graphic>();
      foreach (Graphic graphic in graphics) graphic.CrossFadeAlpha(0f, secondsToFadeOut, false);

      float counter = 0f;

      while (counter < secondsToFadeOut)
      {
        counter += Time.deltaTime;
        yield return null;
      }

      Destroy(gameObject);
    }
  }
}
