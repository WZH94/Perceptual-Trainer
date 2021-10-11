using UnityEngine;

using TMPro;

public class TestPath : MonoBehaviour
{
  private TMP_Text testText;

  private void Awake()
  {
    testText = GetComponent<TMP_Text>();

    testText.text = Application.persistentDataPath;
  }
}
