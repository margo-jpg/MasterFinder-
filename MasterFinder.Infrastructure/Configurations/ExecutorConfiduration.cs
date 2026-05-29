using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MasterFinder.Domain.Entities;
using MasterFinder.ValueObjects;
using MasterFinder.ValueObjects.Validators;

namespace MasterFinder.Infrastructure.EntityFramework.Configurations
{
    public class ExecutorConfiguration : IEntityTypeConfiguration<Executor>
    {
        public void Configure(EntityTypeBuilder<Executor> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Username)
                .IsRequired()
                .HasConversion(
                    username => username.Value,
                    str => new Username(str))
                .HasMaxLength(UsernameValidator.MAX_LENGTH);

            builder.Property(x => x.Phone)
                .IsRequired()
                .HasConversion(
                    phone => phone.Value,
                    str => new PhoneNumber(str))
                .HasMaxLength(PhoneNumberValidator.MAX_LENGTH);

            builder.Property(x => x.Specialization)
                .IsRequired()
                .HasConversion(
                    spec => spec.Value,
                    str => new Specialization(str))
                .HasMaxLength(SpecializationValidator.MAX_LENGTH);

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasConversion(
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc));

            builder.HasMany(x => x.Responses)
                .WithOne(x => x.Executor)
                .HasForeignKey("ExecutorId");
            //.OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Executions)
                .WithOne(x => x.Executor)
                .HasForeignKey("ExecutorId");
                //.OnDelete(DeleteBehavior.Cascade);

            //builder.Ignore(x => x.Responses);
            //builder.Ignore(x => x.Executions);
        }
    }
}
