// Track the validation status of individual brand form fields.
let brandNameOk = false;
let brandWebsiteOk = true;
let brandLogoOk = true;
let brandEmailOk = true;

// Overall form validation status.
let formOk = false;

// References to error message containers for each field.
const brandNameErr = document.getElementById("brand_err");
const brandWebsiteErr = document.getElementById("brand_url_err");
const brandLogoErr = document.getElementById("brand_logo_err");
const brandEmailErr = document.getElementById("brand_email_err");
const applicationIconErr = document.getElementById("brand_logo_err");

// Container for displaying the uploaded logo preview.
const iconPreview = document.getElementById("icon_preview");

/**
 * Helper function to validate a URL.
 * Returns true if the URL is properly formatted.
 * @param {string} url - The URL to validate.
 * @returns {boolean} - True if valid, false otherwise.
 */
const isValidUrl = (url) => {
	try {
		new URL(url);
		return true;
	} catch (e) {
		return false;
	}
};

/**
 * Re-evaluates overall form validity based on the validation status of all fields.
 * Enables or disables the submit button depending on the form status.
 */
const checkForm = () => {
	if (brandNameOk && brandWebsiteOk && brandLogoOk && brandEmailOk) {
		formOk = true;
	} else {
		formOk = false;
	}
	if (formOk) {
		document.getElementById("submit").removeAttribute("disabled");
	} else {
		document.getElementById("submit").setAttribute("disabled", "disabled");
	}
};

/**
 * Validates the brand name field.
 * Ensures the name meets length requirements and checks uniqueness via an API call.
 * Displays appropriate error messages and updates validation status.
 * @param {HTMLElement} element - The input field for the brand name.
 */
const checkBrand = async (element) => {
	const brandName = element.value;
	if (brandName.length < 2) {
		element.classList.remove("input-valid");
		element.classList.add("input-invalid");
		brandNameErr.innerHTML = await localize(
			"Brand name must be at least 2 characters long"
		);
		brandNameOk = false;
		checkForm();
		return;
	}
	if (brandName.length > 50) {
		element.classList.remove("input-valid");
		element.classList.add("input-invalid");
		brandNameErr.innerHTML = await localize(
			"The brand name must be shorter than 50 characters"
		);
		brandNameOk = false;
		checkForm();
		return;
	}
	// check if is brand name unique
	const result = await fetchData("/api/IsBrandNameUnique/" + brandName);
	if (result.success) {
		if (result.data.isUnique) {
			element.classList.remove("input-invalid");
			element.classList.add("input-valid");
			brandNameErr.innerHTML = "";
			brandNameOk = true;
			checkForm();
			return;
		} else {
			element.classList.remove("input-valid");
			element.classList.add("input-invalid");
			brandNameErr.innerHTML = await localize(
				"Brand name is already registered"
			);
			brandNameOk = false;
			checkForm();
			return;
		}
	} else {
		element.classList.remove("input-valid");
		element.classList.add("input-invalid");
		brandNameErr.innerHTML = await localize("Error checking brand name");
		brandNameOk = false;
		checkForm();
		return;
	}
};

/**
 * Validates the brand website URL field.
 * If provided, ensures the URL is properly formatted.
 * Displays error messages and updates validation status.
 * @param {HTMLElement} element - The input field for the brand website URL.
 */
const checkUrl = async (element) => {
	const brandUrl = element.value;
	if (brandUrl.length > 0) {
		if (!isValidUrl(brandUrl)) {
			console.log("Invalid URL");
			element.classList.remove("input-valid");
			element.classList.add("input-invalid");
			brandWebsiteErr.innerHTML = await localize("Invalid URL");
			brandWebsiteOk = false;
			checkForm();
			return;
		} else {
			element.classList.remove("input-invalid");
			element.classList.add("input-valid");
			brandWebsiteErr.innerHTML = "";
			brandWebsiteOk = true;
			checkForm();
			return;
		}
	} else {
		console.log("Empty URL");
		element.classList.remove("input-invalid");
		element.classList.add("input-valid");
		brandWebsiteErr.innerHTML = "";
		brandWebsiteOk = true;
		checkForm();
		return;
	}
};

/**
 * Validates the uploaded brand logo file.
 * Ensures file type is supported and verifies the file is a real image.
 * Displays a preview and updates validation status.
 * @param {HTMLElement} element - The file input field for the brand logo.
 */
const checkIcon = async (element) => {
	console.log("Checking the icon");
	const validTypes = ["image/jpeg", "image/png", "image/jpg", "image/gif"];
	const file = element.files[0];
	if (!file) {
		console.log("File not found");

		element.classList.add("input-valid");
		element.classList.remove("input-invalid");
		applicationIconOk = true;
		applicationIconErr.innerHTML = "";
		checkForm();
		return;
	}
	console.log(file);
	if (validTypes.includes(file.type)) {
		try {
			const isValidImage = await checkIfRealImage(file);
			console.log(isValidImage);
			if (!isValidImage) {
				element.classList.remove("input-valid");
				element.classList.add("input-invalid");
				applicationNameErr.innerHTML = await localize(
					"Error checking application name"
				);
				applicationNameOk = false;
				checkForm();
				message.textContent = "The file is not a valid image.";
			} else {
				showImagePreview(file); // Display the image preview
				element.classList.remove("input-invalid");
				element.classList.add("input-valid");
				applicationIconErr.innerHTML = "";
				applicationIconOk = true;
				checkForm();
			}
		} catch (error) {
			message.textContent = "An error occurred while validating the file.";
		}
	}

	/**
	* Determines whether the uploaded file is a valid image by loading it into an Image object.
	* @param {File} file - The file to check.
	* @returns {Promise<boolean>} - Resolves to true if valid, false otherwise.
	*/
	async function checkIfRealImage(file) {
		const reader = new FileReader();

		return new Promise((resolve, reject) => {
			reader.onload = (e) => {
				const img = new Image();
				img.onload = () => resolve(true); // Real image
				img.onerror = () => resolve(false); // Invalid image
				img.src = e.target.result; // Load the image
			};

			reader.onerror = () => reject("Error reading the file");
			reader.readAsDataURL(file); // Read file content as Data URL
		});
	}

	/**
	* Displays a 32x32 preview of the uploaded image in the icon preview container.
	* @param {File} file - The uploaded file to display.
	*/
	async function showImagePreview(file) {
		const reader = new FileReader();

		reader.onload = (e) => {
			const img = document.createElement("img");
			img.src = e.target.result; // Set the source to the file's data URL
			img.alt = "Image Preview";
			iconPreview.innerHTML = "";
			iconPreview.appendChild(img);
		};

		reader.readAsDataURL(file); // Read file content as Data URL
	}
};

/**
 * Validates the brand contact email address.
 * Checks the format against a regular expression.
 * Displays error messages and updates validation status.
 * @param {HTMLElement} element - The input field for the brand contact email.
 */
const checkMail = async (element) => {
	console.log("checkEmail");
	// validate format
	const email = element.value;
	let emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
	if (!emailRegex.test(email)) {
		element.classList.remove("input-valid");
		element.classList.add("input-invalid");
		brandEmailErr.innerHTML = await localize("Invalid email address");
		emailOk = false;
	} else {
		element.classList.remove("input-invalid");
		element.classList.add("input-valid");
		brandEmailErr.innerHTML = "";
		emailOk = true;
	}
	checkForm();
};