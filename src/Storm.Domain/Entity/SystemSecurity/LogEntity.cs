using System;

namespace Storm.Domain.Entity.SystemSecurity
{
    public class LogEntity : IEntity<LogEntity>, ICreationAudited
    {
        public string Id { get; set; }
        public DateTime? Date { get; set; }
        public string Account { get; set; }
        public string NickName { get; set; }
        public string Type { get; set; }
        public string IPAddress { get; set; }
        public string IPAddressName { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool? Result { get; set; }
        public string Description { get; set; }
        public DateTime? CreatorTime { get; set; }
        public string CreatorUserId { get; set; }
    }
}
