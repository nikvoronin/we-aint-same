namespace WeAintSame
{
    public class DuplicateGroup: IEquatable<DuplicateGroup>
    {
        private static ulong _currentGroupId = 0;
        public ulong Id { get; init; }
        private readonly List<ImageInfo> _duplicates = new();
        public IEnumerable<ImageInfo> Duplicates => _duplicates;

        private DuplicateGroup() { }

        public static DuplicateGroup Create()
            => new() { Id = ++_currentGroupId };

        public void AddRange( IEnumerable<ImageInfo> iis )
        {
            foreach ( var ii in iis )
                AddDuplicate( ii );
        }

        public void AddDuplicate( ImageInfo ii )
        {
            _duplicates.Add( ii );
            ii.Group = this;
        }

        public bool Equals( DuplicateGroup? other )
        {
            if (other is null ) return false;
            if ( ReferenceEquals( this, other ) ) return true;
            if ( GetType() != other.GetType() ) return false;
            return Id == other.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==( DuplicateGroup left, DuplicateGroup right )
        {
            if ( left is null )
                return right is null;

            return left.Equals( right );
        }

        public static bool operator !=( DuplicateGroup left, DuplicateGroup right )
            => !( left == right );

        public override bool Equals( object? obj )
            => Equals( obj as DuplicateGroup );
    }
}
