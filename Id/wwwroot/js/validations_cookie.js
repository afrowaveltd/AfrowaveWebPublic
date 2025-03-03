// References to error message containers for each cookie-related field.
const nameError = document.getElementById('cookie_name_err');
const domainError = document.getElementById('cookie_domain_err');
const pathError = document.getElementById('cookie_path_err');
const secureError = document.getElementById('cookie_secure_err');
const sameSiteError = document.getElementById('cookie_same_site_err');
const httpOnlyError = document.getElementById('cookie_http_only_err');
const expirationError = document.getElementById('cookie_expiration_err');
const submitButton = document.getElementById('submit');

// Track validation status for each field.
let nameOk = false;
let domainOk = false;
let pathOk = false;
let secureOk = false;
let sameSiteOk = false;
let httpOnlyOk = false;
let expirationOk = false;

// Overall form validation status.
let formOk = false;

/**
 * Re-evaluates overall form validity based on the status of each field.
 * Enables or disables the submit button based on the form's validity.
 */
const checkForm = () => {
	(nameOk && domainOk && pathOk && secureOk && sameSiteOk && httpOnlyOk && expirationOk)
		? formOk = true
		: formOk = false;

	formOk
		? submitButton.removeAttribute('disabled')
		: submitButton.setAttribute('disabled', 'disabled');
}

/**
 * Marks a field as valid.
 * Updates the field's CSS classes and clears any existing error message.
 * @param {HTMLElement} element - The field to mark as valid.
 * @param {HTMLElement} errorElement - The associated error message container.
 */
const setValid = (element, errorElement) => {
	element.classList.remove('input-invalid');
	element.classList.add('input-valid');
	errorElement.innerHTML = '';
}

/**
 * Marks a field as invalid.
 * Updates the field's CSS classes and displays the provided error message.
 * @param {HTMLElement} element - The field to mark as invalid.
 * @param {HTMLElement} errorElement - The associated error message container.
 * @param {string} message - The error message to display.
 */
const setInvalid = (element, errorElement, message) => {
	element.classList.remove('input-valid');
	element.classList.add('input-invalid');
	errorElement.innerHTML = message;
}

/**
 * Validates the cookie name field.
 * Ensures the name is present and meets character requirements.
 * Updates validation status and triggers form re-check.
 * @param {HTMLElement} element - The input field for the cookie name.
 */
const checkName = async (element) => {
	if (element.value.length < 1) {
		setInvalid(element, nameError, await localize('Name is required'));
		nameOk = false;
	} else {
		const regex = /^[a-zA-Z0-9_.]+$/;
		if (!regex.test(element.value)) {
			setInvalid(element, nameError, await localize('Name must contain only letters, numbers, underscores and dot'));
			nameOk = false;
		} else {
			setValid(element, nameError);
			nameOk = true;
		}
	}
	checkForm();
}

/**
 * Validates the cookie domain field.
 * Ensures the domain is present and meets character requirements.
 * Updates validation status and triggers form re-check.
 * @param {HTMLElement} element - The input field for the cookie domain.
 */
const checkDomain = async (element) => {
	if (element.value.length < 1) {
		setInvalid(element, domainError, await localize('Domain is required'));
		domainOk = false;
	} else {
		const regex = /^[a-zA-Z0-9_.]+$/;
		if (!regex.test(element.value)) {
			setInvalid(element, domainError, await localize('Domain must contain only letters, numbers and underscores'));
			domainOk = false;
		} else {
			setValid(element, domainError);
			domainOk = true;
		}
	}
	checkForm();
}

/**
 * Validates the cookie path field.
 * Ensures the path is valid, auto-filling it with "/" if empty.
 * Updates validation status and triggers form re-check.
 * @param {HTMLElement} element - The input field for the cookie path.
 */
const checkPath = async (element) => {
	if (element.value.length < 1) {
		element.value = '/';
	}

	const regex = /^[a-zA-Z0-9_./]+$/;
	if (!regex.test(element.value)) {
		setInvalid(element, pathError, await localize('Path must contain only letters, numbers and underscores'));
		pathOk = false;
	} else {
		setValid(element, pathError);
		pathOk = true;
	}
	checkForm();
}

/**
 * Validates the secure flag for the cookie.
 * Marks the field as valid regardless, but provides a warning if less secure.
 * @param {HTMLElement} element - The select field for the Secure flag.
 */
const checkSecure = async (element) => {
	if (element.value == 'true') {
		setValid(element, secureError);
		secureOk = true;
	} else {
		setInvalid(element, secureError, await localize('This option is less secure'));
		secureOk = true;  // This allows non-secure cookies but marks the form valid.
	}
	checkForm();
}

/**
 * Validates the SameSite attribute for the cookie.
 * Provides a warning if set to the least secure option.
 * @param {HTMLElement} element - The select field for the SameSite attribute.
 */
const checkSameSite = async (element) => {
	if (element.value == '0') {
		setInvalid(element, sameSiteError, await localize('This option is less secure'));
		sameSiteOk = true;  // Still allowing the choice, but with a warning.
	} else {
		setValid(element, sameSiteError);
		sameSiteOk = true;
	}
	checkForm();
}

/**
 * Validates the HttpOnly flag for the cookie.
 * Provides a warning if not set.
 * @param {HTMLElement} element - The select field for the HttpOnly flag.
 */
const checkHttpOnly = async (element) => {
	if (element.value == 'true') {
		setValid(element, httpOnlyError);
		httpOnlyOk = true;
	} else {
		setInvalid(element, httpOnlyError, await localize('This option is less secure'));
		httpOnlyOk = true;  // Allows insecure configuration but warns.
	}
	checkForm();
}

/**
 * Validates the cookie expiration time.
 * Ensures the value is a non-negative number.
 * Updates validation status and triggers form re-check.
 * @param {HTMLElement} element - The input field for the expiration time.
 */
const checkExpiration = async (element) => {
	if (element.value < 0) {
		setInvalid(element, expirationError, await localize('The value must be a positive number or zero'));
		expirationOk = false;
	} else {
		setValid(element, expirationError);
		expirationOk = true;
	}
	checkForm();
}

/**
 * Initializes validation on page load by pre-checking all fields.
 * Ensures the initial form state is correctly set based on existing values.
 */
const startup = async () => {
	const name = document.getElementById('cookie_name');
	const domain = document.getElementById('cookie_domain');
	const path = document.getElementById('cookie_path');
	const secure = document.getElementById('cookie_secure');
	const sameSite = document.getElementById('cookie_same_site');
	const httpOnly = document.getElementById('cookie_http_only');
	const expiration = document.getElementById('cookie_expiration');
	await checkName(name);
	await checkDomain(domain);
	await checkPath(path);
	await checkSecure(secure);
	await checkSameSite(sameSite);
	await checkHttpOnly(httpOnly);
	await checkExpiration(expiration);
}