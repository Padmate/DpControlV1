﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using DpControl.Domain.Entities;

namespace DpControl.Domain.EFContext.Configurations
{
    public class SceneConfiguration
    {
        public SceneConfiguration(EntityTypeBuilder<Scene> entityBuilder)
        {
            entityBuilder.ToTable("Scenes", "ControlSystem");
            entityBuilder.HasKey(s => s.SceneId);
            entityBuilder.Property(s => s.Name).IsRequired().HasMaxLength(50);

            entityBuilder.Property(s => s.ModifiedDate).IsRequired();
            entityBuilder.Property(s => s.RowVersion).IsConcurrencyToken();

            entityBuilder.HasMany(s => s.LocationGroups).WithOne(g => g.Scene);
            entityBuilder.HasMany(s => s.SceneSegments).WithOne(s => s.Scene).HasForeignKey(s => s.SenseId);
        }
    }
}