using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a suspended user entity with its details.
	/// </summary>
	public class SuspendedUser
	{
		/// <summary>
		/// Gets or sets the unique identifier of the suspended user.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the application.
		/// </summary>
		[Required]
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the identifier of the user who suspended the user.
		/// </summary>
		public int ApplicationUserId { get; set; } = 0;

		/// <summary>
		/// Gets or sets the identifier of the user who suspended the user.
		/// </summary>
		[Required]
		public string SuspenderId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a value indicating whether the suspension is active.
		/// </summary>
		public bool SuspensionActive { get; set; } = true;

		/// <summary>
		/// Gets or sets the date and time the user was suspended from.
		/// </summary>
		public DateTime SuspendedFrom { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Gets or sets the date and time the user was suspended until.
		/// </summary>
		public DateTime SuspendedUntil { get; set; } = DateTime.Now.AddDays(1);

		/// <summary>
		/// Gets or sets the reason the user was suspended.
		/// </summary>
		public SuspensionReason Reason { get; set; } = SuspensionReason.Other;

		/// <summary>
		/// Gets or sets the application in which the user was suspended.
		/// </summary>
		public Application? Application { get; set; }

		/// <summary>
		/// Gets or sets the user who is suspended
		/// </summary>
		public ApplicationUser? Suspended { get; set; }

		/// <summary>
		/// Gets or sets the user who suspended the user.
		/// </summary>
		public User? Suspender { get; set; }
	}
}