﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserMaintenance.Entities;

namespace UserMaintenance
{
    public partial class Form1 : Form
    {
        BindingList<User> users = new BindingList<User>();
        public Form1()
        {
            InitializeComponent();
            lblFullName.Text = Resource1.FullName;
            btnAdd.Text = Resource1.Add;
            btnSave.Text = Resource1.Save;
            btnDelete.Text = Resource1.Delete;

            listUsers.DataSource = users;
            listUsers.ValueMember = "ID";
            listUsers.DisplayMember = "FullName";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var u = new User()
            {
                FullName = txtFullName.Text
            };
            users.Add(u);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.InitialDirectory = Application.StartupPath; 
            sfd.Filter = "Comma Seperated Values (*.csv)|*.csv";
            sfd.DefaultExt = "csv"; 
            sfd.AddExtension = true;

            if (sfd.ShowDialog() != DialogResult.OK) return;

            using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
            {
                foreach (var u in users)
                {
                    sw.Write(u.ID);
                    sw.Write(";");
                    sw.Write(u.FullName);
                    sw.WriteLine();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selectedUser = (from x in users
                            where x.ID.ToString() == listUsers.SelectedValue.ToString()
                            select x).FirstOrDefault();
            users.Remove(selectedUser);
   
        }
    }
}
