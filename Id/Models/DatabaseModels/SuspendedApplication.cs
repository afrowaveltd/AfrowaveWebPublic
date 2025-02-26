using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
	public class SuspendedApplication
	{
		public int Id { get; set; }

		[Required]
		public string ApplicationId { get; set; } = string.Empty;

		[Required]
		public string SuspenderId { get; set; } = string.Empty;

		public bool SuspensionActive { get; set; } = true;

		public DateTime SuspendedFrom { get; set; } = DateTime.UtcNow;
		public DateTime SuspendedUntil { get; set; } = DateTime.Now.AddDays(7);
		public SuspensionReason Reason { get; set; } = SuspensionReason.Other;
		public Application? Application { get; set; }
		public User? Suspender { get; set; }
	}

	public enum SuspensionReason
	{
		CommunityRulesBreach,
		HatredPromotion,
		AbusiveBehavior,
		IllegalAdvertisement,
		ViolencePromotion,
		IllegalContent,
		Piracy,
		ChildrenAbuse,
		Other
	}
}