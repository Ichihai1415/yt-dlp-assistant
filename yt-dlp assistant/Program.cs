using System;
using System.Diagnostics;
using System.IO;

namespace yt_dlp_assistant
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
                            ytdlpPath_ = Console.ReadLine().Replace("\"", "");
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
                        string _pro = Console.ReadLine();
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
                            Process process = Process.Start(Console.ReadLine());
                            process.WaitForExit();
                            Console.WriteLine("完了しました。");
                            break;
                        case 1:
                            Console.WriteLine("保存するコマンドやメモを一行入力してください。`settings\\commands.txt`に追加されます。");
                            string command = Console.ReadLine();
                            if (File.Exists("settings\\commands.txt"))
                                command = (File.ReadAllText("settings\\commands.txt") + "\n" + command).Replace("\n\n", "\n");
                            File.WriteAllText("settings\\commands.txt", command);
                            break;
                        case 2:
                            if (File.Exists("settings\\commands.txt"))
                            {
                                Console.WriteLine("----------");
                                Console.WriteLine(File.ReadAllText("settings\\commands.txt"));
                                Console.WriteLine("----------");
                            }
                            else
                                Console.WriteLine("`settings\\commands.txt`が見つかりません。先に保存してください。");
                            break;
                        case 3:
                            Console.WriteLine("`{yt-dlp.exeのパス} -o \"{入力されたフォルダのパス}\\%(title)s.mp4\" {入力されたURL} -f mp4`を実行します。スキップする場合URLを空白にしてください。");
                            Console.WriteLine("保存するフォルダのパスを入力してください。(空白だとyt-dlpのフォルダ、`/c/`でこのyt-dlp assistant.exeがあるフォルダ、`/c/\\output`のように)");
                            string path = Console.ReadLine().Replace("\"", "").Replace("/c/", Path.GetFullPath("yt-dlp assistant.exe").Replace("yt-dlp assistant.exe", ""));
                            Console.WriteLine("URLを入力してください。");
                            string url = Console.ReadLine();
                            if (url != "")
                            {
                                Console.WriteLine($"{ytdlpPath} -o \"{path}\\%(title)s.mp4\" {url} -f mp4");
                                Process process2 = new Process();
                                process2.StartInfo.FileName = ytdlpPath;
                                process2.StartInfo.Arguments = $"-o \"{path}\\%(title)s.mp4\" {url} -f mp4";
                                process2.StartInfo.UseShellExecute = false;
                                process2.StartInfo.RedirectStandardOutput = true;
                                process2.Start();

                                //Console.WriteLine(process2.StandardOutput.ReadToEnd());
                                while (!process2.StandardOutput.EndOfStream)
                                {
                                    Console.WriteLine(process2.StandardOutput.ReadLine());
                                }
                                process2.WaitForExit();
                                Console.WriteLine("完了しました。");
                            }
                            break;

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }//todo:cmdでこれ開けば色とか経過とかいい感じにできないかな？
        }
    }
}
