using Microsoft.AspNetCore.StaticFiles;

namespace BishalAgroSeed.Dtos;
public class FileBlobDto
{
    public FileBlobDto()
    {
    }

    public FileBlobDto(byte[] content, string fileName)
    {
        Content = content;
        FileName = fileName;
        new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);
        ContentType = contentType;
    }

    public FileBlobDto(byte[] content, string fileName, string contentType)
    {
        Content = content;
        FileName = fileName;
        ContentType = contentType;
    }

    public byte[] Content { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
}
