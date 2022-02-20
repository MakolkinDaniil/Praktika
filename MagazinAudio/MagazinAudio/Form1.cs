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
            textBoxPassword.PasswordChar = '*';
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
                        cmd.CommandText = "select Password from [Staff] where Email = '" + textBoxLogin.Text + "'";
                        string password = Convert.ToString(cmd.ExecuteScalar());
                        if (password != "")
                        {
                            if (textBoxPassword.Text == password)
                            {
                                cmd.CommandText = "select [Post ID] from [Staff] where Email = '" + textBoxLogin.Text + "'";
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
                                    case "3":
                                        {
                                            MessageBox.Show("Добро пожаловать!");
                                            Admin Admin = new Admin();
                                            Admin.Show();
                                            this.Hide();
                                        }
                                        break;
                                }
                            }
                            else { MessageBox.Show("Неверный пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                        }
                        else { MessageBox.Show("Неверный логин!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
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
            else
            {
                MessageBox.Show("Не все поля заполнены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e) // закрыть окно
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) // показать пароль
        {
            if (textBoxPassword.PasswordChar == '*')
            {
                button1.BringToFront();
                textBoxPassword.PasswordChar = '\0';
            }
        }
        private void button1_Click(object sender, EventArgs e) // скрыть пароль
        {
            if (textBoxPassword.PasswordChar == '\0')
            {
                button2.BringToFront();
                textBoxPassword.PasswordChar = '*';
            }
        }
    }
}
