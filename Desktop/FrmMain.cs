using Desktop.Model;
using Desktop.Repository;
using MetroFramework.Forms;
using System;

namespace Desktop
{
    public partial class FrmMain : MetroForm
    {
        public LoginResult LoginResult { get; set; }

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            var authRepository = new AuthRepository();
            LoginResult = authRepository.Login();

            Text += $@" (Current User: {LoginResult.FirstName})";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var contactRepository = new ContactRepository(LoginResult.JwtToken);
            var contacts = contactRepository.Find(textBox1.Text);

            metroGrid1.DataSource = contacts;
        }
    }
}
