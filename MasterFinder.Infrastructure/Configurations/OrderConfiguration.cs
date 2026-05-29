using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Enums;
using MasterFinder.ValueObjects;
using MasterFinder.ValueObjects.Validators;

namespace MasterFinder.Infrastructure.EntityFramework.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasConversion(
                    title => title.Value,
                    str => new OrderTitle(str))
                .HasMaxLength(OrderTitleValidator.MAX_LENGTH);

            builder.Property(x => x.Description)
                .IsRequired(false)
                .HasConversion(
                    desc => desc.Value,
                    str => new OrderDescription(str));

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion(
                    status => status.ToString(),
                    str => (OrderStatus)Enum.Parse(typeof(OrderStatus), str));

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasConversion(
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc));

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey("CustomerId");
            //.HasPrincipalKey(x => x.Id);

            builder.HasOne(x => x.Execution)
                .WithOne(x => x.Order)
                .HasForeignKey<Execution>("OrderId");
            //.HasPrincipalKey<Order>(x => x.Id);

            builder.HasMany(x => x.Responses)
                .WithOne(x => x.Order)
                .HasForeignKey("OrderId");
                //.HasPrincipalKey(x => x.Id);

            //builder.Ignore(x => x.Responses);
            //builder.Ignore(x => x.Execution);
        }
    }
}
