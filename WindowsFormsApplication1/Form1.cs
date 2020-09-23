using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FormStud : Form
    {
        private BindingSource bindingSource = new BindingSource();
        DataSet dataset = new DataSet();

        public FormStud()
        {
            InitializeComponent();

            ReadData();
        }

        private void кэшированиеДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadData();
        }

        void ReadData()
        {
            // соединяемся с сервером
            SqlConnection connection = new SqlConnection("Data Source=LENOVO-PC\\SQLEXPRESS;Initial Catalog=students;Integrated Security=True;Pooling=false");
            connection.Open();
            // подготавливаем команду 
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM students";
            command.Connection = connection;
            // создаем адаптер
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            // заполняем набор данных
            adapter.Fill(dataset);
            // закрываем соединение, которое нам больше не нужно
            connection.Close();
            // связываем набор данных с сеткой через посредника bindingSource
            dataGridView1.AutoGenerateColumns = true;
            bindingSource.DataSource = dataset.Tables[0];
            dataGridView1.DataSource = bindingSource;
        }

        private void отправитьДанныеНаСерверToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source=LENOVO-PC\\SQLEXPRESS;Initial Catalog=students;Integrated Security=True;Pooling=false");
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM students", connection);

            adapter.UpdateCommand = new SqlCommand("UPDATE students SET Имя = @name, Фамилия = @fam, Группа = @group, Курс = @course, Факультет = @faculty " +
                "WHERE Id = @id");

            adapter.UpdateCommand.Parameters.Add("@name", SqlDbType.NChar, 10, "Имя");
            adapter.UpdateCommand.Parameters.Add("@fam", SqlDbType.NChar,10, "Фамилия");
            adapter.UpdateCommand.Parameters.Add("@group", SqlDbType.NChar, 10, "Группа");
            adapter.UpdateCommand.Parameters.Add("@course", SqlDbType.NChar, 10, "Курс");
            adapter.UpdateCommand.Parameters.Add("@faculty", SqlDbType.NChar, 10, "Факультет");
            adapter.UpdateCommand.Parameters.Add("@id", SqlDbType.Int, 10, "Id");

            adapter.UpdateCommand.Connection = connection;

            adapter.Update(dataset.Tables[0]);
            connection.Close();
        }
    }
}
