using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Test.ModelBuilder.ModelBuilder
{
    internal abstract class ModelBuilder<T, Y>(T model) where T : Model where Y : ModelBuilder<T, Y>
    {
        protected static ulong _instances = 0;
        protected readonly T _model = model;

        public T Get() => _model;

        public Y SetId(ulong id)
        {
            _model.Id = id;
            return (Y)this;
        }
    }
}
