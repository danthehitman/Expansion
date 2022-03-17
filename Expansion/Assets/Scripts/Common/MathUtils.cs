using UnityEngine;

namespace Assets.Scripts.Common
{
    public class MathUtils
    {
        public static int GetSign(float v)
        {
            if (Mathf.Approximately(v, 0))
            {
                return 0;
            }
            else if (v > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}
