
using Updater.Core.Abstractions;
using Updater.Core.Services;
using Updater.UI;


namespace Updater.SampleApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
          
        }

     

        private void button1_Click(object sender, EventArgs e)
        {
            new UpdaterForm().Show(); 
        }
    }
}
