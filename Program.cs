using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;

namespace HolaMundoConsola
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            //DateTime date2 = new DateTime(2021, 7, 13, 0, 0, 0);
            
            //int result = DateTime.Compare(DateTime.Now, date2);

            ////Console.WriteLine(date2.ToString());

            //Console.WriteLine(DateTime.Now.ToString());

            Revisar_Caracteres();
        }

        private static void Revisar_Caracteres()
        {
            string connStringUtil;

            connStringUtil = "data source=128.1.200.167; initial catalog=Canella_SISCON; persist security info=True; user id=usrsap; password=C@nella20$";

            string query = @"select * from VW_SBO_PRODUCTOS";

            DataTable dt = new DataTable();

            using (SqlConnection connUtil = new SqlConnection(connStringUtil))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(query, connUtil))
                {
                    connUtil.Open();

                    da.Fill(dt);

                    connUtil.Close();
                }
            }

            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                string hexString = "";

                string strProducto = dt.Rows[i - 1]["cod_producto"].ToString();

                if (strProducto != "")
                {

                    for (int j = 0; j <= (strProducto.Length - 1); j++)
                    {

                        byte[] ba1 = Encoding.Default.GetBytes(strProducto.Substring(j, 1));

                        hexString = BitConverter.ToString(ba1);

                        if (hexString == "02")
                        {
                            Console.WriteLine("Este es el producto " + j.ToString() + ' ' + strProducto + ' ' + dt.Rows[i - 1]["descripcion"].ToString());
                        }
                    }
                }

                string strDescripcion = dt.Rows[i - 1]["descripcion"].ToString();

                if (strDescripcion != "")
                {

                    for (int k = 0; k <= (strDescripcion.Length - 1); k++)
                    {

                        byte[] ba2 = Encoding.Default.GetBytes(strDescripcion.Substring(k, 1));

                        hexString = BitConverter.ToString(ba2);

                        if (hexString == "02")
                        {
                            Console.WriteLine("Este es la descripción " + strProducto + ' ' + strDescripcion);
                        }
                    }
                }

                //Console.WriteLine(hexString.ToString());
            }          
        }

        private void HashString()
        {
            string source = "Arrecis25$";

            using (SHA256 sha256Hash = SHA256.Create())
            {
                string hash = GetHash(sha256Hash, source);

                Console.WriteLine($"The SHA256 hash of {source} is: {hash}.");

                Console.WriteLine("Verifying the hash...");

                if (VerifyHash(sha256Hash, source, hash))
                {
                    Console.WriteLine("The hashes are the same.");
                }
                else
                {
                    Console.WriteLine("The hashes are not same.");
                }
            }

            //string alex = Passwoird
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            // Hash the input.
            var hashOfInput = GetHash(hashAlgorithm, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }

        public static string HashPassword(string password)
        {
            var salt = Crypto.GenerateSalt();
            var saltedPassword = password + salt;
            var hashedPassword = Crypto.HashPassword(saltedPassword);
            return hashedPassword;
        }
    }
}
