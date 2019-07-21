using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DepenDarcy.Core.Extensions
{
    public static class XmlDocumentExtensions
    {
        public static bool TryGetElementsByTagName(this XmlDocument xmlDocument, string name, out XmlNodeList xmlNodeList)
        {
            xmlNodeList = xmlDocument.GetElementsByTagName(name);
            if (xmlNodeList.Count == 0)
            {
                return false;
            }
            return true;
        }

    }
}
