using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantGuideToGalaxy
{
    /*
glob is I
prok is V
pish is X
tegj is L
glob glob Silver is 34 Credits
glob prok Gold is 57800 Credits
pish pish Iron is 3910 Credits
how much is pish tegj glob glob ?
how many Credits is glob prok Silver ?
how many Credits is glob prok Gold ?
how many Credits is glob prok Iron ?
how much wood could a woodchuck chuck if a woodchuck could chuck wood ?
     
pish tegj glob glob is 42
glob prok Silver is 68 Credits
glob prok Gold is 57800 Credits
glob prok Iron is 782 Credits
I have no idea what you are talking about
    */
    class Program
    {
        // Dictionary to hold the respective values for the Arabic Numerals
        static Dictionary<char, int> symbols = new Dictionary<char, int>();
        // Dictionary to hold the newely provided Symbols 'glob',/'prok'
        static Dictionary<string, char> customSymbols = new Dictionary<string, char>();
        // Dictionary to hold multiplier words - GOLD, SILVER...
        static Dictionary<string, int> keyWords = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            // Fill the Arabic dictionary
            symbols.Add('I', 1);
            symbols.Add('V', 5);
            symbols.Add('X', 10);
            symbols.Add('L', 50);
            symbols.Add('C', 100);
            symbols.Add('D', 500);
            symbols.Add('M', 1000);
            while (true)
            {
                processLine(Console.ReadLine());
            }
        }
        public static void processLine(String line)
        {
            String[] splitString = line.Split(' ');

            if (line.EndsWith("?"))
            {
                Console.WriteLine(questionAndReply(line, splitString));
            }
            else if (splitString[2].Length == 1 && symbols.ContainsKey(splitString[2][0]) && splitString[1].ToLower() == "is")
            {
                customSymbols.Add(splitString[0], splitString[2][0]);
            }
            else if (line.ToLower().EndsWith("credits"))
            {
                updateDict(splitString);
            }
        }

        private static string questionAndReply(string line, string[] splitString)
        {
            string result = "I have no idea what you are talking about";
            if (line.StartsWith("how much is"))
            {
                string romanNumeral = string.Empty;
                int sum = getNumber(splitString, 3, splitString.Length - 2, ref romanNumeral);
                result = string.Empty;
                for (int i = 3; i <= splitString.Length - 2; i++)
                {
                    result += splitString[i] + " ";
                }
                result += "is " + sum;
            }
            else if (line.StartsWith("how many Credits is"))
            {
                
                string romanNumeral = string.Empty;
               int sum =  getNumber(splitString, 4, splitString.Length - 3, ref romanNumeral);
                if (keyWords.ContainsKey(splitString[splitString.Length - 2]))
                {
                    sum *= keyWords[splitString[splitString.Length - 2]];
                    result = string.Empty;
                    for (int i = 4; i <= splitString.Length - 2; i++)
                    {
                        result += splitString[i] + " ";
                    }
                    result += "is " + sum + " Credits";
                }
                else
                {
                    return result;
                }
            }
            return result;
        }

        private static void updateDict(String[] splitString)
        {
            int sum = int.Parse(splitString[splitString.Length - 2]);
            string romanNumeral = string.Empty;
            int eNumber = getNumber(splitString, 0, splitString.Length - 1, ref romanNumeral);
            sum = sum / eNumber;
            keyWords.Add(splitString[splitString.Length - 4], sum);
        }

        private static int getNumber(String[] splitString, int startIndex, int endIndex, ref string romanNumeral)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                char temp;
                customSymbols.TryGetValue(splitString[i], out temp);
                romanNumeral += temp;
            }
            int eNumber = convertRomanToArabic(romanNumeral);
            return eNumber;
        }

        static int convertRomanToArabic(string RomanNumerals)
        {
            RomanNumerals = RomanNumerals.ToUpper();
            char[] charThis = RomanNumerals.ToCharArray();
            int[] intThis = new int[charThis.Length];
            bool bValid = false;
            int intCalc = 0;

            //go through char array to change individual roman numerals to numbers
            for (int i = 0; i < charThis.Length; i++)
            {
                //check to ensure char array contains only roman numerals
                foreach (char c in symbols.Keys)
                {
                    if (charThis[i] == c)
                    {
                        bValid = true;
                        break;
                    }
                }
                if (!bValid)
                {
                    return -1;
                }
                symbols.TryGetValue(charThis[i], out intThis[i]);
            }

            //perform subtraction where needed
            intCalc = 0;
            for (int i = 0; i < intThis.Length - 1; i++)
            {
                if (intThis[i] < intThis[i + 1])
                {
                    intCalc = intThis[i + 1] - intThis[i];
                    intThis[i] = intCalc;
                    //two numerals combined into first int, so zero out second int
                    intThis[i + 1] = 0;
                    //avoid doing subtraction twice in a row
                    i++;
                }
            }
            //add together all of the individual values
            intCalc = 0;
            for (int i = 0; i < intThis.Length; i++)
            {
                intCalc = intCalc + intThis[i];
            }
            return intCalc;
        }

    }
}
