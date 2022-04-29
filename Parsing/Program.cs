using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.Globalization;

namespace Parsing
{
    class Program
    {
        static string path = "file_new.txt"; // текстовый файл с перепиской (легко качается в kate mobile)
        static ArrayList list = new ArrayList();
        static ArrayList diffsA = new ArrayList();
        static ArrayList diffsK = new ArrayList();
        static ArrayList diffsAnew = new ArrayList();
        static ArrayList diffsKnew = new ArrayList();
        static string? time;
        static string? last_name;
        static string? last_time;
        static string? name;
        async static Task Main(string[] args)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    list.Add(line);
                }

            }
            last_name = "Анастасия"; //имя вашего собеседника
            last_time = "";
            foreach (string? line in list)
            {
                bool find_space = false;
                bool find_time = false;
                time = "";
                name = "";
                for(int i = 0; i < line.Length; i++)
                {
                    char letter = line[i];
                    if (i == 0)
                    {
                        if (letter == 'А') name = "Анастасия"; // первая буква имени ваше собеседника
                        else if (letter == 'К') name = "Кирилл"; //первая буква вашего имени
                        else name = "";
                    }
                    if (letter == 'г')
                        find_space = true;
                    if ((letter == '.') && find_space)
                        find_time = true;
                    if (letter == ')')
                        find_time = false;
                    if ((((letter >= '0') && (letter <= '9')) || (letter == ':')) && find_time)
                        time += letter;
                }
                if ((name != "") && (last_name != "") && (time != "") && (last_name != name))
                {
                    DateTime last_time_date = new DateTime();
                    DateTime time_date = new DateTime();
                    if ((last_time != "") && (time != ":") && (last_time != ":"))
                    {
                        last_time_date = DateTime.ParseExact(last_time, "HH:mm:ss", null);
                        time_date = DateTime.ParseExact(time, "HH:mm:ss", null);
                        TimeSpan diff = time_date.Subtract(last_time_date);
                        if (diff < new TimeSpan(0,0,0))
                        {
                            diff = -diff;
                            diff = (new TimeSpan(24, 0, 0)).Subtract(diff);
                        }
                        int seconds = diff.Hours * 3600 + diff.Minutes * 60 + diff.Seconds;
                            Console.WriteLine(seconds);
                        if (seconds < 4000)
                        {
                            if (name == "Кирилл") // ваше имя в переписке
                                diffsK.Add(seconds.ToString());
                            else diffsA.Add(seconds.ToString());
                        }
                    }
                    last_name = name;
                    last_time = time;
                }
            }
            string pathAnew = "A1.txt"; // файл с интервалами ответов на сообщения вашего собеседника вам
            string pathKnew = "K1.txt"; // файл с интервалами ответов на сообщения вами вашему собеседнику
            using (StreamWriter writer = new StreamWriter(pathAnew, false))
            {
                foreach(string diff in diffsA)
                    await writer.WriteLineAsync(diff);
            }
            using (StreamWriter writer = new StreamWriter(pathKnew, false))
            {
                foreach (string diff in diffsK)
                    await writer.WriteLineAsync(diff);
            }
        }
    }
}
