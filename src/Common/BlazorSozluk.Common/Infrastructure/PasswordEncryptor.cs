using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Common.Infrastructure
{
    public class PasswordEncryptor
    {
        public static string Encrpt(string password)
        {
            using var md5 = MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(password); // gelen sifrenin byte array'e cevrilmesi islemi
            byte[] hashBytes = md5.ComputeHash(inputBytes); // sifreyi sifrelenmis data'ya ceviriyoruz

            return Convert.ToHexString(hashBytes);
        }
    }
}
