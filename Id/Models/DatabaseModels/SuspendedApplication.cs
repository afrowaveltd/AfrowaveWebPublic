using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a suspended application entity with its details.
	/// </summary>
	public class SuspendedApplication
	{
		/// <summary>
		/// Gets or sets the unique identifier of the suspended application.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the application.
		/// </summary>
		[Required]
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the identifier of the user who suspended the application.
		/// </summary>
		[Required]
		public string SuspenderId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a value indicating whether the suspension is active.
		/// </summary>
		public bool SuspensionActive { get; set; } = true;

		/// <summary>
		/// Gets or sets the date and time the application was suspended from.
		/// </summary>
		public DateTime SuspendedFrom { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Gets or sets the date and time the application was suspended until.
		/// </summary>
		public DateTime SuspendedUntil { get; set; } = DateTime.Now.AddDays(7);

		/// <summary>
		/// Gets or sets the reason the application was suspended.
		/// </summary>
		public SuspensionReason Reason { get; set; } = SuspensionReason.Other;

		/// <summary>
		/// Gets or sets the application that was suspended.
		/// </summary>
		public Application? Application { get; set; }

		/// <summary>
		/// Gets or sets the user who suspended the application.
		/// </summary>
		public User? Suspender { get; set; }
	}

	/// <summary>
	/// Represents the reason an application was suspended.
	/// </summary>
	public enum SuspensionReason
	{
		/// <summary>
		/// The application was suspended for breaching the community rules.
		/// </summary>
		CommunityRulesBreach,

		/// <summary>
		/// Suspended for breaching the terms of service considering the hate speech.
		/// </summary>
		HatredPromotion,

		/// <summary>
		/// Suspended for breaching the community rules about the abusive behavior.
		/// </summary>
		AbusiveBehavior,

		/// <summary>
		/// Suspended for breaching the community rules about the illegal advertisement.
		/// </summary>
		IllegalAdvertisement,

		/// <summary>
		/// Suspended for breaching the community rules about the violence promotion.
		/// </summary>
		ViolencePromotion,

		/// <summary>
		/// Suspended for breaching the community rules about the illegal content.
		/// </summary>
		IllegalContent,

		/// <summary>
		/// Suspended for breaching the community rules about the piracy.
		/// </summary>
		Piracy,

		/// <summary>
		/// Suspended for breaching the community rules about the children
		/// </summary>
		ChildrenAbuse,

		/// <summary>
		/// Another reason for the suspension.
		/// </summary>
		Other
	}
}