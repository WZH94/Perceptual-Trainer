using System;

using UnityEngine;

namespace CwispyStudios.FingerprintTrainer
{
  [Serializable]
  public class SerializedVector2
  {
    public float x;
    public float y;

    public SerializedVector2( float x, float y )
    {
      this.x = x;
      this.y = y;
    }

    public SerializedVector2( Vector2 vector )
    {
      x = vector.x;
      y = vector.y;
    }
  }

  public static class SerializedVector2Extension
  {
    public static Vector2 ToVector2( this SerializedVector2 serializedVector2 )
    {
      return new Vector2(serializedVector2.x, serializedVector2.y);
    }

    public static SerializedVector2 FromVector2( this Vector2 vector2 )
    {
      return new SerializedVector2(vector2);
    }
  }
}
