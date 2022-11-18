﻿using SFML.Graphics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom.Json
{
    public class ColorJsonConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string stringValue = reader.GetString()!;
            if (stringValue[0] != '#' || (stringValue.Length != 7 && stringValue.Length != 9))
            {
                throw new JsonException($"Improperly formatted color '{stringValue}'.");
            }
            byte r = byte.Parse(stringValue.Substring(1, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(stringValue.Substring(3, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(stringValue.Substring(5, 2), NumberStyles.HexNumber);
            byte a = stringValue.Length > 7
                ? byte.Parse(stringValue.Substring(7, 2), NumberStyles.HexNumber)
                : (byte)255;
            return new Color(r, g, b, a);
        }

        public override void Write(Utf8JsonWriter writer, Color @object, JsonSerializerOptions options)
        {
            writer.WriteStringValue(
                string.Format("#{0:x}{1:x}{2:x}{3:x}", @object.R, @object.G, @object.B, @object.A));
        }
    }
}