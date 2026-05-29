using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MasterFinder.Domain.Entities;
using MasterFinder.ValueObjects;

namespace MasterFinder.Infrastructure.EntityFramework.Configurations
{
    public class ExecutionConfiguration : IEntityTypeConfiguration<Execution>
    {
        public void Configure(EntityTypeBuilder<Execution> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.StartedAt)
                .IsRequired()
                .HasConversion(
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc));

            builder.Property(x => x.CompletedAt)
                .IsRequired(false)
                .HasConversion(
                    src => !src.HasValue ? src : src.Value.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src.Value, DateTimeKind.Utc),
                    dst => !dst.HasValue ? dst : dst.Value.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst.Value, DateTimeKind.Utc));

            builder.Property(x => x.CancelledAt)
                .IsRequired(false)
                .HasConversion(
                    src => !src.HasValue ? src : src.Value.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src.Value, DateTimeKind.Utc),
                    dst => !dst.HasValue ? dst : dst.Value.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst.Value, DateTimeKind.Utc));

            builder.Property(x => x.CancelReason)
                .IsRequired(false)
                .HasConversion(
                    reason => reason != null ? reason.Value : null,
                    str => str != null ? new CancelReason(str) : null);

            builder.HasOne(x => x.Order)
                .WithOne(x => x.Execution)
                .HasForeignKey<Execution>("OrderId");
                //.OnDelete(DeleteBehavior.Cascade);


            //builder.HasOne(x => x.Executor)
            //    .WithMany(x => x.Executions)
            //    .HasForeignKey("ExecutorId");
           
        }
    }
}
