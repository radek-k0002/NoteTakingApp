using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NoteTakingApp
{
    [Serializable]
    class NoteContainer : IEnumerable, ISerializable
    {
        List<Note> notes;
        int counts;

        public NoteContainer()
        {
            notes = new List<Note>();
            counts = 0;
        }

        private NoteContainer(SerializationInfo info, StreamingContext context)
        {
            try
            {
                notes = (List<Note>)info.GetValue("Notes", new List<Note>().GetType());
                counts = info.GetInt32("Counts");
            }
            catch (Exception evt)
            {
                MainWindow.SerialiazingError(evt);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Notes", notes, notes.GetType());
            info.AddValue("Counts", counts);
        }

        public void Add(Note msg)
        {
            notes.Add(msg);
            counts += 1;
        }

        public int FindByPath(string pth)
        {
            return notes.FindIndex((Note n) => n.Path == pth);
        }

        public void Remove(int index)
        {
            if (notes.Count > index)
            {
                notes.RemoveAt(index);
                counts -= 1;
            }
        }

        public void Remove(Note msg)
        {
            notes.Remove(msg);
            counts -= 1;
        }

        public int Counts { get { return counts; } }

        public Note this[int i]
        {
            get { return notes[i]; }
            set { notes[i] = value; }
        }

        public IEnumerator GetEnumerator()
        {
            return notes.GetEnumerator();
        }


    }
}
