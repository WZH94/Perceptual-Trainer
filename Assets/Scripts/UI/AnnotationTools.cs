using System;
using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using FingerprintAnnotation;

  public class AnnotationTools : MonoBehaviour
  {
    private Dictionary<DetailType, Detail> annotationTools = new Dictionary<DetailType, Detail>();

    private void Awake()
    {
      Detail[] detailToolsList = GetComponentsInChildren<Detail>();

      foreach (Detail detailTool in detailToolsList)
      {
        annotationTools.Add(detailTool.DetailType, detailTool);
      }
    }

    public void SetAvailableTools( DetailType detailFlag )
    {
      if (detailFlag == 0)
      {
        gameObject.SetActive(false);
        return;
      }

      Array detailTypes = Enum.GetValues(typeof(DetailType));

      foreach (DetailType detailType in detailTypes)
      {
        if (!detailFlag.HasFlag(detailType))
        {
          annotationTools[detailType].gameObject.SetActive(false);
        }
      }
    }
  }
}
