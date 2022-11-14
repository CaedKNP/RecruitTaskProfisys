using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitTaskProfisys
{
    class DocumentItem
    {
        //DocumentId;Ordinal;Product;Quantity;Price;TaxRate
        public int documnetId { get; set;}
        public int ordinal { get; set;}
        public string product { get; set;}
        public int quantity { get; set; }
        public float price { get; set; }
        public int taxRate { get; set; }

        public DocumentItem(int _dId, int _ord, string _prdct, int _qnt, float _prc, int _tRt)
        {
            documnetId = _dId;
            ordinal = _ord;
            product = _prdct;
            quantity = _qnt;
            price = _prc;
            taxRate = _tRt;
        }
    }
}
