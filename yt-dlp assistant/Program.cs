using System;
using System.Diagnostics;
using System.IO;

namespace yt_dlp_assistant
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.WriteLine("yt-dlp.exeとffmpeg.exeを準備して同じフォルダに配置してください。");
            Console.WriteLine("yt-dlp.exe:https://github.com/yt-dlp/yt-dlp/releases/latest");
            Console.WriteLine("ffmpeg.exe:https://ffmpeg.org/download.html (わからない場合調べてください)");
            while (true)
                try
                {
                    if (!Directory.Exists("settings"))
                        Directory.CreateDirectory("settings");
                    string ytdlpPath = "";
                    while (true)
                    {
                        string ytdlpPath_ = "";
                        if (File.Exists("settings\\yt-dlp.path.txt"))
                            ytdlpPath_ = File.ReadAllText("settings\\yt-dlp.path.txt");
                        else
                        {
                            Console.WriteLine("yt-dlp.exeのパスを入力してください。");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            ytdlpPath_ = Console.ReadLine().Replace("\"", "");
                            Console.ForegroundColor = defaultColor;
                        }
                        if (File.Exists(ytdlpPath_) && ytdlpPath_.Contains("yt-dlp.exe"))
                        {
                            if (!File.Exists("settings\\yt-dlp.path.txt"))
                            {
                                Console.WriteLine("yt-dlp.exeが見つかりました。");
                                File.WriteAllText("settings\\yt-dlp.path.txt", ytdlpPath_);
                            }
                            ytdlpPath = ytdlpPath_;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"yt-dlp.exeが見つかりませんでした。({ytdlpPath_})");
                            if (File.Exists("settings\\yt-dlp.path.txt"))
                                File.Delete("settings\\yt-dlp.path.txt");
                        }
                    }

                    int pro = -1;
                    while (pro == -1)
                    {
                        Console.WriteLine("処理を選択してください。　0:直接入力 1:コマンド保存 2:保存済みコマンド表示 3:シンプルダウンロード");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        string _pro = Console.ReadLine();
                        Console.ForegroundColor = defaultColor;
                        if (_pro == "0")
                            pro = 0;
                        else if (_pro == "1")
                            pro = 1;
                        else if (_pro == "2")
                            pro = 2;
                        else if (_pro == "3")
                            pro = 3;
                    }
                    switch (pro)
                    {
                        case 0:
                            Console.WriteLine("コマンドを入力してください。");

                            Process proc0 = new Process();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            string fileWithArgs = Console.ReadLine();
                            proc0.StartInfo.FileName = fileWithArgs.Split(' ')[0];
                            proc0.StartInfo.Arguments = fileWithArgs.Replace(proc0.StartInfo.FileName + " ", "");
                            Console.ForegroundColor = defaultColor;

                            proc0.StartInfo.UseShellExecute = false;
                            proc0.StartInfo.RedirectStandardOutput = true;
                            proc0.Start();
                            Console.ForegroundColor = ConsoleColor.Green;
                            while (!proc0.StandardOutput.EndOfStream)
                                Console.WriteLine(proc0.StandardOutput.ReadLine());
                            proc0.WaitForExit();
                            proc0.Dispose();
                            Console.ForegroundColor = defaultColor;
                            Console.WriteLine("完了しました。");
                            break;
                        case 1:
                            Console.WriteLine("保存するコマンドやメモを一行入力してください。`settings\\commands.txt`に追加されます。");
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            string command = Console.ReadLine();
                            Console.ForegroundColor = defaultColor;
                            if (File.Exists("settings\\commands.txt"))
                                command = (File.ReadAllText("settings\\commands.txt") + "\n" + command).Replace("\n\n", "\n");
                            File.WriteAllText("settings\\commands.txt", command);
                            break;
                        case 2:
                            if (File.Exists("settings\\commands.txt"))
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine(File.ReadAllText("settings\\commands.txt"));
                                Console.ForegroundColor = defaultColor;
                            }
                            else
                                Console.WriteLine("`settings\\commands.txt`が見つかりませんでした。先に保存してください。");
                            break;
                        case 3:
                            Console.WriteLine("`{yt-dlp.exeのパス} -o \"{入力されたフォルダのパス}\\%(title)s.mp4\" {入力されたURL} -f mp4`を実行します。スキップする場合URLを空白にしてください。");
                            Console.WriteLine("保存するフォルダのパスを入力してください。(空白だとyt-dlpのフォルダ、`/c/`でこのyt-dlp assistant.exeがあるフォルダ、`/c/\\output`のように)");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            string path = Console.ReadLine().Replace("\"", "").Replace("/c/", Path.GetFullPath("yt-dlp assistant.exe").Replace("yt-dlp assistant.exe", ""));
                            Console.ForegroundColor = defaultColor;
                            Console.WriteLine("URLを入力してください。");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            string url = Console.ReadLine();
                            Console.ForegroundColor = defaultColor;
                            if (url != "")
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine($"{ytdlpPath} -o \"{path}\\%(title)s.mp4\" {url} -f mp4");
                                Process proc3 = new Process();
                                proc3.StartInfo.FileName = ytdlpPath;
                                proc3.StartInfo.Arguments = $"-o \"{path}\\%(title)s.mp4\" {url} -f mp4";
                                proc3.StartInfo.UseShellExecute = false;
                                proc3.StartInfo.RedirectStandardOutput = true;
                                proc3.Start();

                                Console.ForegroundColor = ConsoleColor.Green;
                                while (!proc3.StandardOutput.EndOfStream)
                                    Console.WriteLine(proc3.StandardOutput.ReadLine());
                                proc3.WaitForExit();
                                proc3.Dispose();
                                Console.ForegroundColor = defaultColor;
                                Console.WriteLine("完了しました。");
                            }
                            break;

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
        }
    }
}
