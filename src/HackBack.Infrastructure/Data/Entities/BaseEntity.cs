namespace ZazaTemplate.Infrastructure.Data.Entities;

public class BaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;
}
