using System;
using System.Text;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Monq.Core.TestExtensions
{
    /// <summary>
    /// Хеш-функции для внутренних типов.
    /// </summary>
    public static class HashExtensions
    {
        /// <summary>
        /// Получить SHA1 hash Для строки
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetShaHash(this string value)
        {
            using (var sha1 = SHA256.Create())
            {
                var buf = Encoding.UTF8.GetBytes(value);
                var hash = sha1.ComputeHash(buf, 0, buf.Length);
                var hashstr = BitConverter.ToString(hash).Replace("-", "");
                return hashstr;
            }
        }

        /// <summary>
        /// Сгенерировать GUID по MD5 хешу входного объекта.
        /// </summary>
        /// <param name="data">Входной объект.</param>
        /// <returns></returns>
        public static Guid GenerateFromSelf(this object data)
        {
            using (var md5 = MD5.Create())
            {
                var serializedObj = JsonConvert.SerializeObject(data);
                var hash = md5.ComputeHash(Encoding.Default.GetBytes(serializedObj));
                var result = new Guid(hash);
                return result;
            }
        }
    }
}