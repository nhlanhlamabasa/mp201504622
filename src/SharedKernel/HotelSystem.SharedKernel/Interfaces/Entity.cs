namespace HotelSystem.SharedKernel
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="Entity{EntityId}" />
    /// </summary>
    /// <typeparam name="EntityId"></typeparam>
    public abstract class Entity<EntityId> : IEquatable<Entity<EntityId>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity{EntityId}"/> class.
        /// </summary>
        protected Entity()
        {

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Entity{EntityId}"/> class.
        /// </summary>
        /// <param name="id">The id<see cref="EntityId"/></param>
        protected Entity(EntityId id)
        {
            if (Equals(id, default(EntityId)))
            {
                throw new ArgumentException("The ID cannot be the type's default value.", "id");
            }

            Id = id;
        }

        /// <summary>
        /// Gets or sets the CreationTime
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public EntityId Id { get; protected set; }

        /// <summary>
        /// Gets or sets the ModifiedDate
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// The Equals
        /// </summary>
        /// <param name="other">The other<see cref="Entity{EntityId}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool Equals(Entity<EntityId> other)
        {
            if (other == null)
            {
                return false;
            }
            return Id.Equals(other.Id);
        }

        /// <summary>
        /// The Equals
        /// </summary>
        /// <param name="otherObject">The otherObject<see cref="object"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public override bool Equals(object otherObject)
        {
            if (otherObject is Entity<EntityId> entity)
            {
                return Equals(entity);
            }
            return base.Equals(otherObject);
        }

        /// <summary>
        /// The GetHashCode
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
