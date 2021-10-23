using System;
using System.Web;
using System.IO;
using IntelliOverflowAPI;
using System.Threading.Tasks;

namespace APITest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*Console.WriteLine("Enter a string having '&', '<', '>' or '\"' in it: ");
            string myString = Console.ReadLine();

            // Encode the string.
            string myEncodedString = HttpUtility.HtmlEncode(myString);

            Console.WriteLine($"HTML Encoded string is: {myEncodedString}");
            StringWriter myWriter = new StringWriter();

            // Decode the encoded string.
            HttpUtility.HtmlDecode(myEncodedString, myWriter);

            string myDecodedString = myWriter.ToString();
            Console.Write($"Decoded string of the above encoded string is: {myDecodedString}");*/

            StackExchangeRequest test = await API.DoSearchAsync("Visual studio linker error");
            Console.WriteLine("Success");
        }
    }
}
