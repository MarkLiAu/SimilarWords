using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace WordSimilarityLib
{
    public class MergeSubtitles
    {

        public static void merge(string path)
        {
            string outputFile = Path.Combine(path, "out.txt");
            FileInfo [] files = new DirectoryInfo(path).GetFiles("*.720p.*.srt", SearchOption.AllDirectories);
            files = files.OrderBy(x => x.FullName).ToArray();
            foreach(var f in files)
            {
                string[] subtutles = File.ReadAllLines(f.FullName);
                var sub2 = CleanSubtitle(subtutles);
                File.AppendAllText(outputFile, Environment.NewLine + "========================" + Environment.NewLine + f.Name + Environment.NewLine+ "========================" + Environment.NewLine);
                File.AppendAllLines(outputFile, sub2);
            }
        }

        static List<string> CleanSubtitle(string[] subtitle)
        {
            List<string> result = new List<string>();
            for(int i=0;i<subtitle.Length;i++)
            {
                if (i< subtitle.Length-1 && subtitle[i].Length > 0 && char.IsDigit(subtitle[i][0]) && subtitle[i + 1].Contains("-->")) { i++; continue; }
                result.Add(subtitle[i]);
            }
            return result;
        }


    }
}
