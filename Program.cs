using CoenM.ImageHash;
using CoenM.ImageHash.HashAlgorithms;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

const string ImageFilesMask = "*.jpg";
const string SamplesFolderName = "Samples2"; // .../User/MyPictures + Samples/*.jpg
const double ImageSimilarityThreshold = 90.0; // percents

string samplesFolderPath =
    Path.Combine(
        Environment.GetFolderPath(
            Environment.SpecialFolder.MyPictures ),
        SamplesFolderName );

IImageHash hashMethod = new PerceptualHash();
List<ImageInfo> pictures = [];
var imageHashComparer =
    new ImageHashComparer( ImageSimilarityThreshold );

var sw = Stopwatch.StartNew();

var precomputedHashesFileName =
    Path.Combine( samplesFolderPath, "hashs.json" );

if (File.Exists( precomputedHashesFileName )) {
    Console.WriteLine( "\n+++ Restoring hashes..." );

    using var fs = File.OpenRead( precomputedHashesFileName );
    pictures = JsonSerializer.Deserialize<List<ImageInfo>>( fs )!;
}
else {
    Console.WriteLine( "\n+++ Computing hashes..." );

    var _pictureFileNames =
        Directory.GetFiles(
            samplesFolderPath, ImageFilesMask,
            SearchOption.AllDirectories );

    foreach (var picturePath in _pictureFileNames) {
        Console.WriteLine( $"+ {picturePath}" );

        using var stream = File.OpenRead( picturePath );
        ulong hash = hashMethod.Hash( stream );

        pictures.Add(
            new ImageInfo( picturePath, hash ) );
    }

    using var fs = File.OpenWrite( precomputedHashesFileName );

    JsonSerializer.Serialize( fs, pictures );
}

Console.WriteLine( "\n+++ Chasing duplicates..." );

var _groups =
    pictures
    .GroupBy( x => x.Hash, imageHashComparer )
    .Where( grp => grp.Count() > 1 ) // remove not duplicates
    .Select( grp => grp.ToList() )
    .ToList();

Console.WriteLine( $"\n+++ Similarity: max= {imageHashComparer.MaxSim}% / min= {imageHashComparer.MinSim}%" );
Console.WriteLine( $"\n+++ Duplicate Groups ({_groups.Count}):" );

var groupIx = 0;
foreach (var group in _groups) {
    Console.WriteLine( $"Group #{groupIx}" );

    foreach (var iinfo in group)
        Console.WriteLine( $"\t{Path.GetFileName( iinfo.Path )}" );

    groupIx++;
}

Console.WriteLine( $"\n+++ TOTAL: {sw.Elapsed}" );
sw.Stop();

file class ImageInfo( string path, ulong hash )
{
    public string Path { get; init; } = path;
    public ulong Hash { get; init; } = hash;
}

file class ImageHashComparer( double threshold ) : IEqualityComparer<ulong>
{
    public double MinSim { get; private set; } = 200.0;
    public double MaxSim { get; private set; }

    public bool Equals( ulong x, ulong y )
    {
        double similarity = CompareHash.Similarity( x, y );

        MaxSim = Math.Max( MaxSim, similarity );
        MinSim = Math.Min( MinSim, similarity );

        return similarity > _threshold;
    }

    // always return zero to always call Equals(x, y)
    public int GetHashCode( [DisallowNull] ulong obj ) => 0;

    private readonly double _threshold = threshold;
}