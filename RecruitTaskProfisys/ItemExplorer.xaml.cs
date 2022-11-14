using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace RecruitTaskProfisys
{
    /// <summary>
    /// Logika interakcji dla klasy ItemExplorer.xaml
    /// </summary>
    public partial class ItemExplorer : Window
    {
        public int DocId;
        List<DocumentItem> documentItems = new List<DocumentItem>();
        public ItemExplorer(int _Id)
        {
            InitializeComponent();
            DocId = _Id;
            items.ItemsSource = documentItems;
            DBLoad("DESKTOP-51BJ13O", "RTPS", "sa", "qwerty");
        }

        private void DBLoad(string _src, string _db, string _id, string _pswrd)
        {
            //db connection
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=" + _src + ";Initial Catalog=" + _db + ";User ID=" + _id + ";Password=" + _pswrd + ";";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            //cnn.Close();

            //Data read
            DocumentItem d;

            SqlCommand command;
            SqlDataReader reader;
            string sql;
            sql = "SELECT * FROM DocumentItems WHERE DocumentId = " + DocId.ToString();
            command = new SqlCommand(sql, cnn);
            reader = command.ExecuteReader();

            documentItems.Clear();

            while (reader.Read())
            {
                d = new DocumentItem(Int32.Parse(reader.GetValue(0).ToString()),
                    Int32.Parse(reader.GetValue(1).ToString()),
                    reader.GetValue(2).ToString(),
                    Int32.Parse(reader.GetValue(3).ToString()),
                    float.Parse(reader.GetValue(4).ToString()),
                    Int32.Parse(reader.GetValue(5).ToString()));

                documentItems.Add(d);
            }

            //cleaning
            reader.Close();
            command.Dispose();
            cnn.Close();
        }
    }
}
