using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace AppleStore
{
    public class Func
    {
        public static string IDClientGoogle = "987251945724-c3uqooemo7p1o1tj1q0v1nha5882hhe6.apps.googleusercontent.com";
        public static string IDClientFacebook = "559900125477541";

        public static string RECAPTCHA_SITE_KEY = "6LeShkgfAAAAAD17sscX7kYWeY4yrHcbNR__Smsv";
        public static string RECAPTCHA_SECRET_KEY = "6LeShkgfAAAAAGhsTOj5fVVt_MtHxiX-XmyF--yC";

        public static string vnp_TmnCode = "JDW67A9Y";
        public static string vnp_HashSecret = "LUNSQRAOZYLJODQZQRXELIZHSRHRIBVM";

        public static int DATA_PER_PAGE = 10;

        public string NumberToStr(int number)
        {
            return string.Format("{0:n0}", number);
        }
        public string NumberToStr(long number)
        {
            return string.Format("{0:n0}", number);
        }

        private static Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void InitProperties(object obj)
        {
            foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite))
            {
                var type = prop.PropertyType;
                var constr = type.GetConstructor(Type.EmptyTypes); //find paramless const
                if (type.IsClass && constr != null)
                {
                    var propInst = Activator.CreateInstance(type);
                    prop.SetValue(obj, propInst, null);
                    InitProperties(propInst);
                }
            }
        }

        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public JsonSerializerSettings JsonSetting()
        {
            return new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,

            };
        }
    }
}
