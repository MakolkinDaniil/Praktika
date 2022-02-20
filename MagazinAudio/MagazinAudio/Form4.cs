using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MagazinAudio
{
    public partial class ProductsAdd : Form
    {
        DataBase database = new DataBase();
        public ProductsAdd()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            database.openConnection();
            var name = textBox1.Text;
            int idTypeProduct;
            int idManufacturer;
            int purchasePrice;
            int retailPrice;

            if (int.TryParse(textBox2.Text, out idTypeProduct) && textBox2.Text != "0") 
            {
                if (int.TryParse(textBox3.Text, out idManufacturer) && textBox3.Text != "0") 
                {
                    if (int.TryParse(textBox4.Text, out purchasePrice) && textBox4.Text != "0")
                    {
                        if (int.TryParse(textBox5.Text, out retailPrice) && textBox5.Text != "0")
                        {
                            var addQuery = $"insert into Products (Name, [Product type ID], [Manufacturer ID], [Purchase price], " +
                                $"[Retail price]) values ('{name}','{idTypeProduct}','{idManufacturer}','{purchasePrice}','{retailPrice}')";
                            var command = new SqlCommand(addQuery, database.getConnection());
                            command.ExecuteNonQuery();
                            MessageBox.Show("Запись добавлена", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Розничная цена должна иметь числовой формат и иметь значение больше нуля!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Закупочная цена должна иметь числовой формат и иметь значение больше нуля!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Идентификатор производителя должен иметь числовой формат и иметь значение больше нуля!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Идентификатор типа товара должен иметь числовой формат и иметь значение больше нуля!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            database.closeConnection();
        }
    }
}
