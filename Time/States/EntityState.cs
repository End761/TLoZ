using Microsoft.Xna.Framework;
using Terraria;

namespace TLoZ.Time.States
{
    public abstract class EntityState<T> : State where T : Entity
    {
        protected EntityState(T entity) : base(entity)
        {
            Entity = entity;

            Active = entity.active;

            Position = entity.position;
            Velocity = entity.velocity;
        }


        public override void Restore()
        {
            Entity.active = Active;

            Entity.position = Position;
            Entity.velocity = Velocity;
        }


        public T Entity { get; }

        public bool Active { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
    }
}