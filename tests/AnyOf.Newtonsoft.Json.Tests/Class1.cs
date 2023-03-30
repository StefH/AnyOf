using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyOfTypes.Newtonsoft.Json
{
    partial class AccountNumberNewtonsoftJsonConverter : global::Newtonsoft.Json.JsonConverter
    {
        
        public override bool CanRead => true;

        
        public override bool CanWrite => true;

        
        public override bool CanConvert(global::System.Type type) => type == typeof(AccountNumber);

        
        public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, object? value, global::Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(((AccountNumber)value).Value);
            }
        }

        
        public override object? ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object? existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.TokenType == global::Newtonsoft.Json.JsonToken.StartObject)
            {
                object? value = null;
                bool valueRead = false;
                reader.Read();
                while (reader.TokenType != global::Newtonsoft.Json.JsonToken.EndObject)
                {
                    if (!valueRead && reader.TokenType == global::Newtonsoft.Json.JsonToken.PropertyName && ((string?)reader.Value) == "Value")
                    {
                        reader.Read();
                        if (reader.TokenType == global::Newtonsoft.Json.JsonToken.Null && objectType == typeof(global::System.Nullable<AccountNumber>))
                        {
                            value = null;
                        }
                        else
                        {
                            value = new AccountNumber(serializer.Deserialize<long>(reader));
                        }

                        valueRead = true;
                        reader.Read();
                    }
                    else
                    {
                        reader.Skip();
                        reader.Read();
                    }
                }

                return value;
            }

            if (reader.TokenType == global::Newtonsoft.Json.JsonToken.Null && objectType == typeof(global::System.Nullable<AccountNumber>))
            {
                return null;
            }
            else
            {
                return new AccountNumber(serializer.Deserialize<long>(reader));
            }
        }
    }
}