using System;

namespace Jacobi.Vst3.Common
{
    public static class Guard
    {
        public static void ThrowIfNull<T>(string parameterName, T value) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void ThrowIfTooLong(string parameterName, string value, int minLength, int maxLength)
        {
            if (value != null &&
                (value.Length < minLength || value.Length > maxLength))
            {
                throw new ArgumentOutOfRangeException(parameterName, value,
                    String.Format("The length of the value '{0}' is not within range of {1}-{2}.", value, minLength, maxLength));
            }
        }

        public static void ThrowIfOutOfRange<T>(string parameterName, IComparable<T> value, T minValue, T maxValue)
        {
            if (value.CompareTo(minValue) < 0 || value.CompareTo(maxValue) > 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, value,
                    String.Format("The value '{0}' is not within range of {1}-{2}.", value, minValue, maxValue));
            }
        }
    }
}
