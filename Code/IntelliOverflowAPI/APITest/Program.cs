using System;
using System.Web;
using System.IO;

namespace APITest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a string having '&', '<', '>' or '\"' in it: ");
            string myString = Console.ReadLine();

            // Encode the string.
            string myEncodedString = HttpUtility.HtmlEncode(myString);

            Console.WriteLine($"HTML Encoded string is: {myEncodedString}");
            StringWriter myWriter = new StringWriter();

            // Decode the encoded string.
            HttpUtility.HtmlDecode(myEncodedString, myWriter);

            string myDecodedString = myWriter.ToString();
            Console.Write($"Decoded string of the above encoded string is: {myDecodedString}");
        }
    }
}
