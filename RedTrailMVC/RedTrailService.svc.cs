using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Security.Cryptography;
using System.ServiceModel.Activation;
using System.Text;

namespace RedTrailMVC
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IRedTrailService
    {
        [WebInvoke(Method = "POST", UriTemplate = "Authenticate", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        bool Authenticate(string username, string passkey, string deviceid);

        [WebInvoke(Method = "POST", UriTemplate = "Register", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        bool Register(UserForm user);

        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "Hash/{input}")]
        [OperationContract]
        string GetHash(string input);

        [WebInvoke(Method = "POST", UriTemplate = "GetOne", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        UserForm GetOne();

        [WebInvoke(Method = "POST", UriTemplate = "Select", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat =WebMessageFormat.Json)]
        UserForm[] Select();

        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "One/{username}")]
        [OperationContract]
        UserForm SelectOne(string username);
    }
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class RedTrailService : IRedTrailService
    {
        public bool Authenticate(string username, string passkey, string deviceid)
        {
            RedTrailEntities db = new RedTrailEntities();
            passkey = Utility.GetMd5Hash(passkey);
            return db.RedUsers.Where(p => p.Username == username && p.PasswordHash == passkey).Any();
        }

        public UserForm[] Select()
        {
            //int pageSize = Convert.ToInt16(ConfigurationManager.AppSettings["paging"]);
            RedTrailEntities db = new RedTrailEntities();
            return db.RedUsers.Select(p => new UserForm()
            {
                Address1 = p.Address1,
                Address2 = p.Address2,
                Age = p.Age,
                BloodGroup = p.BloodGroup,
                ContactNo = p.ContactNo,
                CreatedAt = p.CreatedAt,
                Email = p.Email,
                Gender = p.Gender,
                IsDonor = p.IsDonor,
                Zone = p.RedLocation.ZoneName,
                ZoneId = p.RedLocation.Id,
                Username = p.Username,
                Longtitude = p.Longtitude,
                Latitude = p.Latitude,
                Id = p.Id
            }).ToArray();
        }

        public UserForm SelectOne(string username)
        {
            RedTrailEntities db = new RedTrailEntities();
            return db.RedUsers.Where(p => p.Username == username).Select(p => new UserForm(){
                Address1 = p.Address1,
                Address2 = p.Address2,
                Age = p.Age,
                BloodGroup = p.BloodGroup,
                ContactNo = p.ContactNo,
                CreatedAt = p.CreatedAt,
                Email = p.Email,
                Gender = p.Gender,
                IsDonor = p.IsDonor,
                Zone = p.RedLocation.ZoneName,
                ZoneId = p.RedLocation.Id,
                Username = p.Username,
                Longtitude = p.Longtitude,
                Latitude = p.Latitude
            }).SingleOrDefault();
        }
        public UserForm[] SelectByZone(int Zoneid)
        {
            RedTrailEntities db = new RedTrailEntities();
            return db.RedUsers.Where(p => p.RedLocation.Id == Zoneid).Select(p => new UserForm()
            {
                Address1 = p.Address1,
                Address2 = p.Address2,
                Age = p.Age,
                BloodGroup = p.BloodGroup,
                ContactNo = p.ContactNo,
                CreatedAt = p.CreatedAt,
                Email = p.Email,
                Gender = p.Gender,
                IsDonor = p.IsDonor,
                Zone = p.RedLocation.ZoneName,
                ZoneId = p.RedLocation.Id,
                Username = p.Username,
                Longtitude = p.Longtitude,
                Latitude = p.Latitude
            }).ToArray();
        }
        public bool Register(UserForm user)
        {
            try
            {
                RedTrailEntities db = new RedTrailEntities();
                db.RedUsers.Add(new RedUser {
                    Address1 = user.Address1,
                    Address2 = user.Address2,
                    Age = user.Age,
                    BloodGroup = user.BloodGroup,
                    ContactNo = user.ContactNo,
                    CreatedAt = user.CreatedAt,
                    Email = user.Email,
                    Gender = user.Gender,
                    IsDonor = user.IsDonor,
                    PasswordHash = Utility.GetMd5Hash(user.Password),
                    Latitude = user.Latitude,
                    Longtitude = user.Longtitude,
                    Username = user.Username,
                    Location = Utility.GetLocation(user.Latitude, user.Longtitude)
                });
                db.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public UserForm GetOne()
        {
            RedTrailEntities db = new RedTrailEntities();
            RedUser user= db.RedUsers.FirstOrDefault();
            return new UserForm()
            {
                Address1 = user.Address1,
                Username = user.Username,
                Password = user.PasswordHash,
                Address2 = user.Address2,
                Age = user.Age,
                BloodGroup = user.BloodGroup,
                ContactNo = user.ContactNo,
                CreatedAt = user.CreatedAt,
                Email = user.Email,
                Gender = user.Gender,
                IsDonor = user.IsDonor,
                Latitude = user.Latitude,
                Longtitude = user.Longtitude
            };
        }
        public string GetHash(string input)
        {
            return Utility.GetMd5Hash(input);
        }
    }
    internal enum Gender
    {
        Male,
        Female
    }
    [DataContract]
    public class UserForm
    {
        [DataMember]
	    public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public short Gender { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public string ContactNo { get; set; }
        [DataMember]
        public int BloodGroup { get; set; }
        [DataMember]
        public bool IsDonor { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longtitude { get; set; }
        [DataMember]
        public string Zone { get; set; }
        [DataMember]
        public int ZoneId { get; set; }
        [DataMember]
        public DateTime CreatedAt { get; set; }
        [DataMember]
        public int Id { get; set; }
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
    internal static class Utility
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
        /// <summary>
        /// This Script uses the Haversine Formula (shown below) expressed in terms of a two-argument inverse tangent function to calculate the great circle distance between two points on the Earth. 
        /// This is the method recommended for calculating short distances by Bob Chamberlain (rgc@jpl.nasa.gov) of Caltech and NASA's Jet Propulsion Laboratory as described on the U.S. Census Bureau Web site.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longtitude"></param>
        /// <returns></returns>

        public static int GetLocation(string latitude, string longtitude)
        {
            RedTrailEntities db = new RedTrailEntities();
            RedLocation[] redlocations = db.RedLocations.ToArray();
            double dlat = Convert.ToDouble(latitude);
            double dlon = Convert.ToDouble(longtitude);
            int nearestId = 0;
            double nearestdistance = -1;
            foreach (var zone in redlocations)
            {
                double d = Calculate(dlat, dlon, Convert.ToDouble(zone.LatitudeMedian), Convert.ToDouble(zone.LongtitudeMedian));
                if(nearestdistance > 0 || d < nearestdistance)
                    nearestId = zone.Id;
            }
            return nearestId;
        }
        public static double Calculate(double sLatitude, double sLongitude, double eLatitude,double eLongitude)
        {
            var sLatitudeRadians = sLatitude * (Math.PI / 180.0);
            var sLongitudeRadians = sLongitude * (Math.PI / 180.0);
            var eLatitudeRadians = eLatitude * (Math.PI / 180.0);
            var eLongitudeRadians = eLongitude * (Math.PI / 180.0);

            var dLongitude = eLongitudeRadians - sLongitudeRadians;
            var dLatitude = eLatitudeRadians - sLatitudeRadians;

            var result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                          Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) *
                          Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Using 3956 as the number of miles around the earth
            var result2 = 3956.0 * 2.0 *
                          Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            return result2;
        }
    }
}