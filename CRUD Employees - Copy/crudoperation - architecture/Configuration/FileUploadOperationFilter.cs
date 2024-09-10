//using Swashbuckle.AspNetCore.SwaggerGen;

//namespace EmployeeManagement.Configuration
//{
//    public class FileUploadOperationFilter : IOperationFilter
//    {
//        public void Apply(Operation operation, OperationFilterContext context)
//        {
//            if (operation.OperationId.ToLower() == "apifileuploaduploadfilepost")
//            {
//                operation.Parameters.Add(new NonBodyParameter()
//                {
//                    Name = "file",
//                    In = "formData",
//                    Description = "Upload File",
//                    Required = true,
//                    Type = "file"
//                });
//                operation.Consumes.Add("multipart/form-data");
//            }
//        }
//    }
//}
