using UnityEngine;
using UnityEngine.SceneManagement;

namespace CwispyStudios.FingerprintTrainer.SceneNavigation
{
  public class SceneLoader : MonoBehaviour
  {
    [SerializeField] private string sceneToLoad;

    public void LoadScene()
    {
      SceneManager.LoadScene(sceneToLoad);
    }

    public void SetSceneToLoad( string sceneName )
    {
      sceneToLoad = sceneName;
    }

    public void ReloadScene()
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
  }
}
