using System.Text.Json;
using WinFormsTestAssessment.Models;

namespace WinFormsTestAssessment
{
    public partial class CompanyForm : Form
    {
        private List<Company> companies = new List<Company>();
        private ListView companyListView;
        private TextBox txtName;
        private TextBox txtAddress1;
        private TextBox txtAddress2;
        private TextBox txtZipCode;
        private TextBox txtTelephone;
        private Button btnSaveCompany;

        public CompanyForm()
        {
            InitializeComponent();


            this.Text = "Company Management";
            var lblName = new Label { Text = "Name*", Top = 10, Left = 10 };
            txtName = new TextBox { Top = 10, Left = 120, Width = 200 };
            var lblAddress1 = new Label { Text = "Address Line 1*", Top = 40, Left = 10 };
            txtAddress1 = new TextBox { Top = 40, Left = 120, Width = 200 };
            var lblAddress2 = new Label { Text = "Address Line 2", Top = 70, Left = 10 };
            txtAddress2 = new TextBox { Top = 70, Left = 120, Width = 200 };
            var lblZipCode = new Label { Text = "Zip Code*", Top = 100, Left = 10 };
            txtZipCode = new TextBox { Top = 100, Left = 120, Width = 200 };
            var lblTelephone = new Label { Text = "Telephone*", Top = 130, Left = 10 };
            txtTelephone = new TextBox { Top = 130, Left = 120, Width = 200 };

            var btnAddCompany = new Button { Text = "Add Company", Top = 160, Left = 10 };
            btnAddCompany.Click += (sender, e) => AddOrUpdateCompany(null);

            btnSaveCompany = new Button { Text = "Save Company", Top = 160, Left = 100, Enabled = false };
            btnSaveCompany.Click += (sender, e) => AddOrUpdateCompany(companyListView.SelectedItems[0]);

            companyListView = new ListView { Top = 200, Left = 10, Width = 380, Height = 200, FullRowSelect = true };
            companyListView.View = View.Details;
            companyListView.Columns.Add("Name", 100);
            companyListView.Columns.Add("Address1", 100);
            companyListView.Columns.Add("Address2", 100);
            companyListView.Columns.Add("Zip Code", 80);
            companyListView.Columns.Add("Telephone", 100);
            companyListView.ItemSelectionChanged += (sender, e) => LoadSelectedCompany();

            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblAddress1);
            this.Controls.Add(txtAddress1);
            this.Controls.Add(lblAddress2);
            this.Controls.Add(txtAddress2);
            this.Controls.Add(lblZipCode);
            this.Controls.Add(txtZipCode);
            this.Controls.Add(lblTelephone);
            this.Controls.Add(txtTelephone);
            this.Controls.Add(btnAddCompany);
            this.Controls.Add(btnSaveCompany);
            this.Controls.Add(companyListView);

            LoadCompanies();
        }

        private void AddOrUpdateCompany(ListViewItem item)
        {
            if (item == null)
            {
                var company = new Company
                {
                    Name = txtName.Text,
                    Address1 = txtAddress1.Text,
                    Address2 = txtAddress2.Text,
                    ZipCode = txtZipCode.Text,
                    Telephone = txtTelephone.Text
                };

                if (ValidateCompany(company))
                {
                    companies.Add(company);
                    SaveCompanies();
                    LoadCompanies();
                    MessageBox.Show("Company added successfully!");
                    ClearForm();
                }
                else
                    MessageBox.Show("Please fill in required fields!");
            }
            else
            {
                var company = companies[item.Index];
                company.Name = txtName.Text;
                company.Address1 = txtAddress1.Text;
                company.Address2 = txtAddress2.Text;
                company.ZipCode = txtZipCode.Text;
                company.Telephone = txtTelephone.Text;

                if (ValidateCompany(company))
                {
                    SaveCompanies();
                    LoadCompanies();

                    MessageBox.Show("Company updated successfully!");
                    ClearForm();
                }
                else
                    MessageBox.Show("Please fill in required fields!");
            }
        }

        private bool ValidateCompany(Company company)
        {
            
            return !string.IsNullOrEmpty(company.Name) && !string.IsNullOrEmpty(company.Address1) &&
                   !string.IsNullOrEmpty(company.ZipCode) && !string.IsNullOrEmpty(company.Telephone);
        }

        private void LoadSelectedCompany()
        {
            if (companyListView.SelectedItems.Count > 0)
            {
                var item = companyListView.SelectedItems[0];
                txtName.Text = item.SubItems[0].Text;
                txtAddress1.Text = item.SubItems[1].Text;
                txtAddress2.Text = item.SubItems[2].Text;
                txtZipCode.Text = item.SubItems[3].Text;
                txtTelephone.Text = item.SubItems[4].Text;
                btnSaveCompany.Enabled = true;
            }
            else
            {
                ClearForm();
            }
        }

        private void SaveCompanies()
        {
            var json = JsonSerializer.Serialize(companies);
            File.WriteAllText("companies.json", json);
        }

        private void LoadCompanies()
        {
            if (File.Exists("companies.json"))
            {
                var json = File.ReadAllText("companies.json");
                companies = JsonSerializer.Deserialize<List<Company>>(json);
            }
            companyListView.Items.Clear();

            companyListView.Items.AddRange(companies.Select(i =>
                new ListViewItem(new[] { i.Name, i.Address1, i.Address2, i.ZipCode, i.Telephone })).ToArray());
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtZipCode.Text = "";
            txtTelephone.Text = "";
            btnSaveCompany.Enabled = false;
        }
    }
}
