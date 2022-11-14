using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;

namespace RecruitTaskProfisys
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Document> documents = new List<Document>();

        public MainWindow()
        {
            InitializeComponent();
            DBLoad("DESKTOP-51BJ13O", "RTPS", "sa", "qwerty");
            docs.ItemsSource = documents;
        }

        private void DBLoad(string _src, string _db, string _id, string _pswrd)
        {
            //db connection
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=" + _src + ";Initial Catalog=" + _db + ";User ID=" + _id + ";Password=" + _pswrd + ";";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            //Data read
            Document d;

            SqlCommand command;
            SqlDataReader reader;
            string sql;
            sql = "SELECT * FROM Documents";
            command = new SqlCommand(sql, cnn);
            reader = command.ExecuteReader();

            documents.Clear();

            while (reader.Read())
            {
                d = new Document(Int32.Parse(reader.GetValue(0).ToString()),
                    reader.GetValue(1).ToString(),
                    reader.GetValue(2).ToString(),
                    reader.GetValue(3).ToString(),
                    reader.GetValue(4).ToString(),
                    reader.GetValue(5).ToString());

                documents.Add(d);
            }

            //cleaning
            reader.Close();
            command.Dispose();
            cnn.Close();
        }

        private void ReadCsv(string _src, string _db, string _id, string _pswrd, string _path)
        {
            List<DocumentItem> documentItems = new List<DocumentItem>();

            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=" + _src + ";Initial Catalog=" + _db + ";User ID=" + _id + ";Password=" + _pswrd + ";";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            string[] lines = System.IO.File.ReadAllLines(_path + "Documents.csv");
            string[] cNames = lines[0].Split(';');

            string query = "INSERT INTO Documents VALUES(@" + cNames[0] + ", @" + cNames[1] + ", @" + cNames[2] + ", @" + cNames[3] + ", @" + cNames[4] + ", @" + cNames[5] + ")";
            SqlCommand cmd = new SqlCommand(query, cnn);

            cmd.Parameters.Add("@" + cNames[0], SqlDbType.Int);
            cmd.Parameters.Add("@" + cNames[1], SqlDbType.VarChar);
            cmd.Parameters.Add("@" + cNames[2], SqlDbType.Date);
            cmd.Parameters.Add("@" + cNames[3], SqlDbType.VarChar);
            cmd.Parameters.Add("@" + cNames[4], SqlDbType.VarChar);
            cmd.Parameters.Add("@" + cNames[5], SqlDbType.VarChar);

            foreach (string line in lines.Skip(1))
            {
                string[] c = line.Split(';');
                Document d = new Document(int.Parse(c[0]), c[1], c[2], c[3], c[4], c[5]);
                if (!documents.Exists(x => x.id == int.Parse(c[0])))
                {
                    documents.Add(d);

                    cmd.Parameters["@" + cNames[0]].Value = c[0];
                    cmd.Parameters["@" + cNames[1]].Value = c[1];
                    cmd.Parameters["@" + cNames[2]].Value = c[2];
                    cmd.Parameters["@" + cNames[3]].Value = c[3];
                    cmd.Parameters["@" + cNames[4]].Value = c[4];
                    cmd.Parameters["@" + cNames[5]].Value = c[5];

                    cmd.ExecuteNonQuery();
                }
            }

            cmd.Dispose();

            //Read value from db 
            SqlCommand command;
            SqlDataReader reader;
            string sql;
            sql = "SELECT * FROM DocumentItems";
            command = new SqlCommand(sql, cnn);
            reader = command.ExecuteReader();

            documentItems.Clear();
            DocumentItem tempDi;

            while (reader.Read())
            {
                tempDi = new DocumentItem(Int32.Parse(reader.GetValue(0).ToString()),
                    Int32.Parse(reader.GetValue(1).ToString()),
                    reader.GetValue(2).ToString(),
                    Int32.Parse(reader.GetValue(3).ToString()),
                    float.Parse(reader.GetValue(4).ToString()),
                    Int32.Parse(reader.GetValue(5).ToString()));

                documentItems.Add(tempDi);
            }

            reader.Close();
            command.Dispose();

            //Same process as above but for document Items 

            string[] lines1 = System.IO.File.ReadAllLines(_path + "DocumentItems.csv");
            string[] cNames1 = lines[0].Split(';');

            string query1 = "INSERT INTO DocumentItems VALUES(@" + cNames1[0] + ", @" + cNames1[1] + ", @" + cNames1[2] + ", @" + cNames1[3] + ", @" + cNames1[4] + ", @" + cNames1[5] + ")";
            SqlCommand cmd1 = new SqlCommand(query1, cnn);

            cmd1.Parameters.Add("@" + cNames1[0], SqlDbType.Int);
            cmd1.Parameters.Add("@" + cNames1[1], SqlDbType.Int);
            cmd1.Parameters.Add("@" + cNames1[2], SqlDbType.VarChar);
            cmd1.Parameters.Add("@" + cNames1[3], SqlDbType.Int);
            cmd1.Parameters.Add("@" + cNames1[4], SqlDbType.Float);
            cmd1.Parameters.Add("@" + cNames1[5], SqlDbType.Int);

            foreach (string line in lines1.Skip(1))
            {
                string[] c = line.Split(';');
                DocumentItem di = new DocumentItem(int.Parse(c[0]), int.Parse(c[1]), c[2], int.Parse(c[3]), float.Parse(c[4]), int.Parse(c[5]));
                if (documentItems.Count <= lines1.Length)
                {
                    documentItems.Add(di);

                    cmd1.Parameters["@" + cNames1[0]].Value = c[0];
                    cmd1.Parameters["@" + cNames1[1]].Value = c[1];
                    cmd1.Parameters["@" + cNames1[2]].Value = c[2];
                    cmd1.Parameters["@" + cNames1[3]].Value = c[3];
                    cmd1.Parameters["@" + cNames1[4]].Value = c[4];
                    cmd1.Parameters["@" + cNames1[5]].Value = c[5];

                    cmd1.ExecuteNonQuery();
                }
            }


            cmd1.Dispose();
            cnn.Close();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (docs.SelectedItem == null) return;
            Document d = docs.SelectedItem as Document;
            ItemExplorer ie = new ItemExplorer(d.id);
            ie.Show();

        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            ReadCsv("DESKTOP-51BJ13O", "RTPS", "sa", "qwerty", "C:/Users/HP/Downloads/Zadanie/");
            DBLoad("DESKTOP-51BJ13O", "RTPS", "sa", "qwerty");
        }
    }
}