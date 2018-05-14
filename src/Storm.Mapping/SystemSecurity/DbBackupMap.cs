using Storm.Domain.Entity.SystemSecurity;
using System.Data.Entity.ModelConfiguration;

namespace Storm.Mapping.SystemSecurity
{
    public class DbBackupMap : EntityTypeConfiguration<DbBackupEntity>
    {
        public DbBackupMap()
        {
            this.ToTable("Sys_DbBackup");
            this.HasKey(t => t.Id);
        }
    }
}
