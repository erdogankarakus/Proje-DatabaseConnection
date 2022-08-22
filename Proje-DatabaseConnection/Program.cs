using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Proje_DatabaseConnection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var datasource = @"DESKTOP-UF4J58T\SQLEXPRESS";//your server
            var database = "Musteri"; //your database name
            var username = "sa"; //username of server to connect
            var password = "123456"; //password
            SqlCommand command;
            string okunanDosya;
            string yazilanDosya;
           
            //your connection string 
            string connString = @"Data Source=" + datasource + ";Initial Catalog="
                        + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

           

            //create instanace of database connection
            SqlConnection conn = new SqlConnection(connString);


            try
            {
             //   Console.WriteLine("Openning Connection ...");

                //open connection
                conn.Open();

             //   Console.WriteLine("Connection successful!");
            }
            catch (Exception e)
            {

                Console.WriteLine("Connection error!");

            }

            //Console.Read();


            List<DataBaseModel> musteriListesi = new List<DataBaseModel>();
            string sql = "SELECT * FROM MusteriBilgileri";
            command = new SqlCommand(sql, conn);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read()) {
                DataBaseModel mus = new DataBaseModel();
                mus.TC = Convert.ToInt32(dr[0]);
                mus.Ad = dr[1].ToString();
                mus.Soyad = dr[2].ToString();
                mus.DogumTarihi = dr[3].ToString();
                musteriListesi.Add(mus);
                

            }
            conn.Close();


            //StreamWriter Yaz = new StreamWriter("C:\\Users\\Erdoğan Karakuş\\Desktop\\deneme.txt");
            string DosyaYolu = "C:\\Users\\Erdoğan Karakuş\\Desktop\\deneme.txt";
            if (!File.Exists(DosyaYolu))
            {
                DosyaYaz(DosyaYolu,musteriListesi);
                    
              
            }
            else
            {
                okunanDosya=DosyaOku(DosyaYolu);
                okunanDosya = Regex.Replace(okunanDosya, "\r\n", "*");

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < musteriListesi.Count; i++)
                {
                    
                    builder.Append(musteriListesi[i].TC);
                    builder.Append(" ");
                    builder.Append(musteriListesi[i].Ad);
                    builder.Append(" ");
                    builder.Append(musteriListesi[i].Soyad);
                    builder.Append(" ");
                    builder.Append(musteriListesi[i].DogumTarihi);
                    builder.Append(" ");
                   

                }
                yazilanDosya = builder.ToString();
                


                // Eski listeyi txt sil seni liste ekle.
                if (yazilanDosya != okunanDosya) {

                    TextWriter tw = new StreamWriter(DosyaYolu);
                    tw.Write("");
                    tw.Close();

                    DosyaYaz(DosyaYolu, musteriListesi);


                }
                
            }


          
            
        }
        static void DosyaYaz(string DosyaYolu, List<DataBaseModel> list)
        {
            FileStream fs = new FileStream(DosyaYolu, FileMode.Append, FileAccess.Write, FileShare.Write);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < list.Count; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(list[i].TC);
                sb.Append(" ");
                sb.Append(list[i].Ad);
                sb.Append(" ");
                sb.Append(list[i].Soyad);
                sb.Append(" ");
                sb.Append(list[i].DogumTarihi);
                sb.Append(" ");
                sb.AppendLine();

                sw.Write(sb);

                

            }

            sw.Close();

        }
        static string  DosyaOku(string DosyaYolu )
        {
            StreamReader sr = new StreamReader(DosyaYolu);
            string txtDosya = sr.ReadToEnd();
            sr.Close();

            return txtDosya;

        }

    }
}
