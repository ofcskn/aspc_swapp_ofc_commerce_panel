using Entity.Models;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Service.Utilities
{
    public class Helpers
    {
        public static string ExecuteCmd(string arguments)
        {
            // Create the Process Info object with the overloaded constructor
            // This takes in two parameters, the program to start and the
            // command line arguments.
            // The arguments parm is prefixed with "@" to eliminate the need
            // to escape special characters (i.e. backslashes) in the
            // arguments string and has "/C" prior to the command to tell
            // the process to execute the command quickly without feedback.
            ProcessStartInfo _info =
                new ProcessStartInfo("cmd", @"/C " + arguments);

            // The following commands are needed to redirect the
            // standard output.  This means that it will be redirected
            // to the Process.StandardOutput StreamReader.
            _info.RedirectStandardOutput = true;

            // Set UseShellExecute to false.  This tells the process to run
            // as a child of the invoking program, instead of on its own.
            // This allows us to intercept and redirect the standard output.
            _info.UseShellExecute = false;

            // Set CreateNoWindow to true, to supress the creation of
            // a new window
            _info.CreateNoWindow = true;

            // Create a process, assign its ProcessStartInfo and start it
            Process _p = new Process();
            _p.StartInfo = _info;
            _p.Start();

            // Capture the results in a string
            string _processResults = _p.StandardOutput.ReadToEnd();

            // Close the process to release system resources
            _p.Close();

            // Return the output stream to the caller
            return _processResults;
        }

        public static string MonthName(int monthNumber)
        {
            string result = string.Empty;
            switch (monthNumber)
            {
                case 1: result = "Ocak"; break;
                case 2: result = "Şubat"; break;
                case 3: result = "Mart"; break;
                case 4: result = "Nisan"; break;
                case 5: result = "Mayıs"; break;
                case 6: result = "Haziran"; break;
                case 7: result = "Temmuz"; break;
                case 8: result = "Ağustos"; break;
                case 9: result = "Eylül"; break;
                case 10: result = "Ekim"; break;
                case 11: result = "Kasım"; break;
                case 12: result = "Aralık"; break;
            }
            return result;
        }

        public static string ToSlug(string input)
        {
            input = input.Replace("?", "").Replace("!", "").Trim();
            input = Regex.Replace(input, @"[^\w\@-]", "-").ToLower();
            Dictionary<string, string> replacements = new Dictionary<string, string> { { "ğ", "g" }, { "ü", "u" }, { "ş", "s" }, { "ı", "i" }, { "ö", "o" }, { "ç", "c" }, { "--", "" }, { "---", "" }, { "----", "" } };

            foreach (string key in replacements.Keys)
            {
                input = Regex.Replace(input, key, replacements[key]);
            }
            while (input.IndexOf("--") > -1)
            {
                input = input.Replace("--", "-");
            }

            if (input.Length <= 150)
            {
                return input;
            }
            else
            {
                return input.Substring(0, 150);
            }
        }

        public static string ChangeFileName(string fileName)
        {
            return fileName.Replace('/', '_');
        }

        public static string ChangeFilePath(string fileName)
        {
            return fileName.Replace('/', '\\');
        }
        public static DateTime Today => new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        public static DateTime FirstDateOfThisMonth => new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        public static DateTime LastDateOfThisMonth => FirstDateOfThisMonth.AddMonths(1).AddDays(-1);

        public static DateTime LastDateOfNextMont => new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month));

        public static string GenerateSHA256String(string input)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            string hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            return hash;
        }

        public static string GenerateMD5String(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}