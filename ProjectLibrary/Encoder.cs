/*
 * File: Encoder.cs
 * Descrption: Contains a static method to Encode accented strings to ascii strings
 * Date: 2020-02-03
 */
namespace ProjectLibrary
{
    public class Encoder
    {
        /// <summary>
        /// Replace accented string with ascii string
        /// </summary>
        /// <param name="accentedString"></param>
        /// /// <returns>string with ascii characters</returns>
        static public string AccentStringToASCII(string accentedString)
        {
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedString);
            string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);
            return asciiStr;
        }
    }
}
