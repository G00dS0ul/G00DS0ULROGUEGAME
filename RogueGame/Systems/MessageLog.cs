using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace RogueGame.Systems
{
    public class MessageLog
    {
        private static readonly int _maxLines = 10;

        private readonly Queue<string> _messages;

        public MessageLog()
        {
            _messages = new Queue<string>();
        }

        public void Add(string message)
        {
            _messages.Enqueue(message);

            if (_messages.Count > _maxLines)
            {
                _messages.Dequeue();
            }
        }

        public void Draw(RLConsole console)
        {
            console.Clear();
            var messages = _messages.ToArray();
            for (var i = 0; i < messages.Length; i++)
            {
                console.Print(1, i + 1, messages[i], RLColor.White);
            }
        }
    }
}
