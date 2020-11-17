

using System;
using System.Collections;
using System.Collections.Generic;

namespace CSVGenerator.generator
{
    public class CSVRow
    {
        protected Dictionary<string, object> values;
        public event EventHandler<FieldSetEventArgs> FieldSet;

        public CSVRow()
        {
            values = new Dictionary<string, object>();
        }

        public string getValues(ArrayList fields)
        {
            string output = "";
            for (int i = 0; i < fields.Count; i++)
            {
                string field = (string)fields[i];
                string value = getFieldvalue(field);
                if (i < fields.Count - 1)
                {
                    output += value + ",";
                }
                else
                {
                    output += value + Environment.NewLine;
                }
            }
            return output;
        }

        protected string getFieldvalue(string field)
        {
            if (values.ContainsKey(field))
            {
                object value = values[field];
                if (value.GetType() == typeof(string))
                {
                    string fieldvalue = (string)value;
                    fieldvalue = "\"" + fieldvalue + "\"";
                    return fieldvalue;
                }
                else if (IsNumericType(value))
                {
                    return Convert.ToString(value);
                }
                else if (value.GetType() == typeof(ArrayList))
                {
                    ArrayList values = (ArrayList)value;
                    return "\"" + string.Join(",", (string[])values.ToArray(Type.GetType("System.String"))) + "\"";
                }
                else
                {
                    return "";
                }
            }
            return "";
        }

        public void Add(string field, object value)
        {
            if (value == null)
            {
                return;
            }
            values.Add(field, value);
            FieldSetEventArgs target = new FieldSetEventArgs();
            target.field = field;
            OnFieldSet(target);
        }

        protected virtual void OnFieldSet(FieldSetEventArgs e)
        {
            EventHandler<FieldSetEventArgs> handler = FieldSet;
            if (handler != null)
            {
                handler(this, e);
            }
        }        

        public static bool IsNumericType(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
