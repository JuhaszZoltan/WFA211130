using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA211130
{
    public partial class FrmMain : Form
    {
        public string ConnectionString { private get; set; }
        public FrmMain()
        {
            ConnectionString = 
                "Server = (localdb)\\MSSQLLocalDB;" +
                "Database = verseny;";
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            FillDGV();
            FillCB();
        }

        private void FillCB()
        {
            using (var c = new SqlConnection(ConnectionString))
            {
                c.Open();
                var r = new SqlCommand(
                    "SELECT DISTINCT nemzetiseg FROM versenyzo;",
                    c).ExecuteReader();
                while (r.Read()) cbNem.Items.Add(r[0]);
            }
        }

        private void FillDGV()
        {
            dgv.Rows.Clear();

            using (var c = new SqlConnection(ConnectionString))
            {
                c.Open();

                string whereNemzetiseg = cbNem.SelectedIndex == -1
                    ? "" : $"AND nemzetiseg = '{cbNem.Text}' ";

                var r = new SqlCommand(
                    "SELECT versenyzo.id, nev, csapatNev, nemzetiseg " +
                    "FROM versenyzo " +
                    "INNER JOIN csapat ON csapatId = csapat.id " +
                    $"WHERE (nev LIKE '{tbKereses.Text}%' " +
                        $"OR csapatnev LIKE '{tbKereses.Text}%') " +
                        whereNemzetiseg +
                    "ORDER BY nev;", c).ExecuteReader();

                while (r.Read())
                {
                    dgv.Rows.Add(r[0], r[1], r[2], r[3]);
                }
            }
        }

        private void TbKereses_TextChanged(object sender, EventArgs e)
        {
            FillDGV();
        }

        private void cbNem_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDGV();
        }
    }
}
