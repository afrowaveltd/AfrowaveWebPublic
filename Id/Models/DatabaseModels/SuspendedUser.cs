using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
	public class SuspendedUser
	{
		public int Id { get; set; }

		[Required]
		public string ApplicationId { get; set; } = string.Empty;

		public int ApplicationUserId { get; set; } = 0;

		[Required]
		public string SuspenderId { get; set; } = string.Empty;

		public bool SuspensionActive { get; set; } = true;

		public DateTime SuspendedFrom { get; set; } = DateTime.UtcNow;
		public DateTime SuspendedUntil { get; set; } = DateTime.Now.AddDays(1);
		public SuspensionReason Reason { get; set; } = SuspensionReason.Other;
		public Application? Application { get; set; }
		public ApplicationUser? Suspended { get; set; }
		public User? Suspender { get; set; }
	}
}