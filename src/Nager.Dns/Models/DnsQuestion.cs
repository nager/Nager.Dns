using System.Diagnostics.CodeAnalysis;

namespace Nager.Dns.Models
{
    /// <summary>
    /// Dns Question
    /// </summary>
    public class DnsQuestion
    {
        /// <summary>
        /// Name
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public DnsAnswerType Type { get; set; }

        /// <summary>
        /// Dns Question
        /// </summary>
        public DnsQuestion() { }

        /// <summary>
        /// Dns Question
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        [SetsRequiredMembers]
        public DnsQuestion(string name, DnsAnswerType type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
