using System;

namespace CommunicationSample
{
    public static class ArrayExtensions
    {
        public static T[] SubArray<T>(this T[] data, long index, long length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
