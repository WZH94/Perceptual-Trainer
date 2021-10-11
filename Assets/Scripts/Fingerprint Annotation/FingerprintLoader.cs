using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  [RequireComponent(typeof(Image))]
  public class FingerprintLoader : MonoBehaviour
  {
    private Image fingerprintImage;

    private FingerprintDetails loadedFingerprintDetails;

    private void Awake()
    {
      fingerprintImage = GetComponent<Image>();
    }

    public void LoadFingerprintDetails( FingerprintDetails fingerprintDetails )
    {
      loadedFingerprintDetails = fingerprintDetails;
      fingerprintImage.sprite = loadedFingerprintDetails.FingerprintImage;
    }
  }
}
