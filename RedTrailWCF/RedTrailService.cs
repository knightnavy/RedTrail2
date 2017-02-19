using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Security.Cryptography;
using System.Text;

namespace RedTrailWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class RedTrailService : IRedTrailService
    {
        public bool Authenticate(string username, string passkey, string deviceid)
        {
            RedTrailEntities db = new RedTrailEntities();
            passkey = Crypto.GetMd5Hash(passkey);
            return db.Users.Where(p => p.Username == username && p.PasswordHash == passkey).Any();
        }

        public bool Register(User user)
        {
            return true;
        }
        public string GetHash(string input)
        {
            return Crypto.GetMd5Hash(input);
        }
    }
    [DataContract]
    public class Login
    {
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public string deviceid { get; set; }
    }
    internal static class Crypto
    {
        public static string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }
    }
}
