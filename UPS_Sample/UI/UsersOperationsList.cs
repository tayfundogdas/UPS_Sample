using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UPS_Sample.restops.domain;
using UPS_Sample.restops.operations;

namespace UPS_Sample.UI
{
    public partial class UsersOperationsList : Form
    {

        private List<User> currentUsers;

        public UsersOperationsList()
        {
            InitializeComponent();
        }

        private void UsersOperationsUI_Load(object sender, EventArgs e)
        {
            bindData(null, 1, false);
        }

        private void bindData(string searchTerm, int page, bool skipPageBoxSet)
        {
            var action = new UserActions();
            var model = action.getUsers(page, searchTerm);
            this.currentUsers = model.data;
            var list = new BindingList<User>(model.data);
            dwUsers.DataSource = list;

            if (!skipPageBoxSet)
            {
                //page box set-up
                cmbPageSelect.Items.Clear();
                if (model.meta.pagination.pages == 0)
                {
                    cmbPageSelect.Items.Add(1);
                }
                else
                {
                    for (var i = 1; i <= model.meta.pagination.pages; ++i)
                    {
                        cmbPageSelect.Items.Add(i);
                    }
                }

                cmbPageSelect.SelectedItem = model.meta.pagination.page;
            }
        }

        private void cmbPageSelect_SelectedValueChanged(object sender, EventArgs e)
        {
            bindData(txtSearch.Text, (int)cmbPageSelect.SelectedItem, true);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            bindData(txtSearch.Text, 1, false);
        }

        private void dwUsers_Click(object sender, EventArgs e)
        {
            if (dwUsers.SelectedRows.Count > 0)
            {
                processNewOrEditOperation((int)dwUsers.SelectedRows[0].Cells[0].Value);
            }
        }

        private void processNewOrEditOperation(int currentUserId)
        {
            var modal = new UserOperationsEdit();
            modal.currentId = currentUserId;
            DialogResult dialogResult = modal.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                //refresh
                bindData(txtSearch.Text, (int)cmbPageSelect.SelectedItem, false);
            }
            if (dialogResult == DialogResult.Abort)
            {
                MessageBox.Show("There is a problem action you tried to perform, please check your connection & data");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            processNewOrEditOperation(0);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.currentUsers.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Output.csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            string columnNames = "";
                            string[] outputCsv = new string[this.currentUsers.Count + 1];

                            for (int i = 0; i < dwUsers.Columns.Count; i++)
                            {
                                columnNames += dwUsers.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 0; i < this.currentUsers.Count; i++)
                            {
                                outputCsv[i + 1] += this.currentUsers[i].ToCSVRow();
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("Data Exported Successfully !!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }
    }
}
