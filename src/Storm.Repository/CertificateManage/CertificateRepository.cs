using Storm.Data;
using Storm.Domain.Entity.CertificateManage;
using Storm.Domain.IRepository.CertificateManage;

namespace Storm.Repository.CertificateManage
{
    public class CertificateRepository : RepositoryBase<CertificateEntity>, ICertificateRepository
    {
    }
}
