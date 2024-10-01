using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace FinalWorkDbApp
{
    public partial class Form1 : Form
    {
        private DataTable dataTable;

        public Form1()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            string connectionString = $"Data Source=(local);Initial Catalog=Northwind;Integrated Security=True;TrustServerCertificate=True;;User Id={usernameTextBox.Text};Password={passwordTextBox.Text};";

            try
            {
                sqlServerConnection = new SqlConnection(connectionString);
                sqlServerConnection.Open();
                MessageBox.Show("Connected successfully!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"sqlServerConnection failed: {ex.Message}");
            }

        }

        private void LoadData()
        {
            string query = "SELECT CustomerID, CompanyName FROM Northwind.dbo.Customers";
            sqlServerDataAdapter = new SqlDataAdapter(query, sqlServerConnection);
            dataTable = new DataTable();
            sqlServerDataAdapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }


        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow newRow = dataTable.NewRow();
                newRow["CustomerID"] = column1TextBox.Text;
                newRow["CompanyName"] = column2TextBox.Text;

                dataTable.Rows.Add(newRow);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(sqlServerDataAdapter);
                sqlServerDataAdapter.Update(dataTable);

                MessageBox.Show("Data added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding data: {ex.Message}");
            }

        }
        
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                if (selectedRow != null)
                {
                    selectedRow.Cells["CustomerID"].Value = column1TextBox.Text;
                    selectedRow.Cells["CompanyName"].Value = column2TextBox.Text;
                    
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(sqlServerDataAdapter);
                    sqlServerDataAdapter.Update(dataTable);

                    MessageBox.Show("Data updated successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating data: {ex.Message}");
            }

        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(sqlServerDataAdapter);
                    sqlServerDataAdapter.Update(dataTable);

                    MessageBox.Show("Data deleted successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting data: {ex.Message}");
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlServerConnection != null && sqlServerConnection.State == ConnectionState.Open)
            {
                sqlServerConnection.Close();
            }

        }
        }
        }