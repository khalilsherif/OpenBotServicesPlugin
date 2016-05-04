using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBot.Plugins;
using OpenBot.Plugins.Delegates;
using OpenBot.Plugins.Interfaces;
using OpenBot.Plugins.Matches;
using OpenBotServicesPlugin.Handlers.Forms;

namespace OpenBotServicesPlugin.Handlers
{
    public class BasicChatHandler : AbstractChatHandler
    {
        private Dictionary<IMessageMatch, string> _items;
        public Dictionary<IMessageMatch, string> Items
        {
            get { return _items; }
        }

        public override Dictionary<IMessageMatch, MessageReceivedDelegate> MessageHandlers
        {
            get
            {
                Dictionary<IMessageMatch, MessageReceivedDelegate> _returnDictionary = new Dictionary<IMessageMatch, MessageReceivedDelegate>();

                for (int i = 0; i < Items.Count; i++)
                {
                    int u = i;
                    _returnDictionary.Add(Items.ElementAt(i).Key, (a, b, c, d, e) => {return MasterMessageReceivedDelegate(a, b, c, d, e, u); });
                }
                   

                return _returnDictionary;
            }
        }

        private bool MasterMessageReceivedDelegate(IChatUser sender, string message, string[] args, string raw, bool handled, int index)
        {
            API.Adapter.SendMessage(Items.ElementAt(index).Value);
            return true;
        }

        public override Dictionary<IMessageMatch, RawCommandReceivedDelegate> RawCommandHandlers
        {
            get
            {
                return null;
            }
        }

        public override bool HasPreferences
        {
            get
            {
                return true;
            }
        }

        public BasicChatHandler()
        {
            _items = new Dictionary<IMessageMatch, string>();
        }

        public override void Created()
        {

        }

        public override void Destroyed()
        {
            
        }

        public override void ShowPreferences()
        {
            frmBasicChat basicChatForm = new frmBasicChat(this);
            basicChatForm.Show();
        }
    }
}
