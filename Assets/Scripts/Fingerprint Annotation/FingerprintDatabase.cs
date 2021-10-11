using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  [RequireComponent(typeof(DatabaseLoader))]
  public class FingerprintDatabase : SingletonObject<FingerprintDatabase>
  {
    private DatabaseLoader jsonLoader;

    private Dictionary<PatternType, List<FingerprintDetails>> fingerprintDatabase = new Dictionary<PatternType, List<FingerprintDetails>>();

    private bool databaseLoaded = false;

    private const string DatabaseDirectory = "Fingerprint Database\\";

    private void Awake()
    {
      if (!databaseLoaded)
      {
        jsonLoader = GetComponent<DatabaseLoader>();

        LoadAndParseFingerprintDetails();
      }
    }

    private void LoadAndParseFingerprintDetails()
    {
      List<FingerprintDetails> fingerprintDetailsList = jsonLoader.LoadFingerprintDetails();

      foreach (FingerprintDetails fingerprintDetails in fingerprintDetailsList)
      {
        fingerprintDetails.FingerprintImage = Resources.Load<Sprite>(DatabaseDirectory + fingerprintDetails.FingerprintSpriteName);

        PatternType patternType = fingerprintDetails.PatternType;

        if (!fingerprintDatabase.ContainsKey(patternType)) fingerprintDatabase.Add(patternType, new List<FingerprintDetails>());

        fingerprintDatabase[patternType].Add(fingerprintDetails);
      }

      databaseLoaded = true;
    }

    public IList<FingerprintDetails> GetFingerprintsOfPatternType( PatternType patternType )
    {
      return fingerprintDatabase.ContainsKey(patternType) ? fingerprintDatabase[patternType] : new List<FingerprintDetails>();
    }
  }
}
