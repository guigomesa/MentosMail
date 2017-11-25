using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentosMail.AttributesHelper
{
    public class SenderFieldInMailAttribute : Attribute
    {
        public string FieldReplace { get; private set; }

        public SenderFieldInMailAttribute(string fieldReplace = "")
        {
            FieldReplace = fieldReplace;
        }
    }
}
