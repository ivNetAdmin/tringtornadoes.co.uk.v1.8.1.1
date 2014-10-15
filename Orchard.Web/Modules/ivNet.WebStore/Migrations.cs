using System;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Common.Fields;
using Orchard.Core.Common.Models;
using Orchard.Core.Containers.Models;
using Orchard.Core.Contents.Extensions;
using Orchard.Core.Navigation.Models;
using Orchard.Data.Migration;
using Orchard.Users.Models;
using ivNet.Webstore.Models;
using Orchard.Widgets.Models;

namespace ivNet.Webstore {
    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {
            SchemaBuilder.CreateTable("ProductRecord", table => table
                .ContentPartRecord()
                .Column<decimal>("Price")
                .Column<string>("Sku", column => column.WithLength(50))
                );

            return 1;
        }

        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterPartDefinition(typeof (ProductPart).Name, part => part
                .Attachable()
                );

            return 2;
        }

        public int UpdateFrom2()
        {
            ContentDefinitionManager.AlterTypeDefinition("ShoppingCartWidget", type => type
                .WithPart(typeof (ShoppingCartWidgetPart).Name)
                .WithPart(typeof (CommonPart).Name)
                .WithPart(typeof (WidgetPart).Name)
                .WithSetting("Stereotype", "Widget")
                );

            return 3;
        }

        public int UpdateFrom3()
        {
            ContentDefinitionManager.AlterTypeDefinition("ShoppingCartWidget", type => type
                .WithPart(typeof (CommonPart).Name)
                );

            return 4;
        }

        public int UpdateFrom4()
        {
            SchemaBuilder.CreateTable("CustomerRecord", t => t
                .ContentPartRecord()
                .Column<string>("FirstName", c => c.WithLength(50))
                .Column<string>("LastName", c => c.WithLength(50))
                .Column<string>("Title", c => c.WithLength(10))
                .Column<DateTime>("CreatedAt", c => c.NotNull())
                );

            SchemaBuilder.CreateTable("AddressRecord", t => t
                .ContentPartRecord()
                .Column<int>("CustomerId")
                .Column<string>("Type", c => c.WithLength(50))
                );

            SchemaBuilder.CreateForeignKey("Address_Customer", "AddressRecord", new[] {"CustomerId"}, "CustomerRecord",
                new[] {"Id"});

            ContentDefinitionManager.AlterPartDefinition(typeof (CustomerPart).Name, p => p
                .Attachable(false)
                .WithField("Phone", f => f.OfType(typeof (TextField).Name))
                );

            ContentDefinitionManager.AlterTypeDefinition("Customer", t => t
                .WithPart(typeof (CustomerPart).Name)
                .WithPart(typeof (UserPart).Name)
                );

            ContentDefinitionManager.AlterPartDefinition(typeof (AddressPart).Name, p => p
                .Attachable(false)
                .WithField("Name", f => f.OfType(typeof (TextField).Name))
                .WithField("AddressLine1", f => f.OfType(typeof (TextField).Name))
                .WithField("AddressLine2", f => f.OfType(typeof (TextField).Name))
                .WithField("Zipcode", f => f.OfType(typeof (TextField).Name))
                .WithField("City", f => f.OfType(typeof (TextField).Name))
                .WithField("Country", f => f.OfType(typeof (TextField).Name))
                );

            ContentDefinitionManager.AlterTypeDefinition("Address", t => t
                .WithPart(typeof (AddressPart).Name)
                );

            return 5;
        }

        public int UpdateFrom5()
        {
            SchemaBuilder.CreateTable("OrderRecord", t => t
                .Column<int>("Id", c => c.PrimaryKey().Identity())
                .Column<int>("CustomerId", c => c.NotNull())
                .Column<DateTime>("CreatedAt", c => c.NotNull())
                .Column<decimal>("Total", c => c.NotNull())
                .Column<decimal>("SubTotal", c => c.NotNull())
                .Column<decimal>("Vat", c => c.NotNull())
                .Column<string>("Number", c => c.WithLength(255))
                .Column<string>("Status", c => c.WithLength(50).NotNull())
                .Column<string>("PaymentServiceProviderResponse", c => c.Unlimited())
                .Column<string>("PaymentReference", c => c.WithLength(50))
                .Column<DateTime>("PaidAt", c => c.Nullable())
                .Column<DateTime>("CompletedAt", c => c.Nullable())
                .Column<DateTime>("CancelledAt", c => c.Nullable())
                );

            SchemaBuilder.CreateTable("OrderDetailRecord", t => t
                .Column<int>("Id", c => c.PrimaryKey().Identity())
                .Column<int>("OrderRecord_Id", c => c.NotNull())
                .Column<int>("ProductId", c => c.NotNull())
                .Column<int>("Quantity", c => c.NotNull())
                .Column<string>("Size", c => c.WithLength(50))
                .Column<decimal>("UnitPrice", c => c.NotNull())
                .Column<decimal>("VatRate", c => c.NotNull())
                .Column<decimal>("Vat", c => c.NotNull())
                .Column<decimal>("UnitVat", c => c.NotNull())
                 .Column<decimal>("Total", c => c.NotNull())
                .Column<decimal>("SubTotal", c => c.NotNull())
                );

            SchemaBuilder.CreateForeignKey("Order_Customer", "OrderRecord", new[] {"CustomerId"}, "CustomerRecord",
                new[] {"Id"});
            SchemaBuilder.CreateForeignKey("OrderDetail_Order", "OrderDetailRecord", new[] {"OrderRecord_Id"},
                "OrderRecord", new[] {"Id"});
            SchemaBuilder.CreateForeignKey("OrderDetail_Product", "OrderDetailRecord", new[] {"ProductId"},
                "ProductRecord", new[] {"Id"});

            return 6;
        }

        public int UpdateFrom6()
        {
            SchemaBuilder.AlterTable("ivNet_WebStore_OrderRecord", table => table
                .AlterColumn("PaymentServiceProviderResponse", column => column.Unlimited()));
            return 7;
        }
    }
}