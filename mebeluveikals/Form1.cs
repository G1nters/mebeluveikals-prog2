using Microsoft.Data.Sqlite;
using System.ComponentModel.DataAnnotations;

namespace mebeluveikals
{
    public partial class Form1 : Form
    {
        private FurnitureManager furnitureManager;

        public Form1()
        {
            InitializeComponent();

            furnitureManager = new FurnitureManager("Data Source=furniture.db");

            var furniture = furnitureManager.ReadFurniture();
            var furnitureNames = new List<string>();

            foreach (var f in furniture)
            {
                furnitureNames.Add(f.Name);
            }

            selectProductComboBox.DataSource = furnitureNames;
        }

        private void selectBtn_Click(object sender, EventArgs e)
        {
            var furniture = furnitureManager.ReadFurnitureByName(selectProductComboBox.Text);

            nameTextBox.Text = furniture.Name;
            descTextBox.Text = furniture.Description;
            priceTextBox.Text = furniture.Price.ToString();
            hTextBox.Text = furniture.Height.ToString();
            wTextBox.Text = furniture.Width.ToString();
            lTextBox.Text = furniture.Length.ToString();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(nameTextBox.Text))
                {
                    MessageBox.Show("Nav norādīts nosaukums.");
                }
                else if (string.IsNullOrEmpty(descTextBox.Text))
                {
                    MessageBox.Show("Nav norādīts apraksts.");
                }
                else if (string.IsNullOrEmpty(priceTextBox.Text))
                {
                    MessageBox.Show("Nav norādīta cena.");
                }
                else if (string.IsNullOrEmpty(hTextBox.Text))
                {
                    MessageBox.Show("Nav norādīts augstums.");
                }
                else if (string.IsNullOrEmpty(wTextBox.Text))
                {
                    MessageBox.Show("Nav norādīts platums.");
                }
                else if (string.IsNullOrEmpty(lTextBox.Text))
                {
                    MessageBox.Show("Nav norādīts garums.");
                }


                furnitureManager.AddFurniture(nameTextBox.Text, descTextBox.Text,
                    Convert.ToDouble(priceTextBox.Text), Convert.ToInt32(hTextBox.Text),
                    Convert.ToInt32(wTextBox.Text), Convert.ToInt32(lTextBox.Text));

                List<string> furnitureList = (List<string>)selectProductComboBox.DataSource;
                furnitureList.Add(nameTextBox.Text);

                selectProductComboBox.DataSource = null;
                selectProductComboBox.DataSource = furnitureList;

                MessageBox.Show("Ieraksts tika pievienots datubāzei");
            }
            catch (SqliteException ex)
            {
                MessageBox.Show("Notikusi SQL kļūda.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Notikusi kļūda.");
            }
        }



            private void exportCsvButton_Click(object sender, EventArgs e)
{
    var furnitureManager = new FurnitureManager("Data Source=furniture.db");
    var furnitureList = furnitureManager.ReadFurniture();
    var filePath = "furniture_export.csv";

    using (var writer = new StreamWriter(filePath))
    {
        writer.WriteLine("Name,Description,Price,Height,Width,Length");
        foreach (var furniture in furnitureList)
        {
            writer.WriteLine($"{furniture.Name},{furniture.Description},{furniture.Price},{furniture.Height},{furniture.Width},{furniture.Length}");
        }
    }

    MessageBox.Show("CSV fails veiksmīgi eksportēts: " + filePath);
}

private void importCsvButton_Click(object sender, EventArgs e)
{
    var filePath = "furniture_import.csv"; // Aizvietot ar dialoga ceļu, ja nepieciešams
    if (!File.Exists(filePath))
    {
        MessageBox.Show("Importa fails netika atrasts.");
        return;
    }

    var furnitureManager = new FurnitureManager("Data Source=furniture.db");

    using (var reader = new StreamReader(filePath))
    {
        reader.ReadLine(); // Pārlēkt galvenes rindai
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line.Split(',');

            var name = values[0];
            var description = values[1];
            var price = double.Parse(values[2]);
            var height = int.Parse(values[3]);
            var width = int.Parse(values[4]);
            var length = int.Parse(values[5]);

            try
            {
                // Mēģinām pievienot jaunu ierakstu
                furnitureManager.AddFurniture(name, description, price, height, width, length);
            }
            catch
            {
                // Ja eksistē, tad atjaunojam
                furnitureManager.DeleteFurnitureByName(name);
                furnitureManager.AddFurniture(name, description, price, height, width, length);
            }
        }
    }

    MessageBox.Show("Dati no CSV veiksmīgi importēti.");
}




        private void deleteBtn_Click(object sender, EventArgs e)
        {
            furnitureManager.DeleteFurnitureByName(selectProductComboBox.Text);

            List<string> furnitureList = (List<string>)selectProductComboBox.DataSource;
            furnitureList.Remove(selectProductComboBox.Text);

            selectProductComboBox.DataSource = null;
            selectProductComboBox.DataSource = furnitureList;

            MessageBox.Show("Mēbele tika izdzēsta no datubāzes.");
        }
    }
}
