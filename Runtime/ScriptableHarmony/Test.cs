using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Test
{
   [RuntimeInitializeOnLoadMethod]
   public static void TestSwag()
   {
      Debug.Log(Resources.Load<Texture2D>("Capsule").name);
   }
}
