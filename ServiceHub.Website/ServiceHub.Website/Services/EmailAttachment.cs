using System.Runtime.Serialization;

namespace ServiceHub.Website
{
	[DataContract]
	public class EmailAttachment
	{
		[DataMember]
		public byte[] Buffer { get; set; }
		[DataMember]
		public string FileName { get; set; }
		[DataMember]
		public string OriginalFilename { get; set; }
	}
}
