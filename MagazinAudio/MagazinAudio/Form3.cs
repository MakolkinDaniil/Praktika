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
    public partial class Products : Form
    {
        DataBase database = new DataBase(); // подключение к БД
        int selectedRow; // выбранная ячейка
        
        public Products()
        {
            InitializeComponent();
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("Product ID", "Id");
            dataGridView1.Columns.Add("Name", "Наименование");
            dataGridView1.Columns.Add("Product type ID", "Id типа товара");
            dataGridView1.Columns.Add("Manufacturer ID", "Id производителя");
            dataGridView1.Columns.Add("Purchase price", "Закупочная цена");
            dataGridView1.Columns.Add("Retail price", "Розничная цена");
            dataGridView1.Columns.Add("IsNew", String.Empty);
            dataGridView1.Columns[6].Visible = false;
        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetInt32(2), record.GetInt32(3), 
                record.GetInt32(4), record.GetInt32(5), RowState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string queryString = $"select * from Products";
            SqlCommand command = new SqlCommand(queryString, database.getConnection());
            database.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Admin Admin = new Admin();
            Admin.Show();
            this.Hide();
        }

        private void Products_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                textBoxId.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                textBox4.Text = row.Cells[3].Value.ToString();
                textBox5.Text = row.Cells[4].Value.ToString();
                textBox6.Text = row.Cells[5].Value.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProductsAdd ProductsAdd = new ProductsAdd();
            ProductsAdd.Show();
        }

        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string searchString = $"select * from Products where concat ([Product ID], Name, [Product type ID], [Manufacturer ID], [Purchase price], [Retail price]) like '%" + textBox1.Text + "%'";
            SqlCommand com = new SqlCommand(searchString, database.getConnection());
            database.openConnection();
            SqlDataReader read = com.ExecuteReader();
            while(read.Read())
            {
                ReadSingleRow(dgw, read);
            }

            read.Close();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        enum RowState // состояние данных в таблице
        {
            Existed,
            New,
            Modified,
            ModifiedNew,
            Deleted
        }

        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[index].Visible = false;
            dataGridView1.Rows[index].Cells[6].Value = RowState.Deleted;
            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                return;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            deleteRow();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Update2();
        }
        private void Update2()
        {
            database.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var RowState = (RowState)dataGridView1.Rows[index].Cells[6].Value;

                if (RowState == RowState.Existed)
                    continue;

                if (RowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from Products where [Product ID] = {id}";
                    var command = new SqlCommand(deleteQuery, database.getConnection());
                    command.ExecuteNonQuery();
                }
                if (RowState == RowState.Modified)
                {
                    var ProductID = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var name = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var idTypeProduct = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var idManufacturer = dataGridView1.Rows[index].Cells[3].Value.ToString();
                    var purchasePrice = dataGridView1.Rows[index].Cells[4].Value.ToString();
                    var retailPrice = dataGridView1.Rows[index].Cells[5].Value.ToString();

                    var changeQuery = $"update Products set Name = '{name}',[Product type ID] = '{idTypeProduct}'," +
                        $"[Manufacturer ID] = '{idManufacturer}',[Purchase price] = '{purchasePrice}',[Retail price] = '{retailPrice}' where [Product ID] = '{ProductID}'";
                    var command = new SqlCommand(changeQuery, database.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            database.closeConnection();
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var ProductID = textBoxId.Text;
            var name = textBox2.Text;
            int idTypeProduct;
            int idManufacturer;
            int purchasePrice;
            int retailPrice;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            if (int.TryParse(textBox3.Text, out idTypeProduct) && textBox2.Text != "0")
            {
                if (int.TryParse(textBox4.Text, out idManufacturer) && textBox3.Text != "0")
                {
                    if (int.TryParse(textBox5.Text, out purchasePrice) && textBox4.Text != "0")
                    {
                        if (int.TryParse(textBox6.Text, out retailPrice) && textBox5.Text != "0")
                        {
                                dataGridView1.Rows[selectedRowIndex].SetValues(ProductID, name, idTypeProduct, idManufacturer, purchasePrice, retailPrice);
                                dataGridView1.Rows[selectedRowIndex].Cells[6].Value = RowState.Modified;
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
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            Change();
        }
    }
}
