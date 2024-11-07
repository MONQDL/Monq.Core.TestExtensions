using System;
using System.Linq;

namespace Monq.Core.TestExtensions
{
    /// <summary>
    /// Методы расширения для <see cref="Random"/>.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Сгенерировать случайный идентификатор для сущности.
        /// </summary>
        /// <param name="sporadic">ГПСЧ.</param>
        /// <param name="minValue">Минимальное возможное значение.</param>
        /// <returns></returns>
        public static long GetId(this Random sporadic, in long minValue = 1) =>
            sporadic.Next(minValue, long.MaxValue);

        /// <summary>
        /// Сгенерировать случайное наименование.
        /// </summary>
        /// <param name="sporadic">ГПСЧ.</param>
        /// <param name="minLength">Мин. длина строки.</param>
        /// <param name="maxLength">Макс. длина строки.</param>
        /// <returns></returns>
        public static string GetRandomName(this Random sporadic, in int minLength = 4, in int maxLength = 16) =>
            InsertRandomSpaces(sporadic,
                GetRandomString(sporadic,
                    sporadic.Next(minLength, maxLength + 1)), 0.17);

        /// <summary>
        /// Сгенерировать случайное описание
        /// </summary>
        /// <param name="sporadic">ГПСЧ.</param>
        /// <param name="minLength">Мин. длина строки.</param>
        /// <param name="maxLength">Макс. длина строки.</param>
        /// <returns></returns>
        public static string GetRandomDescription(this Random sporadic, in int minLength = 8, in int maxLength = 24) =>
            InsertRandomSpaces(sporadic,
                GetRandomString(sporadic,
                    sporadic.Next(minLength, maxLength + 1)));

        /// <summary>
        /// Сгенерировать случайное число из указанного диапазона.
        /// </summary>
        /// <param name="sporadic">ГПСЧ.</param>
        /// <param name="min">Мин. граница диапазона.</param>
        /// <param name="max">Макс. граница диапазона.</param>
        /// <returns></returns>
        public static long Next(this Random sporadic, in long min, in long max)
        {
            var buf = new byte[8];
            sporadic.NextBytes(buf);
            var longRand = BitConverter.ToInt64(buf, 0);

            return Math.Abs(longRand % (max - min)) + min;
        }

        static string GetRandomString(Random sporadic, in int length, string chars = "aeiouwjptksmnl")
            => new string(Enumerable.Repeat(chars, length)
                .Select(s => s[sporadic.Next(s.Length)]).ToArray());

        static string InsertRandomSpaces(Random sporadic, string input, double probability = 0.3) =>
            InsertCharsSporadically(sporadic, input, ' ', probability);

        static string InsertCharsSporadically(Random sporadic, string input, char character, double probalility = 0.5) =>
            input.Aggregate(
                string.Empty,
                (result, currentChar) =>
                    sporadic.NextDouble() <= probalility
                        ? result + currentChar + character
                        : result + currentChar);
    }
}