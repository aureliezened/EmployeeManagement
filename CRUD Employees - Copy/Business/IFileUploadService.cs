using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public interface IFileUploadService
    {
        Task UpdateProfilePictureUrlAsync(Guid employeeId, string profilePictureUrl);
    }
}
