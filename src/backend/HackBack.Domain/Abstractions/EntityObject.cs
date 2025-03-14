namespace HackBack.Application.Abstractions
{
    public abstract class EntityObject<TId>(TId id)
    {
        public TId Id { get; protected set; } = id ?? throw new ArgumentNullException(nameof(id), "Id cannot be null.");

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (EntityObject<TId>)obj;
            return EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return Id?.GetHashCode() ?? 0;
        }
    }
}
