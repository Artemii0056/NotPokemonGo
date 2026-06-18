using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace AbilityNew.Diagnostics
{
    public sealed class FileAbilityTraceWriter : IAbilityTraceWriter
    {
        private readonly string _filePath;
        private readonly object _lock = new();

        public FileAbilityTraceWriter(string fileName = "ability_trace.log")
        {
            _filePath = Path.Combine(Application.persistentDataPath, fileName);
            Debug.Log($"Ability trace file path: {_filePath}");
        }

        public void Write(in AbilityTraceRecord record)
        {
            string line = AbilityTraceFormatter.Format(record);

            lock (_lock)
            {
                File.AppendAllText(_filePath, line + Environment.NewLine, Encoding.UTF8);
            }
        }
    }
}