using System;

namespace MentosMailCore.AttributesHelper
{
    public class SenderFieldInMailAttribute : Attribute
    {
        public readonly string FieldReplace;

        public SenderFieldInMailAttribute(string fieldReplace = "")
        {
            FieldReplace = fieldReplace;
        }
    }
}