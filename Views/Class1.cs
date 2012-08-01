﻿// ref : RaptorDb.Common.dll
//
// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaptorDB;


namespace SampleViews
{
    #region [  class definitions  ]
    public class LineItem
    {
        public decimal QTY { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }

    public class SalesInvoice
    {
        public SalesInvoice()
        {
            ID = Guid.NewGuid();
        }
        public Guid ID { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public List<LineItem> Items { get; set; }
        public DateTime Date { get; set; }
        public int Serial { get; set; }
        public byte Status { get; set; }
    }
    #endregion

    #region [  views  ]
    [RegisterView]
    public class SalesInvoiceView : View<SalesInvoice>
    {
        public class RowSchema : RDBSchema
        {
            [FullText]
            public string CustomerName;
            public DateTime Date;
            public string Address;
            public int Serial;
            public byte Status;
        }

        public SalesInvoiceView()
        {
            this.Name = "SalesInvoice";
            this.Description = "A primary view for SalesInvoices";
            this.isPrimaryList = true;
            this.isActive = true;
            this.BackgroundIndexing = true;
            
            //// uncomment the following for transaction mode
            //this.TransactionMode = true;

            this.Schema = typeof(SalesInvoiceView.RowSchema);

            this.AddFireOnTypes(typeof(SalesInvoice));

            this.Mapper = (api, docid, doc) =>
            {
                if (doc.Serial == 0)
                    api.RollBack();
                api.EmitObject(docid, doc);
            };
        }
    }

    [RegisterView]
    public class SalesItemRowsView : View<SalesInvoice>
    {
        public class RowSchema : RDBSchema
        {
            public string Product;
            public decimal QTY;
            public decimal Price;
            public decimal Discount;
        }

        public SalesItemRowsView()
        {
            this.Name = "SalesItemRows";
            this.Description = "";
            this.isPrimaryList = false;
            this.isActive = true;
            this.BackgroundIndexing = true;
            
            this.Schema = typeof(SalesItemRowsView.RowSchema);

            this.AddFireOnTypes(typeof(SalesInvoice));

            this.Mapper = (api, docid, doc) =>
            {
                if (doc.Status == 3 && doc.Items != null)
                    foreach (var item in doc.Items)
                        api.EmitObject(docid, item);
            };
        }
    }

    [RegisterView]
    public class newview : View<SalesInvoice>
    {
        public class RowSchema : RDBSchema
        {
            public string Product;
            public decimal QTY;
            public decimal Price;
            public decimal Discount;
        }

        public newview()
        {
            this.Name = "newview";
            this.Description = "";
            this.isPrimaryList = false;
            this.isActive = true;
            this.BackgroundIndexing = true;
            this.Version = 1;

            this.Schema = typeof(newview.RowSchema);

            this.AddFireOnTypes(typeof(SalesInvoice));

            this.Mapper = (api, docid, doc) =>
            {
                if (doc.Status == 3 && doc.Items != null)
                    foreach (var i in doc.Items)
                        api.EmitObject(docid, i);
            };
        }
    }
    #endregion
}