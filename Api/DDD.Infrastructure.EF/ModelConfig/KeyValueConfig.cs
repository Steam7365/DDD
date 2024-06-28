using DDD.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructure.EF.ModelConfig
{
    public class KeyValueConfig : IEntityTypeConfiguration<KeyValue>
    {
        public void Configure(EntityTypeBuilder<KeyValue> builder)
        {
            builder.HasKey(x => x.Key);
            builder.Property(x => x.Key).HasMaxLength(20);
            builder.Property(x => x.Value).HasColumnType("nvarchar(max)");
        }
    }
}
