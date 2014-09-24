using Orchard.Data.Migration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using dsc.CalendarWidget.Models;

namespace dsc.CalendarWidget
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition("TimeSpanPart",
                builder => builder
                    .Attachable()
                    .WithField("StartDateTime",
                        field => field
                            .OfType("DateTimeField")
                            .WithDisplayName("Start DateTime")
                    )
                    .WithField("EndDateTime",
                        field => field
                            .OfType("DateTimeField")
                            .WithDisplayName("End DateTime")
                    )
                    .WithField("AllDay",
                        field=>field
                                .OfType("BooleanField")
                                .WithDisplayName("AllDay")
                    )
            );

            SchemaBuilder.CreateTable("CalendarWidgetPartRecord", table => table
                .ContentPartRecord()
                .Column<int>("QueryId")
            );

            ContentDefinitionManager.AlterPartDefinition(typeof(CalendarWidgetPart).Name,
                builder => builder.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("CalendarWidget",
                cfg => cfg
                    .WithPart("CalendarWidgetPart")
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                    );

            return 2;
        }

        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterPartDefinition("TimeSpanPart",
                builder => builder
                    .WithField("AllDay",
                        field => field
                            .OfType("BooleanField")
                            .WithDisplayName("AllDay")
                    )
                );
            return 2;
        }
    }
}