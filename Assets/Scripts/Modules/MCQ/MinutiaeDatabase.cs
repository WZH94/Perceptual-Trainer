using System;
using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.Modules.MCQ
{
  using FingerprintAnnotation;

  [RequireComponent(typeof(DatabaseLoader))]
  public class MinutiaeDatabase : MonoBehaviour
  {
    private DatabaseLoader minutiaeLoader;
    private Dictionary<MinutiaeType, List<Sprite>> minutiaeDatabase = new Dictionary<MinutiaeType, List<Sprite>>();
    private List<Sprite> allMinutiaeImages = new List<Sprite>();

    private void Awake()
    {
      minutiaeLoader = GetComponent<DatabaseLoader>();
      LoadAndParseMinutiaeImages();
    }

    private void LoadAndParseMinutiaeImages()
    {
      List<Sprite> minutiaeList = minutiaeLoader.LoadMinutiaeImages();

      foreach (Sprite minutiaeImage in minutiaeList)
      {
        MinutiaeType minutiaeType = MinutiaeType.None;

        switch (minutiaeImage.name[0])
        {
          // Bifurcation
          case 'b':
            minutiaeType = MinutiaeType.Bifurcation;
            break;

          // Ridge Ending
          case 'e':
            minutiaeType = MinutiaeType.RidgeEnding;
            break;

          // Dot
          case 'd':
            minutiaeType = MinutiaeType.Dot;
            break;

          // Short Ridge
          case 's':
            minutiaeType = MinutiaeType.ShortRidge;
            break;
        }

        if (minutiaeType == MinutiaeType.None) Debug.LogError($"Invalid minutiae type! {minutiaeImage.name}");

        if (!minutiaeDatabase.ContainsKey(minutiaeType)) minutiaeDatabase.Add(minutiaeType, new List<Sprite>());
        minutiaeDatabase[minutiaeType].Add(minutiaeImage);
        allMinutiaeImages.Add(minutiaeImage);
      }
    }

    public MinutiaeType GetMinutiaeTypeOfImage( Sprite image )
    {
      foreach (MinutiaeType minutiaeType in minutiaeDatabase.Keys)
      {
        if (minutiaeDatabase[minutiaeType].Contains(image)) return minutiaeType;
      }

      return MinutiaeType.None;
    }

    public Sprite GetMinutiaeAndRemoveFromPool()
    {
      int randomIndex = UnityEngine.Random.Range(0, allMinutiaeImages.Count);
      Sprite image = allMinutiaeImages[randomIndex];
      allMinutiaeImages.RemoveAt(randomIndex);

      return image;
    }
  }
}
