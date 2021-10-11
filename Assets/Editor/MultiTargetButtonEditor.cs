using UnityEditor;
using UnityEditor.UI;

namespace CwispyStudios.FingerprintTrainer.Editor
{
  using UI;

  [CustomEditor(typeof(MultiTargetButton))]
  public class MultiTargetButtonEditor : ButtonEditor
  {
    public override void OnInspectorGUI()
    {
      MultiTargetButton multiTargetButton = (MultiTargetButton)target;

      multiTargetButton.IgnoreImages = EditorGUILayout.Toggle("Ignore Images", multiTargetButton.IgnoreImages);

      base.OnInspectorGUI();
    }
  }
}
