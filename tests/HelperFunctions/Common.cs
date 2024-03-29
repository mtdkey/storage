﻿using MtdKey.Storage.Tests.TestFiles;
using System;
using System.Threading.Tasks;

namespace MtdKey.Storage.Tests.HelperFunctions
{
    public static class Common
    {
        public static DateTime DateTimeValue => new(1976, 4, 12, 21, 0, 0);
        public static decimal NumericValue => 3636.36M;
        public static string LongTextValue => FileReader.GetLongText();
        public static string ShortTextValue => FileReader.GetShortText();

        public static string OldXmlSchema => FileReader.GetOldSchema();
        public static string NewXmlSchema => FileReader.GetNewSchema();

        public static bool BooleanValue => true;
        public static string SplitedWordValue => "Inbunchation";


        public static string GetRandomName()
        {
            Random random = new();
            string result = random.Next().ToString();
            return result;
        }

        public static async Task<FileData> GetFileTestAsync()
        {
            var byteArray = await FileReader.GetFileBytesAsync();
            return new FileData
            {
                Mime = "text/plain",
                Name = "LongText.txt",
                Size = byteArray.Length,
                ByteArray = byteArray
            };
        }

    }
}
