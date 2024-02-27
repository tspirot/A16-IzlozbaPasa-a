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

namespace A16_IzlozbaPasa_a
{

    public partial class Form1 : Form
    {
        SqlConnection konekcija;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                konekcija = new SqlConnection
            (@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\A16.mdf;Integrated Security=True");
                osveziPse();
                osveziIzlozbe();
                osveziKategorije();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greska u konekciji!");
            }
        }
        // osvezi combo box pasa
        private void osveziPse()
        {

            try
            {
                konekcija.Open();
                SqlCommand komanda = new SqlCommand
                    ("SELECT PasID,CONCAT(PasID,' - ',Ime) AS ImePsa FROM Pas", 
                    konekcija);
                DataTable tabela = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(komanda);
                adapter.Fill(tabela);
                comboBoxPas.DataSource = tabela;
                comboBoxPas.DisplayMember = "ImePsa";
                comboBoxPas.ValueMember = "PasID";
            }
            catch (Exception)
            {
                MessageBox.Show("Greska u citanju pasa!");
            }
            finally
            {
                konekcija.Close();
            }
        }
        // osvezi combo box izlozbi
        private void osveziIzlozbe()
        {

            try
            {
                konekcija.Open();
                SqlCommand komanda = new SqlCommand
                    ("SELECT IzlozbaID,CONCAT(IzlozbaID,' - ',Mesto," +
                    "' - ',Datum) AS ImeIzlozbe FROM Izlozba",
                    konekcija);
                DataTable tabela = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(komanda);
                adapter.Fill(tabela);
                comboBoxIzlozba.DataSource = tabela;
                comboBoxIzlozba.DisplayMember = "ImeIzlozbe";
                comboBoxIzlozba.ValueMember = "IzlozbaID";
                comboBox1.DataSource = tabela;
                comboBox1.DisplayMember = "ImeIzlozbe";
                comboBox1.ValueMember = "IzlozbaID";
            }
            catch (Exception)
            {
                MessageBox.Show("Greska u citanju pasa!");
            }
            finally
            {
                konekcija.Close();
            }
        }
        // osvezi combo box kategorija
        private void osveziKategorije()
        {

            try
            {
                konekcija.Open();
                SqlCommand komanda = new SqlCommand
                    ("SELECT KategorijaID,CONCAT(KategorijaID,' - ',Naziv) " +
                    "AS ImeKategorije FROM Kategorija", konekcija);
                DataTable tabela = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(komanda);
                adapter.Fill(tabela);

                comboBoxKategorija.DataSource = tabela;
                comboBoxKategorija.DisplayMember = "ImeKategorije";
                comboBoxKategorija.ValueMember = "KategorijaID";
            }
            catch (Exception)
            {
                MessageBox.Show("Greska u citanju pasa!");
            }
            finally
            {
                konekcija.Close();
            }
        }

        private void buttonDodaj_Click(object sender, EventArgs e)
        {
            // provera da li je pas prijavljen
            string sqlProvera = "SELECT * FROM Rezultat " +
                "WHERE PasID=@PasID " +
                "AND IzlozbaID=@IzlozbaID " +
                "AND KategorijaID=@KategorijaID";
            SqlCommand komandaProvera = new 
                SqlCommand(sqlProvera, konekcija);
            komandaProvera.Parameters.AddWithValue
                ("@PasID", comboBoxPas.SelectedValue);
            komandaProvera.Parameters.AddWithValue
                ("@IzlozbaID", comboBoxIzlozba.SelectedValue);
            komandaProvera.Parameters.AddWithValue
                ("@KategorijaID", comboBoxKategorija.SelectedValue);
            SqlDataAdapter ad= new SqlDataAdapter(komandaProvera);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Pas je vec prijavljen!");
                return;
            }
            // prijava psa
            string sqlPrijava = "INSERT INTO Rezultat " +
                "(PasID,IzlozbaID,KategorijaID) " +
                "VALUES (@PasID,@IzlozbaID,@KategorijaID)";
            SqlCommand komandaPrijava = new SqlCommand
                (sqlPrijava, konekcija);
            komandaPrijava.Parameters.AddWithValue
                ("@PasID", comboBoxPas.SelectedValue);
            komandaPrijava.Parameters.AddWithValue
                ("@IzlozbaID", comboBoxIzlozba.SelectedValue);
            komandaPrijava.Parameters.AddWithValue
                ("@KategorijaID", comboBoxKategorija.SelectedValue);
            try
            {
                konekcija.Open();
                komandaPrijava.ExecuteNonQuery();
                MessageBox.Show("Pas je uspesno prijavljen!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greska pri prijavi psa! " + ex.Message);
            }
            finally
            {
                konekcija.Close();
            }
        }

        private void buttonZatvori_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
