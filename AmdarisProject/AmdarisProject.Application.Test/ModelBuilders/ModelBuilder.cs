using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Test.ModelBuilders
{
    public abstract class ModelBuilder<T, Y>(T model) where T : Model where Y : ModelBuilder<T, Y>
    {
        protected readonly T _model = model;

        public T Get() => _model;

        public abstract Y Clone();


        public Y SetId(Guid id)
        {
            _model.Id = id;
            return (Y)this;
        }
    }
}
