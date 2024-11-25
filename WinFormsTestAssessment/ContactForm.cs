using System.Text.Json;
using WinFormsTestAssessment.Models;

namespace WinFormsTestAssessment
{
    public partial class ContactForm : Form
    {
        private List<Company> companies = new();
        private List<Contact> contacts = new();
        private ListView contactListView;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtTelephone;
        private TextBox txtEmail;
        private ComboBox cboCompany;
        private Button btnSaveContact;

        public ContactForm()
        {
            InitializeComponent();
            LoadCompanies();


            this.Text = "Contacts Management";
            var lblFirstName = new Label { Text = "First Name*", Top = 10, Left = 10 };
            txtFirstName = new TextBox { Top = 10, Left = 120, Width = 200 };
            var lblLastName = new Label { Text = "Last Name*", Top = 40, Left = 10 };
            txtLastName = new TextBox { Top = 40, Left = 120, Width = 200 };
            var lblTelephone = new Label { Text = "Telephone*", Top = 70, Left = 10 };
            txtTelephone = new TextBox { Top = 70, Left = 120, Width = 200 };
            var lblEmail = new Label { Text = "Email*", Top = 100, Left = 10 };
            txtEmail = new TextBox { Top = 100, Left = 120, Width = 200 };
            var lblCompany = new Label { Text = "Company*", Top = 130, Left = 10 };
            cboCompany = new ComboBox { Top = 130, Left = 120, Width = 200 };

            cboCompany.Items.AddRange(companies.Select(i => i.Name).ToArray());

            var btnAddContact = new Button { Text = "Add Contact", Top = 160, Left = 10 };
            btnAddContact.Click += (sender, e) => AddOrUpdateContact(null);

            btnSaveContact = new Button { Text = "Save Contact", Top = 160, Left = 100, Enabled = false };
            btnSaveContact.Click += (sender, e) => AddOrUpdateContact(contactListView.SelectedItems[0]);

            contactListView = new ListView { Top = 200, Left = 10, Width = 380, Height = 200, FullRowSelect = true };
            contactListView.View = View.Details;
            contactListView.Columns.Add("First Name", 100);
            contactListView.Columns.Add("Last Name", 100);
            contactListView.Columns.Add("Telephone", 100);
            contactListView.Columns.Add("Email", 100);
            contactListView.Columns.Add("Company", 100);
            contactListView.ItemSelectionChanged += (sender, e) => LoadSelectedContact();

            this.Controls.Add(lblFirstName);
            this.Controls.Add(txtFirstName);
            this.Controls.Add(lblLastName);
            this.Controls.Add(txtLastName);
            this.Controls.Add(lblTelephone);
            this.Controls.Add(txtTelephone);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblCompany);
            this.Controls.Add(cboCompany);
            this.Controls.Add(btnAddContact);
            this.Controls.Add(btnSaveContact);
            this.Controls.Add(contactListView);

            LoadContacts();
        }

        private void AddOrUpdateContact(ListViewItem item)
        {
            if (cboCompany.SelectedItem == null)
            {
                MessageBox.Show("Please select the company!");
                return;
            } 

            if (item == null)
            {
                var contact = new Contact
                {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Telephone = txtTelephone.Text,
                    Email = txtEmail.Text,
                    CompanyName = cboCompany.SelectedItem.ToString()
                };

                if (ValidateContact(contact))
                {
                    contacts.Add(contact);
                    SaveContacts();
                    LoadContacts();
                    MessageBox.Show("Contact added successfully!");
                    ClearForm();
                }
                else
                    MessageBox.Show("Please fill in required fields!");
            }
            else
            {
                var contact = contacts[item.Index];
                contact.FirstName = txtFirstName.Text;
                contact.LastName = txtLastName.Text;
                contact.Telephone = txtTelephone.Text;
                contact.Email = txtEmail.Text;
                contact.CompanyName = cboCompany.SelectedItem.ToString();

                if (ValidateContact(contact))
                {
                    SaveContacts();
                    LoadContacts();
                    MessageBox.Show("Contact updated successfully!");
                    ClearForm();
                }
                else
                    MessageBox.Show("Please fill in required fields!");
            }
        }

        private bool ValidateContact(Contact contact)
        {
            return !string.IsNullOrEmpty(contact.FirstName) && !string.IsNullOrEmpty(contact.LastName) &&
                   !string.IsNullOrEmpty(contact.Email) && !string.IsNullOrEmpty(contact.Telephone);
        }

        private void LoadSelectedContact()
        {
            if (contactListView.SelectedItems.Count > 0)
            {
                var item = contactListView.SelectedItems[0];
                txtFirstName.Text = item.SubItems[0].Text;
                txtLastName.Text = item.SubItems[1].Text;
                txtTelephone.Text = item.SubItems[2].Text;
                txtEmail.Text = item.SubItems[3].Text;
                cboCompany.SelectedItem = item.SubItems[4].Text;
                btnSaveContact.Enabled = true;
            }
            else
            {
                ClearForm();
            }
        }

        private void SaveContacts()
        {
            var json = JsonSerializer.Serialize(contacts);
            File.WriteAllText("contacts.json", json);
        }

        private void LoadContacts()
        {
            if (File.Exists("contacts.json"))
            {
                var json = File.ReadAllText("contacts.json");
                contacts = JsonSerializer.Deserialize<List<Contact>>(json);
            }

            contactListView.Items.Clear();
            contactListView.Items.AddRange(contacts.Select(i =>
                new ListViewItem(new[] { i.FirstName, i.LastName, i.Telephone, i.Email, i.CompanyName })).ToArray());
        }

        private void LoadCompanies()
        {
            if (File.Exists("companies.json"))
            {
                var json = File.ReadAllText("companies.json");
                companies = JsonSerializer.Deserialize<List<Company>>(json);
            }
        }

        private void ClearForm()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtTelephone.Text = "";
            txtEmail.Text = "";
            cboCompany.SelectedItem = null;
            btnSaveContact.Enabled = false;
        }
    }
}
