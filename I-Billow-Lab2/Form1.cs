//Isaac Billow
//CPT-206-A01H
//Lab 2
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace I_Billow_Lab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void cityBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.cityBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.cityDBDataSet);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'cityDBDataSet.City' table. You can move, or remove it, as needed.
            this.cityTableAdapter.Fill(this.cityDBDataSet.City);
            cityDataGridView.DataSource = cityBindingSource;

        }

        private void btnSortPopAscend_Click(object sender, EventArgs e)
        {
            cityBindingSource.Sort = "Population ASC";
        }

        private void btnSortPopDesc_Click(object sender, EventArgs e)
        {
            cityBindingSource.Sort = "Population DESC";
        }

        private void btnSortCityAsc_Click(object sender, EventArgs e)
        {
            cityBindingSource.Sort = "City ASC";
        }

        private void btnSortCityDesc_Click(object sender, EventArgs e)
        {
            cityBindingSource.Sort = "City DESC";
        }

        private void btnTotalPop_Click(object sender, EventArgs e)
        {
            var totalPopulation = cityDBDataSet.City.AsEnumerable()
                .Sum(row => row.Field<int>("Population"));
            lblResult.Text = $"Total Population: {totalPopulation.ToString("N0")}";
        }

        private void btnAveragePop_Click(object sender, EventArgs e)
        {
            var averagePopulation = cityDBDataSet.City.AsEnumerable()
                .Average(row => row.Field<int>("Population"));
            lblResult.Text = $"Average Population: {averagePopulation.ToString("N0")}";
        }

        private void btnMaxPop_Click(object sender, EventArgs e)
        {
            var maxPopulation = cityDBDataSet.City.AsEnumerable()
                .Max(row => row.Field<int>("Population"));
            lblResult.Text = $"Highest Population: {maxPopulation.ToString("N0")}";
        }

        private void btnMinPop_Click(object sender, EventArgs e)
        {
            var minPopulation = cityDBDataSet.City.AsEnumerable()
                .Min(row => row.Field<int>("Population"));
            lblResult.Text = $"Lowest Population: {minPopulation.ToString("N0")}";
        }

        private void btnClearResult_Click(object sender, EventArgs e)
        {
            lblResult.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cityBindingSource.Current == null)
            {
                return;
            }

            DataRowView row = (DataRowView)cityBindingSource.Current;
            row["City"] = txtCity.Text.Trim();
            row["State"] = txtState.Text.Trim();
            row["Population"] = int.Parse(txtPopulation.Text.Trim());

            this.Validate();
            cityBindingSource.EndEdit();
            cityTableAdapter.Update(cityDBDataSet.City);

            txtCity.Clear();
            txtState.Clear();
            txtPopulation.Clear();

            MessageBox.Show("Changes saved successfully.");
        }

        private void btnAddCity_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCity.Text) ||
                string.IsNullOrWhiteSpace(txtState.Text) ||
                string.IsNullOrWhiteSpace(txtPopulation.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }
            if (!int.TryParse(txtPopulation.Text.Trim(), out int population))
            {
                MessageBox.Show("Please enter a valid number for population.");
                return;
            }
            if (population < 0)
            {
                MessageBox.Show("Population cannot be negative.");
                return;
            }
            
            DataRow newRow = cityDBDataSet.City.NewRow();
            newRow["City"] = txtCity.Text.Trim();
            newRow["State"] = txtState.Text.Trim();
            newRow["Population"] = population;

            cityDBDataSet.City.Rows.Add(newRow);
            cityTableAdapter.Update(cityDBDataSet.City);

            txtCity.Clear();
            txtState.Clear();
            txtPopulation.Clear();
            
            MessageBox.Show("New city added successfully.");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cityBindingSource.Current != null)
            {
                cityBindingSource.RemoveCurrent();
                cityTableAdapter.Update(cityDBDataSet.City);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (cityDataGridView.CurrentRow == null)
            {
                return;
            }
            txtCity.Text = cityDataGridView.CurrentRow.Cells["dataGridViewTextBoxColumn2"].Value.ToString();
            txtState.Text = cityDataGridView.CurrentRow.Cells["dataGridViewTextBoxColumn3"].Value.ToString();
            txtPopulation.Text = cityDataGridView.CurrentRow.Cells["dataGridViewTextBoxColumn4"].Value.ToString();
        }
    }
}
