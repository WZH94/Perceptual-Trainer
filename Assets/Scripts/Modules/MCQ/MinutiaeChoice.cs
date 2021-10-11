using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.Modules.MCQ
{
  public class MinutiaeChoice : MonoBehaviour
  {
    [SerializeField] private MinutiaeType minutiaeType;

    public MinutiaeType MinutiaeType => minutiaeType;
  }
}
