using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CAP.API.Models
{
    public class TimeStamp : ITimestampedEntity
    {
        public DateTime CreatedTimestamp { get; set; }
        public DateTime UpdatedTimestamp { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }

    public static class TimestampExtensions
    {
        public static EntityTypeBuilder<TEntity> UseTimestampedProperty<TEntity>(this EntityTypeBuilder<TEntity> entity) where TEntity : class, ITimestampedEntity
        {
            entity.Property(d => d.CreatedTimestamp).ValueGeneratedOnAdd();
            entity.Property(d => d.UpdatedTimestamp).ValueGeneratedOnAddOrUpdate();

            entity.Property(d => d.CreatedTimestamp).Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            entity.Property(d => d.CreatedTimestamp).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            entity.Property(d => d.UpdatedTimestamp).Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            entity.Property(d => d.UpdatedTimestamp).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            return entity;
        }
    }
    public interface ITimestampedEntity
    {
        DateTime CreatedTimestamp { get; set; }
        DateTime UpdatedTimestamp { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}