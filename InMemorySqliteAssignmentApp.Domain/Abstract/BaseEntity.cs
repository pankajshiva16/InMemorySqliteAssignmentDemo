
using Swashbuckle.AspNetCore.Annotations;

namespace InMemorySqliteAssignmentApp.Domain.Abstract
{
    public class BaseEntity<TIdType>
    {
        [SwaggerSchema(ReadOnly = true)]
        public TIdType Id { get; set; }
    }
}
