using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;

namespace CommTools
{

    public class Logger
    {
        public static string _filename { get; set; }
        public static int _level { get; set; }
        //None    6	
        //Not used for writing log messages.Specifies that a logging category should not write any messages.
        //Critical	5	
        //Logs that describe an unrecoverable application or system crash, or a catastrophic failure that requires immediate attention.
        //Error   4	
        //Logs that highlight when the current flow of execution is stopped due to a failure.These should indicate a failure in the current activity, not an application-wide failure.
        //Warning 3	
        //Logs that highlight an abnormal or unexpected event in the application flow, but do not otherwise cause the application execution to stop.
        //Information 2	
        //Logs that track the general flow of the application.These logs should have long-term value.
        //Debug   1	
        //Logs that are used for interactive investigation during development. These logs should primarily contain information useful for debugging and have no long-term value.
        //Trace   0	
        //Logs that contain the most detailed messages.These messages may contain sensitive application data.These messages are disabled by default and should never be enabled in a production environment.

        public static void Init(string filename, int level=2)
        {
            _filename = filename;
            _level = level;
        }
  
        public static void Log(int level, string message,  [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_filename)) return;
                if (level < _level) return;
                File.AppendAllTextAsync(_filename, $"{DateTime.Now}, {caller}, {lineNumber} : { message}"+Environment.NewLine);
            }
            catch
            {

            }
        }

        // c# in dept
        //public sealed class Singleton
        //{
        //    private static readonly Lazy<Singleton>
        //        lazy =
        //        new Lazy<Singleton>
        //            (() => new Singleton());

        //    public static Singleton Instance { get { return lazy.Value; } }

        //    private Singleton()
        //    {
        //    }
        //}


    }

    public abstract class BaseSingleton<T> where T : BaseSingleton<T>
    {
        private static readonly Lazy<T> Lazy =
            new Lazy<T>(() => Activator.CreateInstance(typeof(T), true) as T);

        public static T Instance => Lazy.Value;
    }

    //Child Class

    //public sealed class MyChildSingleton : BaseSingleton<MyChildSingleton>
    //{
    //    private MyChildSingleton() { }
    //}

    public class Singleton<T> where T : class, new()
    {
        private Singleton() { }

        private static readonly Lazy<T> instance = new Lazy<T>(() => Activator.CreateInstance(typeof(T), true) as T);

        public static T Instance { get { return instance.Value; } }
    }
    //Usage pattern:
    //var journalSingleton = Singleton<JournalClass>.Instance;

}
