namespace NuiN.SpleenTween
{
    public enum Loop
    {
        None = 0,
        Restart,
        Rewind,
        Yoyo,
        Increment
    }

    public static class Looping
    {
        /// <summary>
        /// Only calls when loop type is "Increment" to increase the to and from values by the difference between them
        /// </summary>
        public static void RestartLoopTypes<T>(Loop loopType, ref T from, ref T to)
        {
            switch (loopType)
            {
                case Loop.Increment: RestartIncrementLoop(ref from, ref to); break;
            }
        }

        /// <summary>
        /// Checks if loop is either type Yoyo or Rewind, which are weird because they go backwards
        /// </summary>
        public static bool IsLoopWeird(Loop loopType)
        {
            return loopType is Loop.Yoyo or Loop.Rewind;
        }

        static void RestartIncrementLoop<T>(ref T from, ref T to)
        {
            T diff = SpleenExt.SubtractGenerics(to, from);
            from = SpleenExt.AddGenerics(from, diff);
            to = SpleenExt.AddGenerics(to, diff);
        }
    }

}


