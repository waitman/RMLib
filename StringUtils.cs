/*
  RMLib: Nonvisual support classes used by multiple R&M Software programs
  Copyright (C) 2008-2014  Rick Parrish, R&M Software

  This file is part of RMLib.

  RMLib is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  any later version.

  RMLib is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with RMLib.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Net;

namespace RandM.RMLib
{
    static public class StringUtils
    {
        static private Random _R = new Random();

        static public string BytesToStr(long bytes)
        {
            const double KILOBYTE = 1024.0;
            const double MEGABYTE = 1048576.0;
            const double GIGABYTE = 1073741824.0;
            const double TERABYTE = 1099511627776.0;
            const double PETABYTE = 1125899906842624.0;
            const double EXABYTE = 1152921504606846976.0;
            const double ZETTABYTE = 1180591620717411303424.0;
            const double YOTTABYTE = 11529215046120892581961462917470617606846976.0;

            double Num = bytes / YOTTABYTE;
            if (Num >= 1) return bytes.ToString("N0") + " B (" + Num.ToString("F2") + " YB)";

            Num = bytes / ZETTABYTE;
            if (Num >= 1) return bytes.ToString("N0") + " B (" + Num.ToString("F2") + " ZB)";

            Num = bytes / EXABYTE;
            if (Num >= 1) return bytes.ToString("N0") + " B (" + Num.ToString("F2") + " EB)";

            Num = bytes / PETABYTE;
            if (Num >= 1) return bytes.ToString("N0") + " B (" + Num.ToString("F2") + " PB)";

            Num = bytes / TERABYTE;
            if (Num >= 1) return bytes.ToString("N0") + " B (" + Num.ToString("F2") + " TB)";

            Num = bytes / GIGABYTE;
            if (Num >= 1) return bytes.ToString("N0") + " B (" + Num.ToString("F2") + " GB)";

            Num = bytes / MEGABYTE;
            if (Num >= 1) return bytes.ToString("N0") + " B (" + Num.ToString("F2") + " MB)";

            Num = bytes / KILOBYTE;
            if (Num >= 1) return bytes.ToString("N0") + " B (" + Num.ToString("F2") + " KB)";

            return bytes.ToString("N0") + " B";
        }

        /// <summary>
        /// Changes the CRLF's in a given string to be <br />
        /// </summary>
        /// <param name="text">The text to perform the conversion on</param>
        /// <returns>The text with CRLF's replaced with <br /></returns>
        static public string CRLFtoBR(string text)
        {
            return text.Replace("\r\n", "\r").Replace("\n", "\r").Replace("\r", "\r\n").Replace("\r\n", "<br />");
        }

        static public string ExtractShortPathName(string longPathName)
        {
            if (OSUtils.IsWindows)
            {
                StringBuilder ShortPath = new StringBuilder(255);
                NativeMethods.GetShortPathName(longPathName, ShortPath, ShortPath.Capacity);
                return ShortPath.ToString();
            }
            else
            {
                return longPathName;
            }
        }

        static public bool IsValidEmailAddress(string emailAddress)
        {
            try
            {
                new MailAddress(emailAddress);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        static public bool IsValidFileName(string fileName)
        {
            if (fileName.Trim().Length == 0) return false;

            char[] InvalidChars = Path.GetInvalidFileNameChars();
            foreach (char C in fileName)
            {
                for (int i = 0; i < InvalidChars.Length; i++)
                {
                    if (C == InvalidChars[i]) return false;
                }
            }

            return true;
        }

        static public bool IsValidIPAddress(string ipAddress)
        {
            try
            {
                if (ipAddress.Trim() != IPAddress.Parse(ipAddress.Trim()).ToString()) return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsValidPort(string port)
        {
            int Port = 0;
            if (int.TryParse(port, out Port))
            {
                return ((Port > 0) && (Port <= 65535));
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Changes the LF's in a given string to be CR+LF (useful for unix to windows conversions)
        /// </summary>
        /// <param name="text">The text to perform the conversion on</param>
        /// <returns>The text with LF's replaced with CR+LF</returns>
        static public string LFtoCRLF(string text)
        {
            return text.Replace("\r\n", "\r").Replace("\n", "\r").Replace("\r", "\r\n");
        }

        static public string MD5(string text)
        {
            return MD5("", text, "");
        }

        static public string MD5String(string preSalt, string text)
        {
            return MD5(preSalt, text, "");
        }

        static public string MD5(string preSalt, string text, string postSalt)
        {
            return BitConverter.ToString(MD5Bytes(preSalt, text, postSalt)).Replace("-", "");
        }

        static public byte[] MD5Bytes(string text)
        {
            return MD5Bytes("", text, "");
        }

        static public byte[] MD5Bytes(string preSalt, string text)
        {
            return MD5Bytes(preSalt, text, "");
        }

        static public byte[] MD5Bytes(string preSalt, string text, string postSalt)
        {
            MD5CryptoServiceProvider MCSP = new MD5CryptoServiceProvider();
            byte[] Input = RMEncoding.Ansi.GetBytes(preSalt + text + postSalt);
            return MCSP.ComputeHash(Input);
        }

        static public double MoneyToDouble(string text)
        {
            double Temp;
            if (double.TryParse(text.Replace("$", "").Replace("(", "-").Replace(")", ""), out Temp))
            {
                return Temp;
            }
            else
            {
                throw new ArgumentException("\"" + text + "\" is not a valid double value", text);
            }
        }

        static public string PadBoth(string text, char padCharacter, int padToLength)
        {
            if (text.Length == padToLength)
            {
                return text;
            }
            else if (text.Length > padToLength)
            {
                return text.Substring(text.Length - padToLength, padToLength);
            }
            else
            {
                int Left = (padToLength - text.Length) / 2;
                int Right = padToLength - text.Length - Left;
                return new string(padCharacter, Left) + text + new string(padCharacter, Right);
            }
        }

        static public string PadLeft(string text, char padCharacter, int padToLength)
        {
            if (text.Length == padToLength)
            {
                return text;
            }
            else if (text.Length > padToLength)
            {
                return text.Substring(text.Length - padToLength, padToLength);
            }
            else
            {
                return text.PadLeft(padToLength, padCharacter);
            }
        }

        static public string PadRight(string text, char padCharacter, int padToLength)
        {
            if (text.Length == padToLength)
            {
                return text;
            }
            else if (text.Length > padToLength)
            {
                return text.Substring(0, padToLength);
            }
            else
            {
                return text.PadRight(padToLength, padCharacter);
            }
        }

        static public string PathCombine(string path1, string path2)
        {
            // Check for blank parameters
            if (path1.Length == 0) return path2.TrimStart(Path.DirectorySeparatorChar);
            if (path2.Length == 0) return path1.TrimEnd(Path.DirectorySeparatorChar);
            
            // Trim trailing and leading directory separator characters
            path1 = path1.TrimEnd(Path.DirectorySeparatorChar);
            path2 = path2.TrimStart(Path.DirectorySeparatorChar);

            // Combine the paths
            return path1 + Path.DirectorySeparatorChar + path2;
        }

        static public string PathCombine(string path1, string path2, string path3)
        {
            return PathCombine(PathCombine(path1, path2), path3);
        }

        static public string PathCombine(string path1, string path2, string path3, string path4)
        {
            return PathCombine(PathCombine(PathCombine(path1, path2), path3), path4);
        }

        static public double PercentToDouble(string text)
        {
            double Temp;
            if (double.TryParse(text.Replace("%", ""), out Temp))
            {
                return Temp;
            }
            else
            {
                throw new ArgumentException("\"" + text + "\" is not a valid double value", text);
            }
        }

        static public string RandomString(int length)
        {
            return RandomString(length, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        }

        static public string RandomString(int length, string characters)
        {
            string Result = "";
            for (int i = 0; i < length; i++) {
                Result += characters.Substring(_R.Next(0, characters.Length), 1);
            }
            return Result;
        }

        static public string SecToHMS(int seconds)
        {
            TimeSpan TS = TimeSpan.FromSeconds(seconds);
            return PadLeft(TS.Hours.ToString(), '0', 2) + ":" + PadLeft(TS.Minutes.ToString(), '0', 2) + ":" + PadLeft(TS.Seconds.ToString(), '0', 2);
        }

        static public string SecToMS(int seconds)
        {
            TimeSpan TS = TimeSpan.FromSeconds(seconds);
            return PadLeft(TS.Minutes.ToString(), '0', 2) + ":" + PadLeft(TS.Seconds.ToString(), '0', 2);
        }

        static public int StrToIntDef(string text, int defaultValue)
        {
            int Result = 0;
            return (int.TryParse(text, out Result)) ? Result : defaultValue;
        }
    }
}
