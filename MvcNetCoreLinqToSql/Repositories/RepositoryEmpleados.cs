using MvcNetCoreLinqToSql.Models;
using System.Data;
using System.Data.SqlClient;

namespace MvcNetCoreLinqToSql.Repositories
{
    public class RepositoryEmpleados
    {
        private DataTable tablaEmpleados;

        public RepositoryEmpleados()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            string sql = "select * from EMP";
            SqlDataAdapter adEmp = new SqlDataAdapter(sql, connectionString);
            //INSTANCIAMOS NUESTRO DATATABLE
            this.tablaEmpleados = new DataTable();
            //TRAEMOS LOS DATOS
            adEmp.Fill(tablaEmpleados);
        }

        //METODO PARA RECUPERAR TODOS LOS EMPLEADOS
        public List<Empleado> GetEmpleados()
        {
            //LA CONSULTA LINQ SE ALMACENA EN VARIABLES DE TIPO var
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           select datos;
            //LO QUE TENEMOS ALMACENADO EN CONSULTA ES UN CONJUNTO DE 
            //OBJETOS DataRow, QUE SON LOS OBJETOS QUE CONTIENE LA 
            //CLASE DataTable
            //DEBEMOS CONVERTIR DICHOS OBJETOS DataRow EN EMPLEADOS
            List<Empleado> empleados = new List<Empleado>();
            //RECORREMOS CADA FILA DE LA consulta
            foreach (var row in consulta)
            {
                //PARA EXTRAER LOS DATOS DE UNA FILA DataRow
                //  fila.Field<TIPO>("COLUMNA")
                Empleado emp = new Empleado();
                emp.IdEmpleado = row.Field<int>("EMP_NO");
                emp.Apellido = row.Field<string>("APELLIDO");
                emp.Oficio = row.Field<string>("OFICIO");
                emp.Salario = row.Field<int>("SALARIO");
                emp.IdDepartamento = row.Field<int>("DEPT_NO");
                empleados.Add(emp);
            }
            return empleados;
        }

        //METODO PARA BUSCAR UN EMPLEADO POR SU ID
        public Empleado FindEmpleado(int idEmpleado)
        {
            //EL ALIAS datos REPRESENTA CADA OBJETO DENTRO DEL CONJUNTO
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<int>("EMP_NO") == idEmpleado
                           select datos;
            //NOSOTROS SABEMOS QUE DEVUELVE SOLO UNA FILA, PERO 
            //consulta SIEMPRE SERA UNA COLECCION
            //consulta CONTIENE UNA SERIE DE METODOS Lambda PARA INDICAR 
            //CIERTAS FILAS O ACCIONES NECESARIAS
            //TENEMOS UN METODO LLAMADO First() QUE NOS DEVUELVE LA PRIMERA FILA
            var row = consulta.First();
            Empleado empleado = new Empleado();
            empleado.IdEmpleado = row.Field<int>("EMP_NO");
            empleado.Apellido = row.Field<string>("APELLIDO");
            empleado.Oficio = row.Field<string>("OFICIO");
            empleado.Salario = row.Field<int>("SALARIO");
            empleado.IdDepartamento = row.Field<int>("DEPT_NO");
            return empleado;
        }

        public List<Empleado> GetEmpleadosOficioSalario
            (string oficio, int salario)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           && datos.Field<int>("SALARIO") >= salario
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                //EL CODIGO PARA LA LISTA DE EMPLEADOS
                List<Empleado> empleados = new List<Empleado>();
                foreach (var row in consulta)
                {
                    //SINTAXIS PARA INSTANCIAR UN OBJETO Y RELLENAR SUS 
                    //PROPIEDADES A LA VEZ
                    Empleado empleado = new Empleado
                    {
                        IdEmpleado = row.Field<int>("EMP_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Oficio = row.Field<string>("OFICIO"),
                        Salario = row.Field<int>("SALARIO"),
                        IdDepartamento = row.Field<int>("DEPT_NO")
                    };
                    empleados.Add(empleado);
                }
                return empleados;
            }
        }

    }
}
