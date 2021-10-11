using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  public class DatabaseLoader : MonoBehaviour
  {
    private const string FingerprintDatabaseFolderName = "Fingerprint Database";
    private const string MinutiaeDatabaseFolderName = "Minutiae Database";

    public List<FingerprintDetails> LoadFingerprintDetails()
    {
      List<FingerprintDetails> fingerprintDetailsList = new List<FingerprintDetails>();

      Object[] fingerprintDetailObjects = Resources.LoadAll(FingerprintDatabaseFolderName, typeof(TextAsset));

      foreach (Object fingerprintDetailObject in fingerprintDetailObjects)
      {
        FingerprintDetails fingerprintDetailsJson = JsonUtility.FromJson<FingerprintDetails>(((TextAsset)fingerprintDetailObject).text);
        fingerprintDetailsList.Add(fingerprintDetailsJson);
      }

      return fingerprintDetailsList;
    }

    public List<Sprite> LoadMinutiaeImages()
    {
      List<Sprite> minutiaeImagesList = new List<Sprite>();

      Object[] minutiaeImageObjects = Resources.LoadAll(MinutiaeDatabaseFolderName, typeof(Sprite));

      foreach (Object minutiaeImageObject in minutiaeImageObjects)
      {
        minutiaeImagesList.Add((Sprite)minutiaeImageObject);
      }

      return minutiaeImagesList;
    }
  }
}
