using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NoteTakingApp
{
    [Serializable]
    class Note
    {
        readonly string msg;
        readonly string title;
        string path;

        public Note(string msg, string title, string path)
        {
            this.msg = msg;
            this.title = title;
            this.path = path;
        }

        public string Message { get { return msg; } }
        public string Title { get { return title; } }
        public string Path { get { return path; } }
    }
}
