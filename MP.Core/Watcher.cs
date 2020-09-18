using System;
using System.IO;
using System.Threading.Tasks;

namespace MP.Core
{
    public class Watcher
    {
        public Watcher()
        {
        }
        public void Watch()
        {
            var path = "/";
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = path;

                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastWrite
                                     | NotifyFilters.CreationTime
                                     | NotifyFilters.Size
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                watcher.Filter = "";

                // Add event handlers.
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnDeleted;
                watcher.Renamed += OnRenamed;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Press 'q' to quit the sample.");
                while (Console.Read() != 'q') ;
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {

        }

        private void OnDeleted(object source, FileSystemEventArgs e)
        {

        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {

        }
    }
}
