using System;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

public class Program
{
    public static string ReverseAndNot(int i)
    {
        return new string(i.ToString().Reverse().ToArray()) + i.ToString();
    }

    public static string OverTime(double[] arr)
    {
        double hoursOverTime = arr[1] > 17 ? arr[1] - (arr[0] > 17 ? arr[0] : 17) : 0;
        double hours = arr[1] - arr[0] - hoursOverTime;
        hours = hours > 0 ? hours : 0;
        double result = hours * arr[2] + hoursOverTime * (arr[2] * arr[3]);

        return $"${result:0.00}";
    }

    public static string HasSpecialCharactar(string str)
    {
        return str.Any(c => !char.IsLetterOrDigit(c)) ? "true" : "false";
    }

    public static bool ValidatePassword(string password)
    {
        return password.Length >= 6 &&
               password.Length <= 24 &&
               password.Any(char.IsDigit) &&
               password.Any(char.IsUpper) &&
               Regex.IsMatch(new string(password.Where(x => !"!@#$%^&*()+=_-{}[]:;\"'?<>,.".Contains(x)).ToArray()), "^[a-zA-Z0-9]*$") &&
               !password.GroupBy(c => c).Any(g => g.Count() > 2) &&
               password.Any(c => "!@#$%^&*()+=_-{}[]:;\"'?<>,.".Contains(c));
    }

    string TranslateWord(string word)
    {
        if (word.Length == 0)
            return word;

        List<string> punctuation = new List<string> { "", "" };
        while (word.Length != 0 && char.IsPunctuation(word[0]))
        {
            punctuation[0] += word[0];
            word = word.Substring(1);
        }
        while (word.Length != 0 && char.IsPunctuation(word.Last()))
        {
            punctuation[1] += word.Last();
            word = word.Remove(word.Length - 1);
        }
        char[] array = punctuation[1].ToCharArray();
        Array.Reverse(array);
        punctuation[1] = new String(array);
        bool iscapital = char.IsUpper(word[0]);
        if ("aeiou".Contains(word.ToLower()[0]))
        {
            return punctuation[0] + word + "yay" + punctuation[1];
        }
        string consonants = "";
        while (word.Length > 0 && !"aeiou".Contains(word.ToLower()[0]))
        {
            consonants += word[0];
            word = word.Substring(1);
        }
        return iscapital ? punctuation[0] + word[0].ToString().ToUpper() + word.Substring(1).ToLower() + consonants.ToLower() + "ay" + punctuation[1]
            : punctuation[0] + word + consonants + "ay" + punctuation[1];
    }

    string TranslateSentence(string sentence)
    {
        var words = sentence.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            words[i] = TranslateWord(words[i]);
        }
        
        return string.Join(" ", words);
    }

    string MaxOccur(string str)
    {
        var dict = new Dictionary<char, int>();
        foreach (var c in str)
        {
            if (dict.ContainsKey(c))
                dict[c]++;
            else
                dict.Add(c, 1);
        }
        var max = dict.Max(x => x.Value);
        if (max == 1)
        {
            return "No Repetition";
        }
        if (dict.Count(x => x.Value == max) > 1)
        {
            char[] chars = dict.Where(x => x.Value == max).Select(x => x.Key).ToArray();
            Array.Reverse(chars);
            return string.Join(", ", chars).Trim();
        }
        return new string(dict.Where(x => x.Value == max).Select(x => x.Key).ToArray());
    }

    int findCommonDivide(int a, int b)
    {
        if (a == 0)
            return b;
        return findCommonDivide(b % a, a);
    }

    string SimplifyFractions(string str)
    {
        int getDivisor(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        var numbers = str.Split('/');
        int a = int.Parse(numbers[0]);
        int b = int.Parse(numbers[1]);
        int GCD = getDivisor(a, b);
        if (a % b == 0)
            return $"{a / b}";
        if (GCD != 1)
        {
            return $"{a / GCD}/{b / GCD}";
        }
        return str;
    }

    int License(string me, int agents, string others)
    {
        List<string> queue = others.Split(' ').ToList();
        queue.Add(me);
        queue.Sort();
        int index = queue.IndexOf(me);
        int count = 0;
        while (index > 0)
        {
            index -= agents;
            count++;
        }
        return count * 20;
    }

    string Uncensor(string txt, string vowels)
    {
        List<char> letters = vowels.ToCharArray().ToList();
        while (txt.Contains("*"))
        {
            txt = txt.Remove(txt.IndexOf('*')) + letters[0] + txt.Substring(txt.IndexOf('*') + 1);
            letters.RemoveAt(0);
        }

        return txt;
    }

    bool IsValidHexCode(string str)
    {
        return Regex.IsMatch(str, "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
    }

    int GetMaxRepeats(string str)
    {
        int max = 0;
        int count = 0;
        char last = ' ';
        foreach (var c in str)
        {
            if (c == last)
            {
                count++;
            }
            else
            {
                count = 1;
                last = c;
            }
            max = Math.Max(max, count);
        }
        return max;
    }


    class LetterSlot
    {
        public bool IsUpper { get; set; }
        public bool IsWhiteSpace { get; set; }
        public char Letter { get; set; }

        public LetterSlot(char c)
        {
            IsWhiteSpace = char.IsWhiteSpace(c);
            IsUpper = char.IsUpper(c);
            Letter = char.ToLower(c);
        }

        public char GetChar()
        {
            if (IsWhiteSpace)
                return ' ';
            return IsUpper ? char.ToUpper(Letter) : char.ToLower(Letter);
        }
    }

    public static void TestCase(dynamic value, dynamic expected)
    {
        string TrueAlphabetic(string txt)
        {
            List<LetterSlot> slots = txt.Select(i => new LetterSlot(i)).ToList();
            List<char> letters = txt.Replace(" ", "").ToCharArray().ToList();
            letters.Reverse();
            foreach (var i in slots.Where(x => x.Letter != ' '))
            {
                i.Letter = letters[0];
                letters.RemoveAt(0);
            }
            return string.Join("", slots.Select(x => x.GetChar()));


        }

        dynamic result = (TrueAlphabetic(value));
        if (result == expected)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Test passed on {value}\n Expected: {expected}\n Got: {result}\n");
            return;
        }
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Test failed on {value}\n Expected: {expected}\n Got: {result}\n");
        Console.ForegroundColor = ConsoleColor.White;
    }


    public static void Main(string[] args)
    {
        TestCase("Edabit", "Tibade");
        TestCase("UPPER lower", "REWOL reppu");
        TestCase("1 23 456", "6 54 321");
        TestCase("Hello World!", "!dlro Wolleh");
        TestCase("Where's your dog Daisy?", "?ysiadg odru oys 'erehw");
        TestCase("addition(3, 2) = 5", "5=)2,3(noit id d a");
        TestCase("It's known that CSS means Cascading Style Sheets",
            "Stee hsely tsgn IDA csacs Naemsscta Htnwo Nks'ti");

        string TrueAlphabetic(string str)
        {
            List<char> alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
            List<string> words = str.Split(' ').ToList();
            List<string> wordsSorted = words.Select(i => new string(i.OrderBy(x => alpha.IndexOf(x)).ToArray())).ToList();

            return wordsSorted.Aggregate((x, y) => x + " " + y);
        }
        Console.WriteLine(TrueAlphabetic("BEB RA"));
    }
}

