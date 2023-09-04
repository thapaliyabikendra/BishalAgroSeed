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
    }

    public byte[] Content { get; set; }
   public string FileName { get; set; }
}
