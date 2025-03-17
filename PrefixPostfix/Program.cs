using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

List<int> prefix_function(string s, int substringLength)
{
    List<int> resultPos = new List<int>();
    int n = s.Length;
    int[] p = new int[n];
    for (int i = 1; i < n; i++)
    {
        // префикс функция точно не больше этого значения + 1
        int cur = p[i - 1];
        // уменьшаем cur значение, пока новый символ не сматчится
        while (s[i] != s[cur] && cur > 0)
            cur = p[cur - 1];
        // здесь либо s[i] == s[cur], либо cur == 0
        if (s[i] == s[cur])
        {
            p[i] = cur + 1;
            if(p[i] == substringLength)
            {
                resultPos.Add(i - (substringLength - 1) - (substringLength + 1));
            }
        }
    }
    return resultPos;
}

List<int> FindElementsIndex(int[] p, int lengthSubline)
{
    List<int> resultPos = new List<int>();
    int maxValue = lengthSubline;
    for (int i = 1; i < p.Length; i++) 
    { 
        if(p[i] == maxValue)
        {
            resultPos.Add(i - (maxValue - 1) - (maxValue + 1));
        }
    }
    return resultPos;
}

static string GenerateRandomString(int length, string subline = "", int chancePaste = 0, int chanceFakeSubline = 0)
{
    string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    StringBuilder stringBuilder = new StringBuilder(length);
    for (int i = 0; i < length; i++)
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());
        if (random.Next(100) <= chancePaste && i + subline.Length < length)
        {
            random = new Random(Guid.NewGuid().GetHashCode());
            if (random.Next(100) > chanceFakeSubline)
            {
                stringBuilder.Append(subline);
                i += subline.Length - 1;
            }
            else
            {
                string fakeLine = subline.Substring(0, random.Next(subline.Length - 1));
                stringBuilder.Append(fakeLine);
                i += fakeLine.Length - 1;
            }
        }
        else
        {
            random = new Random(Guid.NewGuid().GetHashCode());
            int index = random.Next(chars.Length);
            stringBuilder.Append(chars[index]);
        }
    }
    return stringBuilder.ToString();
}

List<int> naive_search(string text, string substring)
{
    int textLength = text.Length;
    int substringLength = substring.Length;
    List<int> indexes = new List<int>();
    for (int i = 0; i < textLength - substringLength + 1; i++)
    {
        for (int j = 0; j < substringLength; j++)
        {
            if (text[i + j] != substring[j])
            {
                break;
            }
            if (j == substringLength - 1)
            {
                indexes.Add(i);
            }
        }
    }
    return indexes;
}

//Console.WriteLine("--- Введите подстроку ---");
//string searchingLine = Console.ReadLine();

//Console.WriteLine("--- Введите строку ---");
//string line = Console.ReadLine();

string splitElement = "$";
string fullLine = "";
string fullSearchingLine = "";

Stopwatch st = new Stopwatch();
Stopwatch st1 = new Stopwatch();
List<int> result = new List<int>();

for (int i = 0; i < 10; i++)
{

    //fullLine += line;
    //fullSearchingLine += searchingLine;

    fullSearchingLine = GenerateRandomString(10);
    fullLine = GenerateRandomString((i + 1) * 100 - 10, fullSearchingLine,50,50);

    string data = fullSearchingLine + splitElement + fullLine;
    int len = fullSearchingLine.Length;

    //При первом запуске метода prefix_function или любого другого кода,
    //среда выполнения .NET может выполнять Just-In-Time (JIT) компиляцию,
    //что занимает дополнительное время. Последующие вызовы уже скомпилированного
    //кода выполняются быстрее.

    // Just-In-Time
    naive_search(fullLine, fullSearchingLine);
    //-------------------------------------------------------------------
    st1.Start();
    result = naive_search(fullLine, fullSearchingLine);
    st1.Stop();
    //------------------------------------------------------------------- 

    //Just-In-Time
    prefix_function(data, len);
    //-------------------------------------------------------------------
    st.Start();
    result = prefix_function(data, len);
    st.Stop();
    //------------------------------------------------------------------- 

    Console.WriteLine($"--------- Результаты {i + 1} -----------");
    //Console.WriteLine("Индексы подстрок: ");

    //for (int j = 0; j < result.Count; j++)
    //{
    //    Console.Write($"Индекс: {result[i]}");
    //    Console.Write($" Слово: {fullLine.Substring(result[i], fullSearchingLine.Length)}");
    //    Console.WriteLine();
    //}
    //Console.WriteLine();
    Console.WriteLine($"Количество подстрок: {result.Count}");
    Console.WriteLine($"n + m = {fullLine.Length + fullSearchingLine.Length}");
    Console.WriteLine($"Наивный алгоритм выполнил поиск за {st1.Elapsed.TotalMilliseconds} миллисекунд");
    Console.WriteLine($"Префиксный алгоритм выполнил поиск за {st.Elapsed.TotalMilliseconds} миллисекунд");
    st.Reset();
    st1.Reset();
    result.Clear();
}