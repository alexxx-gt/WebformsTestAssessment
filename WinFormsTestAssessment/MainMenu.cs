namespace WinFormsTestAssessment
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
            this.Text = "Main Menu";
            var btnCompany = new Button { Text = "Manage Companies", Dock = DockStyle.Top };
            btnCompany.Click += (sender, e) =>
            {
                var companyForm = new CompanyForm();
                companyForm.Show();
            };
            var btnContact = new Button { Text = "Manage Contacts", Dock = DockStyle.Top };
            btnContact.Click += (sender, e) =>
            {
                var contactForm = new ContactForm();
                contactForm.Show();
            };
            this.Controls.Add(btnContact);
            this.Controls.Add(btnCompany);
        }
    }
}
