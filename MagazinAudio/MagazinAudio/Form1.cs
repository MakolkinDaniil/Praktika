using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MagazinAudio
{
    public partial class Autorization : Form
    {
        DataBase dataBase = new DataBase();
        public Autorization()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (textBoxLogin.Text != "" && textBoxPassword.Text != "")
            { 
                using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-TCO0SQK\SQLEXPRESS;Initial Catalog=MagazinAudio; Integrated Security=true"))
                {
                    try
                    {
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandText = "select Password from [Staff] where Login = '" + textBoxLogin.Text + "'";
                        string password = Convert.ToString(cmd.ExecuteScalar());
                        if (password != "")
                        {
                            if (textBoxPassword.Text == password) // если пароль верный
                            {
                                cmd.CommandText = "select [Post ID] from [Staff] where Login = '" + textBoxLogin.Text + "'";
                                string Staff = Convert.ToString(cmd.ExecuteScalar());

                                switch (Staff)
                                {
                                    case "1":
                                        {
                                            MessageBox.Show("Добро пожаловать!");
                                            Admin Admin = new Admin();
                                            Admin.Show();
                                            this.Hide();
                                        }
                                        break;
                                    case "2":
                                        {
                                            MessageBox.Show("Добро пожаловать!");
                                            Admin Admin = new Admin();
                                            Admin.Show();
                                            this.Hide();
                                        }
                                        break;
                                }
                            }
                            else { MessageBox.Show("Не верный пароль!"); }
                        }
                        else { MessageBox.Show("Не верный Email!"); }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Convert.ToString(ex));
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
