namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a theme definition that specifies various visual styling properties for a user interface.
	/// </summary>
	/// <remarks>This class provides a collection of properties that define the appearance of UI elements, such as
	/// colors, fonts, and backgrounds. It can be used to apply consistent theming across an application.</remarks>
	public class ThemeDefinition
	{
		/// <summary>
		/// Gets or sets the unique identifier for the entity.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier for the theme.
		/// </summary>
		public int ThemeId { get; set; }

		/// <summary>
		/// Gets or sets the font link associated with the current object.
		/// </summary>
		public string? FontLink { get; set; }

		/// <summary>
		/// Gets or sets the background color represented as a hexadecimal color code.
		/// </summary>
		public string Background { get; set; } = "#977e51";

		/// <summary>
		/// Gets or sets the foreground color represented as a hexadecimal color code.
		/// </summary>
		public string Foreground { get; set; } = "#0a0a0a";

		/// <summary>
		/// Gets or sets the foreground color of the form as a hexadecimal color code.
		/// </summary>
		public string FormForeground { get; set; } = "#e2e2e2";

		/// <summary>
		/// Gets or sets the background color of the body as a hexadecimal color code.
		/// </summary>
		public string BodyBackground { get; set; } = "#e0d4c2";

		/// <summary>
		/// Gets or sets the hyperlink or reference associated with the object.
		/// </summary>
		public string Link { get; set; } = "#44371C";

		/// <summary>
		/// Gets or sets the color used for link hover effects.
		/// </summary>
		public string LinkHover { get; set; } = "firebrick";

		/// <summary>
		/// Gets or sets the color code representing the active link state.
		/// </summary>
		public string LinkActive { get; set; } = "#ff0808";

		/// <summary>
		/// Gets or sets the background color of the form as a hexadecimal color code.
		/// </summary>
		public string FormBackground { get; set; } = "#6e3c0d";

		/// <summary>
		/// Gets or sets the color value used for the red border.
		/// </summary>
		public string RedBorder { get; set; } = "firebrick";

		/// <summary>
		/// Gets or sets the color value used for the green border.
		/// </summary>
		public string DisabledBackground { get; set; } = "#44371c";

		/// <summary>
		/// Gets or sets the foreground color used to represent disabled elements.
		/// </summary>
		public string DisabledForeground { get; set; } = "gray";

		/// <summary>
		/// Gets or sets the background color used to indicate a valid state for a form control.
		/// </summary>
		public string FormControlValidBackground { get; set; } = "#6e3c0d";

		/// <summary>
		/// Gets or sets the background color used to indicate an invalid form control.
		/// </summary>
		public string FormControlInvalidBackground { get; set; } = "firebrick";

		/// <summary>
		/// Gets or sets the color used for the navigation bar.
		/// </summary>
		public string Navbar { get; set; } = "#c0b4a2";

		/// <summary>
		/// Gets or sets the color value for the "Hr" property.
		/// </summary>
		public string Hr { get; set; } = "firebrick";

		/// <summary>
		/// Gets or sets the color value representing a successful state.
		/// </summary>
		public string Success { get; set; } = "rgb(50, 200, 50)";

		/// <summary>
		/// Gets or sets the error color code in hexadecimal format.
		/// </summary>
		public string Error { get; set; } = "#aa0000";

		/// <summary>
		/// Gets or sets the warning color code in hexadecimal format.
		/// </summary>
		public string Warning { get; set; } = "#aa5500";

		/// <summary>
		/// Gets or sets the informational string associated with the object.
		/// </summary>
		public string Info { get; set; } = "#111190";

		/// <summary>
		/// Gets or sets the color used for highlighting elements.
		/// </summary>
		public string Highlight { get; set; } = "#000000";

		/// <summary>
		/// Gets or sets the value of the "My" property.
		/// </summary>
		public string My { get; set; } = "green";

		/// <summary>
		/// Gets or sets the family identifier or name associated with the object.
		/// </summary>
		public string Family { get; set; } = "#ff6600";

		/// <summary>
		/// Gets or sets the color code representing the admin role.
		/// </summary>
		public string Admin { get; set; } = "#bb0000";

		/// <summary>
		/// Gets or sets the color value for the light border as a hexadecimal color code.
		/// </summary>
		public string BorderLight { get; set; } = "#333333";

		/// <summary>
		/// Gets or sets the shadow color and opacity in RGBA format.
		/// </summary>
		public string Shadow { get; set; } = "#rgba(150,150,150,0.5)";

		/// <summary>
		/// Gets or sets the background color of the modal in RGBA format.
		/// </summary>
		public string ModalBackground { get; set; } = "rgba(0.2,0.2,0.2,0.8)";

		/// <summary>
		/// Gets or sets the font family used for rendering text.
		/// </summary>
		public string Font { get; set; } = "'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif";

		/// <summary>
		/// Gets or sets the current theme applied to the application.
		/// </summary>
		public Theme Theme { get; set; } = null!;
	}
}