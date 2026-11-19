using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Enums;
using MasterFinder.ValueObjects;

namespace MasterFinder.Infrastructure.EntityFramework.Configurations
{
    public class ResponseConfiguration : IEntityTypeConfiguration<Response>
    {
        public void Configure(EntityTypeBuilder<Response> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Comment)
                .IsRequired(false)
                .HasConversion(
                    comment => comment != null ? comment.Value : null,
                    str => str != null ? new ResponseComment(str) : null);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion(
                    status => status.ToString(),
                    str => (ResponseStatus)Enum.Parse(typeof(ResponseStatus), str));

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasConversion(
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc));

            builder.HasOne(x => x.Order)
                .WithMany(x => x.Responses)
                .HasForeignKey("OrderId");


            builder.HasOne(x => x.Executor)
                .WithMany(x => x.Responses)
                .HasForeignKey("ExecuterId");

        }
    }
}
