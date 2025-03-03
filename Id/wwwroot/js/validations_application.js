let applicationNameOk = false;
let applicationIconOk = true;
let applicationDescriptionOk = true;
let applicationEmailOk = true;
let applicationWebsiteOk = true;

let formOk = false;

const applicationNameErr = document.getElementById('application_name_err');
const applicationIconErr = document.getElementById('application_icon_err');
const iconPreview = document.getElementById('icon_preview');
const applicationEmailErr = document.getElementById('application_email_err');
const applicationWebsiteErr = document.getElementById('application_website_err');

/**
 * Helper function to check if a given URL is valid.
 * Returns true if the URL is correctly formed, false otherwise.
 * @param {string} url - The URL string to validate.
 * @returns {boolean} - True if valid, false if invalid.
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
 * Re-evaluates the overall form validity based on individual field statuses.
 * Enables or disables the form submit button accordingly.
 */
const checkForm = () => {
	if (applicationNameOk
		&& applicationIconOk
		&& applicationDescriptionOk
		&& applicationEmailOk
		&& applicationWebsiteOk) {
		formOk = true;
	} else {
		formOk = false;
	}

	if (formOk) {
		document.getElementById('submit').removeAttribute('disabled');
	}

	else {
		document.getElementById('submit').setAttribute('disabled', 'disabled');
	}
}

/**
 * Validates the application name field.
 * Ensures length constraints are met and checks if the name is unique via an API call.
 * Displays appropriate error messages and updates the validation status.
 * @param {HTMLElement} element - The input element for the application name.
 */
const checkName = async (element) => {
	const applicationName = element.value;
	if (applicationName.length < 3) {
		element.classList.remove('input-valid');
		element.classList.add('input-invalid');
		applicationNameErr.innerHTML = await localize("Application name must be at least 3 characters long");
		applicationNameOk = false;
		checkForm();
		return;
	}
	if (applicationName.length > 32) {
		element.classList.remove('input-valid');
		element.classList.add('input-invalid');
		applicationNameErr.innerHTML = await localize("The application name must be shorter than 32 characters");
		applicationNameOk = false;
		checkForm();
		return;
	}

	// check if is application name unique
	const result = await fetchData('/api/IsApplicationNameUnique/' + applicationName);
	if (result.success) {
		if (result.data.isUnique) {
			element.classList.remove('input-invalid');
			element.classList.add('input-valid');
			applicationNameErr.innerHTML = "";
			applicationNameOk = true;
			checkForm();
			return;
		}
		else {
			element.classList.remove('input-valid');
			element.classList.add('input-invalid');
			applicationNameErr.innerHTML = await localize("Application name is already registered");
			applicationNameOk = false;
			checkForm();
			return;
		}
	}
	else {
		element.classList.remove('input-valid');
		element.classList.add('input-invalid');
		applicationNameErr.innerHTML = await localize("Error checking application name");
		applicationNameOk = false;
		checkForm();
	}
}

/**
 * Validates the uploaded application icon file.
 * Ensures the file is of a supported image type and is a real image.
 * Displays an image preview and updates the validation status.
 * @param {HTMLElement} element - The file input element for the application icon.
 */
const checkIcon = async (element) => {
	const validTypes = ['image/jpeg', 'image/png', 'image/jpg', 'image/gif'];
	const file = element.files[0];
	if (!file) {
		console.log("File not found");

		element.classList.add('input-valid');
		element.classList.remove('input-invalid');
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
				element.classList.remove('input-valid');
				element.classList.add('input-invalid');
				applicationNameErr.innerHTML = await localize("Error checking application name");
				applicationNameOk = false;
				checkForm();
				message.textContent = "The file is not a valid image.";
			} else {
				showImagePreview(file); // Display the image preview
				element.classList.remove('input-invalid');
				element.classList.add('input-valid');
				applicationIconErr.innerHTML = "";
				applicationIconOk = true;
				checkForm();
			}
		} catch (error) {
			message.textContent = "An error occurred while validating the file.";
		}
	};

	/**
	* Check if a file is a real image
	* @param {File} file
	* @returns {Promise<boolean>}
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
	 * Show a 32x32px preview of the uploaded image
	 * @param {File} file
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
}

/**
 * Validates the application email field.
 * Ensures the email address format is correct.
 * Displays an error message if invalid and updates the validation status.
 * @param {HTMLElement} element - The input element for the application email.
 */
const checkEmail = async (element) => {
	const email = element.value;
	let emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
	if (!emailRegex.test(email)) {
		element.classList.remove("input-valid");
		element.classList.add("input-invalid");
		applicationEmailErr.innerHTML = await localize("Invalid email address");
		applicationEmailOk = false;
	} else {
		element.classList.remove("input-invalid");
		element.classList.add("input-valid");
		applicationEmailErr.innerHTML = "";
		applicationEmailOk = true;
	}
	checkForm();
}

/**
 * Validates the application website URL field.
 * Ensures the URL is correctly formatted (if provided).
 * Displays an error message if invalid and updates the validation status.
 * @param {HTMLElement} element - The input element for the application website URL.
 */
const checkWebsite = async (element) => {
	const brandUrl = element.value;
	if (brandUrl.length > 0) {
		if (!isValidUrl(brandUrl)) {
			element.classList.remove("input-valid");
			element.classList.add("input-invalid");
			applicationWebsiteErr.innerHTML = await localize("Invalid URL");
			applicationWebsiteOk = false;
			checkForm();
			return;
		} else {
			element.classList.remove("input-invalid");
			element.classList.add("input-valid");
			applicationWebsiteErr.innerHTML = "";
			applicationWebsiteOk = true;
			checkForm();
			return;
		}
	} else {
		console.log("Empty URL");
		element.classList.remove("input-invalid");
		element.classList.add("input-valid");
		applicationWebsiteErr.innerHTML = "";
		applicationWebsiteOk = true;
		checkForm();
		return;
	}
}