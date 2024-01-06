using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;


namespace doan.Utilities
{
    public class Functions
    {
        public static int _AccountID = 0;
        public static string _UserName = string.Empty;
        public static string _FullName = string.Empty;
        public static string _Email = string.Empty;
        public static string _Image = string.Empty;
        public static string _Phone = string.Empty;
        public static int _RoleID = 0;
        public static string _Message = string.Empty;
        public static string _MessageEmail = string.Empty;
        public static string TitleSlugGeneration(string type, string title, int id)
        {
            string url = string.Empty;
            if (type == string.Empty)
                url = "/" + SlugGenerator.SlugGenerator.GenerateSlug(title) + "-" + id.ToString() + ".html";
            else
                url = "/" + type + "/" + SlugGenerator.SlugGenerator.GenerateSlug(title) + "-" + id.ToString() + ".html";
            return url;
        }
        public static string getCurrenDate()
        {
            return DateTime.Now.ToString();
        }
        public static string ToVnd(double donGia)
        {
            return donGia.ToString("#,##0") + " đ";
        }

        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }

        public static bool IsLogin()
        {
            if (string.IsNullOrEmpty(Functions._UserName) || Functions._AccountID <= 0)
            {
                return false;
            }
            return true;
        }
    }   
}
