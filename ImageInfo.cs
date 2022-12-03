using System.Text.Json.Serialization;

namespace WeAintSame
{
    public class ImageInfo
    {
        public string Path { get; set; }
        public ulong Hash { get; set; }
        [JsonIgnore] public DuplicateGroup? Group;

        public bool InGroup => Group is not null;

        public ImageInfo( string path, ulong hash )
        {
            Path = path;
            Hash = hash;
        }
    }
}