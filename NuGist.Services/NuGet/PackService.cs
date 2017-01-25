using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace NuGist.Services.NuGet
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        public Utf8StringWriter(StringBuilder sb)
            : base(sb)
        {
        }
    }

    public static class PackService
    {
        public static string CreateNuspec()
        {
            var package = new package();
            var s = new XmlSerializer(typeof(package));

            var sb = new StringBuilder();
            var ms = new Utf8StringWriter(sb);

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            s.Serialize(ms, package, ns);

            var result = Regex.Replace(sb.ToString(), @"(xmlns:?[^=]*=[""][^""]*[""])", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return Regex.Replace(result, @"(d2p1:?[^=]*=[""][^""]*[""])", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }
    }
}
