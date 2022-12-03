using System.Text;
using WeAintSame;

namespace WeAintSame
{
    static class Extensions
    {
        public static DuplicateGroup CreateSameGroupForEach(
            this IList<DuplicateGroup> groups,
            params ImageInfo[] mutualDuplicates )
        {
            var newGroup = DuplicateGroup.Create();

            groups.Add( newGroup );
            newGroup.AddRange( mutualDuplicates );

            return newGroup;
        }

        public static void MutualDuplicateWith( this ImageInfo current, ImageInfo other )
        {
            _ = other ?? throw new ArgumentNullException( nameof( other ) );
            _ = current?.Group ?? throw new ArgumentNullException( nameof( current ) );

            current.Group.AddDuplicate( other );
        }

        public static string AsString( this byte[] hash )
            => hash.Aggregate(
                new StringBuilder(),
                ( builder, bite ) => builder.Append( bite.ToString( "x2" ) ) )
            .ToString();
    }
}