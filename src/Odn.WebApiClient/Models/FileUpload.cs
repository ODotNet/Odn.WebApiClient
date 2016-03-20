using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Odn.WebApiClient
{
    public class FileUpload
    {
        public FileUpload(string originalFileName, string contentType, Stream stream)
        {
            OriginalFileName = originalFileName;
            ContentType = contentType;
            Stream = stream;
        }

        public string NewFileName { get; set; }
        public string OriginalFileName { get; set; }
        public string ContentType { get; set; }
        public Stream Stream { get; set; }
        public int FileType { get; set; }

        public static IEnumerable<FileUpload> GetUploadFiles(HttpRequestBase request)
        {
            foreach (string uploadFile in request.Files)
            {
                var file = request.Files[uploadFile];

                if (!(file != null && file.ContentLength > 0)) continue;

                yield return new FileUpload(file.FileName, file.ContentType, file.InputStream); ;
            }
        }
    }
}
