namespace Common.Models
{
    public class Department
    {
        public int departmentId { get; set; }
        public string departmentName { get; set; }
        public ICollection<Employee> Employees { get; set; }


    }
}
