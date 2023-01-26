using System.Text;

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
            ArgumentNullException.ThrowIfNull( other );
            ArgumentNullException.ThrowIfNull( current );

            current!
                .Group!
                .AddDuplicate( other );
        }

        public static string AsString( this byte[] hash )
            => hash.Aggregate(
                new StringBuilder(),
                ( builder, bite ) => builder.Append( bite.ToString( "x2" ) ) )
            .ToString();
    }
}