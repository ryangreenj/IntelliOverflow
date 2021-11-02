using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOVSPlugin
{
    static class Util
    {
        public static string PrepareErrorQuery(string errorText, string errorCode = "", string errorProject = "", string errorFile = "")
        {
            string query = "";

            // Remove any instances of errorProject or errorFile from the query
            string[] projectFileSeparators = new string[] { errorProject, errorFile };
            string[] splitErrorText = errorText.Split(projectFileSeparators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in splitErrorText)
            {
                query += s;
            }

            // Remove any text that is in-between single quotes as that usually contains context-sensitive information
            int firstIndex = 0;
            while ((firstIndex = query.IndexOf('\'')) != -1)
            {
                if (firstIndex < query.Length - 1)
                {
                    int secondIndex = query.Substring(firstIndex + 1).IndexOf('\'');
                    if (secondIndex != -1)
                    {
                        secondIndex += firstIndex + 1; // Shift back to original string
                        query = query.Remove(firstIndex, secondIndex - firstIndex + 1);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            query = query.Trim(); // Remove any leading and trailing whitespace

            // Only ever want 1 space in a row, all other whitespace should also become a single space

            char[] whitespaceSeparators = new char[] { ' ', '\t', '\n', '\r' };
            string[] splitWhitespace = query.Split(whitespaceSeparators, StringSplitOptions.RemoveEmptyEntries);
            query = "";

            foreach (string s in splitWhitespace)
            {
                query += s + ' ';
            }

            query = query.Trim();

            if (!string.IsNullOrEmpty(errorCode))
            {
                query = errorCode + " " + query;
            }
            
            return query;
        }
    }
}
