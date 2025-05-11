using FCG.Domain.Common;

namespace FCG.Infrastructure.Common;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(Type type) : base(FormatMessage(type))
    {
    }

    public EntityNotFoundException(Type type, Exception innerException) : base(FormatMessage(type), innerException)
    {
    }

    private static string FormatMessage(Type type)
    {
        return $"Entidade '{type.Name}' não encontrada.";
    }

    /// <summary>
    /// Se o contexto usar generics, é possível usar este método, para garantir que o tipo seja um BaseEntity.
    /// </summary>
    public static EntityNotFoundException For<T>() where T : BaseEntity
    {
        return new EntityNotFoundException(typeof(T));
    }
}