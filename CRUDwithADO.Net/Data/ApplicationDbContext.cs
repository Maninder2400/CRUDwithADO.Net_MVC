using CRUDwithADO.Net.Models;
using System.Data;
using System.Data.SqlClient;
namespace CRUDwithADO.Net.Data
{
    public class ApplicationDbContext
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext(IConfiguration configuration) {
            _configuration = configuration;
        }

        public  List<Employee> GetEmployeesList()
        {
            List<Employee> employees = new List<Employee>();
            string? cs = _configuration.GetConnectionString("DefaultConnection");
            SqlConnection sqlConnection = new(cs);
            SqlCommand cmd = new("GetAllEmployees_sp", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            sqlConnection.Open();
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            while (sqlDataReader.Read()) {
                Employee employee = new Employee();
                employee.Id = Convert.ToInt32( sqlDataReader.GetValue(0).ToString());
                employee.Name = sqlDataReader.GetValue(1).ToString();
                employee.Age = Convert.ToInt32( sqlDataReader.GetValue(2).ToString());
                employee.City = sqlDataReader.GetValue(3).ToString();
                employee.State = sqlDataReader.GetValue(4).ToString();
                employee.Salary = Convert.ToDouble( sqlDataReader.GetValue(5).ToString());
                employees.Add(employee);
            }
            sqlConnection.Close();
            return employees; 
        }

        public bool AddNewEmployee(Employee employee)
        {
            string? cs = _configuration.GetConnectionString("DefaultConnection");
            SqlConnection sqlConnection = new(cs);
            SqlCommand cmd = new("AddEmployee_sp",sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name",employee.Name);
            cmd.Parameters.AddWithValue("@age",employee.Age);
            cmd.Parameters.AddWithValue("@city",employee.City);
            cmd.Parameters.AddWithValue("@state",employee.State);
            cmd.Parameters.AddWithValue("@salary",employee.Salary);
            sqlConnection.Open();
            int i = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            return i > 0 ? true : false;
        }

        public bool DeleteEmployeeByID(int id)
        {
            string? cs = _configuration.GetConnectionString("DefaultConnection");
            SqlConnection sqlConnection = new(cs);
            SqlCommand cmd = new("DeleteEmployeeByID_sp",sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            sqlConnection.Open();
            int i = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            return i > 0 ? true : false;
        }

        public bool UpdateEmployeeByID(Employee employee)
        {
            string? cs = _configuration.GetConnectionString("DefaultConnection");
            SqlConnection sqlConnection = new(cs);
            SqlCommand cmd = new("UpdateEmployeeByID_sp", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", employee.Id);
            cmd.Parameters.AddWithValue("@name", employee.Name);
            cmd.Parameters.AddWithValue("@age", employee.Age);
            cmd.Parameters.AddWithValue("@city", employee.City);
            cmd.Parameters.AddWithValue("@state", employee.State);
            cmd.Parameters.AddWithValue("@salary", employee.Salary);
            sqlConnection.Open();
            int i = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            return i > 0 ? true : false;
        }

        public Employee GetEmployeeByID(int id)
        {
            string? cs = _configuration.GetConnectionString("DefaultConnection");
            SqlConnection sqlConnection = new(cs);
            SqlCommand cmd = new("select * from Employee where id=@id", sqlConnection);
            cmd.Parameters.AddWithValue("@id",id);
            sqlConnection.Open();
            SqlDataReader dataReader = cmd.ExecuteReader();
            Employee employee = new Employee();
            while (dataReader.Read())
            {
                employee.Id = Convert.ToInt32(dataReader.GetValue(0).ToString());
                employee.Name = dataReader.GetValue(1).ToString();
                employee.Age = Convert.ToInt32(dataReader.GetValue(2).ToString());
                employee.City = dataReader.GetValue(3).ToString();
                employee.State = dataReader.GetValue(4).ToString();
                employee.Salary = Convert.ToDouble(dataReader.GetValue(5).ToString());
            }
            sqlConnection.Close();
            return employee;
        }
    }
}
