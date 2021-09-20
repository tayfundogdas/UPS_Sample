using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UPS_Sample.restops.domain;
using UPS_Sample.restops.operations;

namespace UPS_Sample.UI
{
    public partial class UserOperationsEdit : Form
    {
        public int currentId { get; set; }
        private User currentUserData { get; set; }

        public UserOperationsEdit()
        {
            InitializeComponent();
        }

        private void UserOperationsEdit_Load(object sender, EventArgs e)
        {
            if (currentId > 0)
            {
                //edit
                var action = new UserActions();
                this.currentUserData = action.loadUser(currentId);
            }
            else
            {
                //new
                this.currentUserData = new User();
            }

            //bind data
            this.txtId.Text = this.currentUserData.id.ToString();
            this.txtName.Text = this.currentUserData.name;
            this.txtEmail.Text = this.currentUserData.email;
            this.cmbGender.SelectedItem = this.currentUserData.gender;
            this.cmbStatus.SelectedItem = this.currentUserData.status;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            //refresh data
            this.currentUserData.name = this.txtName.Text;
            this.currentUserData.email = this.txtEmail.Text;
            this.currentUserData.gender = this.cmbGender.SelectedItem.ToString();
            this.currentUserData.status = this.cmbStatus.SelectedItem.ToString();

            var action = new UserActions();
            if (this.currentUserData.id == 0 && action.createUser(currentUserData))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (this.currentUserData.id > 0 && action.updateUser(currentUserData))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var action = new UserActions();
            if (this.currentUserData.id > 0 && action.deleteUser(this.currentUserData.id))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }
    }
}
