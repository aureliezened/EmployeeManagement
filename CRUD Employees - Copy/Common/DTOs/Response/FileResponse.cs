using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Response
{
    public class FileResponse
    {
        public byte[] FileContent { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
    }
}
