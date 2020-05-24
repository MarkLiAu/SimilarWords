using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WordSimilarityLib
{
    public class MemoryLogFibonacci
    {
        public string itemId { get; set; }
        public DateTime viewTime { get; set; }
        public int easiness { get; set; }    // user choose the easiness: -2, -1:hard, 0:normal, 1:easy ,2
        public int viewInterval { get; set; }  // viewTime + viewInterval = next view time

        // calculated fields:
        public DateTime startTime { get; set; }
        public int viewCount { get; set; }
        public int lastInterval { get; set; }

        // from word list
        public string meaning { get; set; }     // multiple lines

        public MemoryLogFibonacci()
        {
            itemId = "";
            viewTime = DateTime.Now;
            viewInterval = -1;
            easiness = 0;

            // calculated 
            startTime = default(DateTime);
            viewCount = 0;
            lastInterval = -1;
        }

        public MemoryLogFibonacci(string id, int interval, int easi, DateTime date = default(DateTime) )
        {
            itemId = id;
            viewTime = date == default(DateTime) ? DateTime.Now : date;
            easiness = easi;
            viewInterval = interval;
        }

        public bool isNewItem()
        {
            return viewInterval < 0; 
        }

        public bool isDue()
        {
            if (viewInterval < 0) return false;     // look like a new item
            return viewTime.AddDays(viewInterval)>DateTime.Now;
        }
    }

    public class MemoryFibonacci
    {
        public string fileName { get; set; }
        public List<MemoryLogFibonacci> logList { get; set; }

        public MemoryFibonacci()
        {
            fileName = "";
            logList = new List<MemoryLogFibonacci>();
        }

        public MemoryFibonacci(string file): this()
        {
            fileName = file;
        }

        public int loadFile(string file="")
        {
            logList.Clear();

            if (string.IsNullOrWhiteSpace(file)) file = fileName;
            if (string.IsNullOrWhiteSpace(file) || !File.Exists(file)) return -1;
            string[] lines = File.ReadAllLines(file);
            if (lines.Length <= 0) return 0;

            for(int i=0; i<lines.Length;i++)
            {
                string[] ss = lines[i].Split(new char[] { ',', ';', '\t' });
                if (ss.Length < 3) continue;

                MemoryLogFibonacci log = new MemoryLogFibonacci();
                log.itemId = ss[0].Trim();
                log.viewTime = Convert.ToDateTime(ss[1]);
                log.viewInterval = Convert.ToInt32(ss[2]);
                logList.Add(log);
            }
            return logList.Count;
        }


        public int AddNewItem(string id)
        {
            foreach (var d in logList)
                if (d.itemId == id) return -1;

            MemoryLogFibonacci memoryLog = new MemoryLogFibonacci(id, -1, 0);
            return SaveNextInterval(memoryLog);
        }

        public int SaveNextInterval(MemoryLogFibonacci memoryLog, string delimeter="\t")
        {
            string log = memoryLog.itemId + delimeter + memoryLog.viewTime + delimeter + memoryLog.viewInterval;

            File.AppendAllLines(fileName, new string[] { log });
            return 1;
        }

        public List<MemoryLogFibonacci> getViewList(int maxNewItem=10)
        {
            loadFile();

            List<MemoryLogFibonacci> result = new List<MemoryLogFibonacci>();

            int newItemCount = 0;
            foreach (var d in logList)
            {
                if (d.isNewItem())
                {
                    if (newItemCount >= maxNewItem) continue;
                    result.Add(d);
                    newItemCount++;
                }
                else if(d.isDue())
                {
                    result.Add(d);
                }
            }

            return result;
        }

        //////////////////////////////////////////////////////////////////////
    }
}
