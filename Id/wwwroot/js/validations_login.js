// References to error containers for each login rule setting.
const maxFailedError = document.getElementById('login_rules_err_max_failed');
const lockoutTimeError = document.getElementById('login_rules_err_lockout_time');
const passwordTokenError = document.getElementById('login_rules_err_password_reset_token_expiration');
const emailTokenError = document.getElementById('login_rules_err_email_token_expiration');
const requireConfirmation = document.getElementById('login_rules_err_require_email_confirmation');
const submitButton = document.getElementById('submit');

// Track the validation status of each field.
let maxFailedOk = true;
let lockoutTimeOk = true;
let passwordTokenOk = true;
let emailTokenOk = true;
let requireConfirmationOk = true;

// Stores dynamic warning messages (set within each check function).
let warning = '';

/**
 * Re-evaluates overall form validity based on all individual field statuses.
 * Enables or disables the submit button accordingly.
 */
const checkForm = () => {
	(maxFailedOk && lockoutTimeOk && passwordTokenOk && emailTokenOk && requireConfirmationOk)
		? formOk = true
		: formOk = false;

	formOk
		? submitButton.removeAttribute('disabled')
		: submitButton.setAttribute('disabled', 'disabled');
}

/**
 * Marks a field as valid with proper styling and clears error message.
 * @param {HTMLElement} element - The input field.
 * @param {HTMLElement} errorElement - The associated error message container.
 */
const setValid = (element, errorElement) => {
	element.classList.remove('input-invalid');
	element.classList.add('input-valid');
	errorElement.innerHTML = '';
}

/**
 * Marks a field as invalid with proper styling and displays a message.
 * @param {HTMLElement} element - The input field.
 * @param {HTMLElement} errorElement - The associated error message container.
 * @param {string} message - The warning or error message.
 */
const setInvalid = (element, errorElement, message) => {
	element.classList.remove('input-valid');
	element.classList.add('input-invalid');
	errorElement.innerHTML = message;
}

/**
 * Validates numeric fields with a set of common rules.
 * Allows for additional warnings for certain values (e.g., 0 being risky).
 * @param {HTMLElement} element - The input field.
 * @param {HTMLElement} errorElement - The associated error container.
 * @param {Object} options - Custom warning messages for edge cases.
 * @returns {Promise<boolean>} - Validation status.
 */
const checkNumericField = async (element, errorElement, options = {}) => {
	const value = parseInt(element.value, 10);

	if (isNaN(value) || value < 0) {
		setInvalid(element, errorElement, await localize("The value must be a positive number or zero"));
		return false;
	} else if (value === 0 && options.zeroWarning) {
		setInvalid(element, errorElement, await localize(options.zeroWarning));
		return true; // Valid but with a warning
	} else if (options.lowValueWarning && value < options.lowValueThreshold) {
		setInvalid(element, errorElement, await localize(options.lowValueWarning));
		return true; // Valid but with a warning
	} else {
		setValid(element, errorElement);
		return true;
	}
}

/**
 * Checks the max failed login attempts rule.
 * Provides warnings for risky values like 0 (disabling lockout) or very low thresholds.
 */
const checkMaxFailed = async (element) => {
	maxFailedOk = await checkNumericField(element, maxFailedError, {
		zeroWarning: "The max failed login attempts set to 0 will disable account locking out",
		lowValueWarning: "The maximum failed login attempts set to less than 3 is not recommended",
		lowValueThreshold: 3
	});
	checkForm();
}

/**
 * Checks the account lockout time rule.
 * Warns if set to 0 (indefinite lockout).
 */
const checkLockoutTime = async (element) => {
	lockoutTimeOk = await checkNumericField(element, lockoutTimeError, {
		zeroWarning: "The account lockout time set to 0 will lock out accounts indefinitely"
	});
	checkForm();
}

/**
 * Checks the password reset token expiration rule.
 * Warns if set to 0 (valid until used).
 */
const checkPasswordTokenExpiration = async (element) => {
	passwordTokenOk = await checkNumericField(element, passwordTokenError, {
		zeroWarning: "The password reset token expiration set to 0 will make token valid until it is used"
	});
	checkForm();
}

/**
 * Checks the email confirmation token expiration rule.
 * Warns if set to 0 (valid until used).
 */
const checkEmailTokenExpiration = async (element) => {
	emailTokenOk = await checkNumericField(element, emailTokenError, {
		zeroWarning: "The email confirmation token expiration set to 0 will make token valid until it is used"
	});
	checkForm();
}

/**
 * Checks whether email confirmation is required.
 * Displays a warning if not required, but allows the configuration.
 */
const checkRequireConfirmedEmail = async (element) => {
	if (element.value === "true") {
		setValid(element, requireConfirmation);
		requireConfirmationOk = true;
	} else {
		setInvalid(element, requireConfirmation, await localize("Disabling email confirmation will allow users to login without confirming their email"));
		requireConfirmationOk = true;  // It's a warning, not a block.
	}
	checkForm();
}