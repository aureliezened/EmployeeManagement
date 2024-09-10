namespace Common.Models
{
    public class WebRole
    {
        public int webRoleId { get; set; }
        public string webRoleName { get; set; }

        private DateTime createdAt {  get; set; }
        public ICollection<WebUser> WebUsers { get; set; }

    }
}