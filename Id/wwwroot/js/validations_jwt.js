// References to error message containers for each JWT-related field.
const issuerError = document.getElementById('issuer_err');
const audienceError = document.getElementById('audience_err');
const accessTokenExpirationError = document.getElementById('access_token_expiration_err');
const refreshTokenExpirationError = document.getElementById('refresh_token_expiration_err');
const submitButton = document.getElementById('submit');

// Track validation status of each individual field.
let issuerOk = false;
let audienceOk = false;
let accessTokenExpirationOk = false;
let refreshTokenExpirationOk = false;

// Overall form validation status.
let formOk = false;

/**
 * Re-evaluates the form's overall validity based on the state of each field.
 * Enables or disables the submit button based on validation result.
 */
const checkForm = () => {
	console.log("checking form");

	// Set formOk to true only if all individual fields are valid.
	(issuerOk && audienceOk && accessTokenExpirationOk && refreshTokenExpirationOk)
		? formOk = true
		: formOk = false;

	// Enable or disable submit button depending on overall form validity.
	formOk
		? submitButton.removeAttribute('disabled')
		: submitButton.setAttribute('disabled', 'disabled');
}

/**
 * Marks a field as valid: applies valid styling and clears error message.
 * @param {HTMLElement} element - The input field being validated.
 * @param {HTMLElement} errorElement - The associated error message container.
 */
const setValid = (element, errorElement) => {
	element.classList.remove('input-invalid');
	element.classList.add('input-valid');
	errorElement.innerHTML = '';
}

/**
 * Marks a field as invalid: applies invalid styling and displays error message.
 * @param {HTMLElement} element - The input field being validated.
 * @param {HTMLElement} errorElement - The associated error message container.
 * @param {string} message - The error message to display.
 */
const setInvalid = (element, errorElement, message) => {
	element.classList.remove('input-valid');
	element.classList.add('input-invalid');
	errorElement.innerHTML = message;
}

/**
 * Validates the issuer field to ensure it's non-empty and only contains allowed characters.
 * @param {HTMLElement} element - The input field for the issuer.
 */
const checkIssuer = async (element) => {
	if (element.value.length < 1) {
		setInvalid(element, issuerError, await localize('Issuer is required'));
		issuerOk = false;
	} else {
		// Ensure the issuer contains only allowed characters.
		const regex = /^[a-zA-Z0-9_.]+$/;
		if (!regex.test(element.value)) {
			setInvalid(element, issuerError, await localize('Issuer must contain only letters, numbers, underscores, and dot'));
			issuerOk = false;
		} else {
			setValid(element, issuerError);
			issuerOk = true;
		}
	}
	checkForm();
}

/**
 * Validates the audience field to ensure it's non-empty and only contains allowed characters.
 * @param {HTMLElement} element - The input field for the audience.
 */
const checkAudience = async (element) => {
	if (element.value.length < 1) {
		setInvalid(element, audienceError, await localize('Audience is required'));
		audienceOk = false;
	} else {
		// Ensure the audience contains only allowed characters (including slashes for URL-like values).
		const regex = /^[a-zA-Z0-9_.*/]+$/;
		if (!regex.test(element.value)) {
			setInvalid(element, audienceError, await localize('Audience must contain only letters, numbers, underscores, dashes, slashes, and dot'));
			audienceOk = false;
		} else {
			setValid(element, audienceError);
			audienceOk = true;
		}
	}
	checkForm();
}

/**
 * Validates the access token expiration time, ensuring it's non-negative.
 * @param {HTMLElement} element - The input field for access token expiration time.
 */
const checkAccessTokenExpiration = async (element) => {
	if (element.value < 0) {
		setInvalid(element, accessTokenExpirationError, await localize('The value must be a positive number or zero'));
		accessTokenExpirationOk = false;
	} else {
		setValid(element, accessTokenExpirationError);
		accessTokenExpirationOk = true;
	}
	checkForm();
}

/**
 * Validates the refresh token expiration time, ensuring it's non-negative.
 * @param {HTMLElement} element - The input field for refresh token expiration time.
 */
const checkRefreshTokenExpiration = async (element) => {
	if (element.value < 0) {
		setInvalid(element, refreshTokenExpirationError, await localize('The value must be a positive number or zero'));
		refreshTokenExpirationOk = false;
	} else {
		setValid(element, refreshTokenExpirationError);
		refreshTokenExpirationOk = true;
	}
	checkForm();
}

/**
 * Initial function called on page load to validate all fields immediately.
 * Ensures form starts in a correctly validated state if values are pre-filled.
 */
const startup = async () => {
	console.log("Starting initial validation...");

	// Get all the form fields.
	const issuer = document.getElementById('issuer');
	const audience = document.getElementById('audience');
	const accessTokenExpiration = document.getElementById('access_token');
	const refreshTokenExpiration = document.getElementById('refresh_token');

	// Run initial validation on all fields.
	await checkIssuer(issuer);
	await checkAudience(audience);
	await checkAccessTokenExpiration(accessTokenExpiration);
	await checkRefreshTokenExpiration(refreshTokenExpiration);
}