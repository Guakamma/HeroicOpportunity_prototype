using Input;
using UnityEngine;

namespace Extensions
{
    public static class Extensions
    {
        public static void Mirror(this ref Direction direction)
        {
            if (direction == Direction.Left)
            {
                direction = Direction.Right;
            }
            else if (direction == Direction.Right)
            {
                direction = Direction.Left;
            }
        }
        
        public static bool RandomBool => Random.Range(0, 1) == 1;
    }
}