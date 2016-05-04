using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenBot.Plugins.Interfaces;
using OpenBot.Plugins.Matches;


namespace OpenBotServicesPlugin.Handlers.Forms
{
    public partial class frmBasicChat : Form
    {
        private BasicChatHandler _handler;
        public frmBasicChat(BasicChatHandler handler)
        {
            _handler = handler;
            InitializeComponent();
        }

        private void frmBasicChat_Load(object sender, EventArgs e)
        {
            PopulateItems();
        }

        private void PopulateItems()
        {
            lstCurrentItems.BeginUpdate();
            lstCurrentItems.Items.Clear();

            foreach (var i in _handler.Items)
                lstCurrentItems.Items.Add(i.Key.MatchValue);

            lstCurrentItems.EndUpdate();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            _handler.Items.Remove(_handler.Items.ElementAt(lstCurrentItems.SelectedIndex).Key);
            PopulateItems();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string matchString = txtMatch.Text;
            string outputString = txtOutput.Text;

            IMessageMatch matcher;

            if (rbAbsolute.Checked)
                matcher = new AbsoluteMessageMatch(matchString);
            else if (rbContains.Checked)
                matcher = new ContainsMessageMatch(matchString);
            else if (rbStartsWith.Checked)
                matcher = new StartsWithMessageMatch(matchString);
            else if (rbRegex.Checked)
                matcher = new RegexMessageMatch(matchString);
            else
                throw new Exception("No radio button was selected.");

            _handler.Items.Add(matcher, outputString);
            PopulateItems();
        }
    }
}
