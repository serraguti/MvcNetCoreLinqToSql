using MvcNetCoreLinqToSql.Models;
using System.Data;
using System.Data.SqlClient;

namespace MvcNetCoreLinqToSql.Repositories
{
    public class RepositoryEnfermos
    {
        //CONSULTAS DE SELECCION
        private DataTable tablaEnfermos;
        //CONSULTAS DE ACCION
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryEnfermos()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            //CONSULTAS DE ACCION
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            //CONSULTAS DE SELECCION LINQ
            string sql = "select * from ENFERMO";
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            this.tablaEnfermos = new DataTable();
            ad.Fill(tablaEnfermos);
        }

        public List<Enfermo> GetEnfermos()
        {
            var consulta = from datos in this.tablaEnfermos.AsEnumerable()
                           select datos;
            List<Enfermo> enfermos = new List<Enfermo>();
            foreach (var row in consulta)
            {
                Enfermo enf = new Enfermo
                {
                    Inscripcion = row.Field<int>("INSCRIPCION"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Direccion = row.Field<string>("DIRECCION"),
                    FechaNacimiento = row.Field<DateTime>("FECHA_NAC"),
                    Genero = row.Field<string>("S")
                };
                enfermos.Add(enf);
            }
            return enfermos;
        }

        public Enfermo FindEnfermo(int inscripcion)
        {
            var consulta = from datos in this.tablaEnfermos.AsEnumerable()
                           where datos.Field<int>("INSCRIPCION") == inscripcion
                           select datos;
            var row = consulta.First();
            Enfermo enfermo = new Enfermo
            {
                Inscripcion = row.Field<int>("INSCRIPCION"),
                Apellido = row.Field<string>("APELLIDO"),
                Direccion = row.Field<string>("DIRECCION"),
                FechaNacimiento = row.Field<DateTime>("FECHA_NAC"),
                Genero = row.Field<string>("S")
            };
            return enfermo;
        }

        public void DeleteEnfermo(int inscripcion)
        {
            string sql = "delete from ENFERMO where INSCRIPCION=@inscripcion";
            this.com.Parameters.AddWithValue("@inscripcion", inscripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
