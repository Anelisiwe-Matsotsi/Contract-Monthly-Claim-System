using System.Security.Cryptography;

namespace PROG6212_POE
{/*Troelsen and Japikse, 2021*/
    public class EncryptionHelper
    {
       
        private static readonly byte[] Key = Convert.FromHexString("f0e19761b055d6469e91c8c445387a5a");
        private static readonly byte[] IV = Convert.FromHexString("13E55E6246CDA44E09A1F638FD4FA1AE");

        //Helper method that does the actua; encryption and decryption work
        private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var memoryStream = new MemoryStream()) //temp storage
            {
                //links the memory stream with the encryption decryption process
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }

        //method to encrypt data
        public static byte[] EncryptFile(byte[] data)
        {
            using (Aes aes = Aes.Create())
            {
                //set secret key and IV
                aes.Key = Key;
                aes.IV = IV;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV)) //tool created from AES object
                {
                    return PerformCryptography(data, encryptor); //call helper method to actually encrypt
                }
            }
        }

        // method to decrypt data
        public static byte[] DecryptFile(byte[] data)
        {
            using (Aes aes = Aes.Create())
            {
                //set secret key and IV
                aes.Key = Key; // Key is like a password used to lock and unlock data
                aes.IV = IV; // IV - initialization vector adds randomness so that even if two files have the same, using their encrypted versions will look different

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(data, decryptor);
                }
            }
        }
    }
}
/*Reference List:
Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
 */ 
