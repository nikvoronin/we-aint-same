using CoenM.ImageHash;
using CoenM.ImageHash.HashAlgorithms;
using System.Diagnostics;
using System.Text.Json;
using WeAintSame;

const string ImageFilesMask = "*.jpg";
const string SamplesFolderName = "Samples2"; // .../User/MyPictures + Samples/*.jpg
const double ImageSimilarityThreshold = 90.0; // percents

string _samplesFolderPath =
    Path.Combine(
        Environment.GetFolderPath( Environment.SpecialFolder.MyPictures ),
        SamplesFolderName );

IImageHash _hashMethod = new PerceptualHash();
List<ImageInfo> _pictures = new();

Stopwatch sw = new();
sw.Start();

var precomputedHashesFileName = PrecomputedHashesFileName();
if ( File.Exists( precomputedHashesFileName ) ) {
    Console.WriteLine( "\n+++ Restoring hashes..." );

    using var fs = File.OpenRead( precomputedHashesFileName );
    _pictures = JsonSerializer.Deserialize<List<ImageInfo>>( fs )!;
}
else {
    Console.WriteLine( "\n+++ Computing hashes..." );

    var _pictureFileNames =
        Directory.GetFiles(
            _samplesFolderPath, ImageFilesMask,
            SearchOption.AllDirectories );

    foreach ( var picturePath in _pictureFileNames ) {
        Console.WriteLine( $"+ {picturePath}" );

        using var stream = File.OpenRead( picturePath );
        ulong hash = _hashMethod.Hash( stream );

        _pictures.Add(
            new ImageInfo( picturePath, hash ) );
    }

    using var fs = File.OpenWrite( PrecomputedHashesFileName() );
    JsonSerializer.Serialize( fs, _pictures );
}

Console.WriteLine( "\n+++ Chasing duplicates..." );

List<DuplicateGroup> _groups = new();

double _maxSim = 0.0;
double _minSim = 200.0;
for ( int c = 0; c < _pictures.Count; c++ ) {
    var current = _pictures[c];
    Console.Write( "." );

    for ( int s = c + 1; s < _pictures.Count; s++ ) {
        var sample = _pictures[s];

        var notSameDiscardEarly = current.InGroup && sample.InGroup;
        if ( notSameDiscardEarly ) continue;

        double similarity = CompareHash.Similarity( current.Hash, sample.Hash );
        _maxSim = Math.Max( _maxSim, similarity );
        _minSim = Math.Min( _minSim, similarity );

        if ( similarity > ImageSimilarityThreshold ) {
            // duplicates found

            var noGroup =
                current.Group is null
                && sample.Group is null;

            if ( noGroup )
                _groups.CreateSameGroupForEach( current, sample );
            else if ( current.InGroup )
                current.MutualDuplicateWith( sample );
            else // if ( sample.InGroup )
                sample.MutualDuplicateWith( current );

            string equalitySign =
                similarity > 99.9 ? "==" 
                : "<>";

            Console.WriteLine( $">>> {
                Path.GetFileName( current.Path )}\n\tdup: {similarity}% {equalitySign} {Path.GetFileName( sample.Path )}" );
        }
    }
}

Console.WriteLine( $"\n+++ Similarity: max= {_maxSim}% / min= {_minSim}%" );
Console.WriteLine( $"\n+++ Duplicate Groups ({_groups.Count}):" );

foreach ( var g in _groups ) {
    Console.WriteLine( $"Group #{g.Id}" );

    foreach ( var iinfo in g.Duplicates ) {
        Console.WriteLine( $"\t{Path.GetFileName( iinfo.Path )}" );

        CopyToSamplesRoot(
            iinfo.Path,
            $"{g.Id}--{Path.GetFileName( iinfo.Path )}" );
    }
}

Console.WriteLine( $"\n+++ TOTAL: {sw.Elapsed}" );
sw.Stop();

string PrecomputedHashesFileName()
    => Path.Combine( _samplesFolderPath, $"hashs.json" );

void CopyToSamplesRoot( string fileName, string? renameTo = null )
{
    return; //////////////////////////////////////////////////////////////////////////// RETURN //
    const bool Overwrite = true;

    var name = Path.GetFileName( renameTo ?? fileName );
    File.Copy(
        fileName,
        Path.Combine( _samplesFolderPath, name ),
        Overwrite );
}
